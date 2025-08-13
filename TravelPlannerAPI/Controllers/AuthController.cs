using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelPlannerAPI.Controllers;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services.Interfaces;

[ApiVersion("1.0")]
//[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly IGenericRepository<UserModel> _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IAuthService _authService;

    public AuthController(
        UserManager<UserModel> userManager,
        SignInManager<UserModel> signInManager,
        IConfiguration configuration,
        ILogger<AuthController> logger,
        IGenericRepository<UserModel> userRepository,
        ITokenService tokenService,
        IAuthService authService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _authService = authService;
    }


    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return BadRequest(new { message = "Email already registered" });

        var newUser = new UserModel
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name,
            IsAdmin = false,
            Role = "User",
            IsActive = true
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = $"User {request.Name} registered successfully!" });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized(new { message = "Invalid credentials" });

        user.LastLoginDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var (accessToken, accessExpiry) = await _tokenService.GenerateAccessToken(user);
        var (refreshToken, refreshExpiry) = await _tokenService.GenerateRefreshToken(user); // also saved to DB by your service

        // Refresh token should be HttpOnly (JS cannot read) and Secure in production (HTTPS).
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Expires = refreshExpiry
        });

        Response.Cookies.Append("jwtToken", accessToken, new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Expires = accessExpiry ?? DateTime.UtcNow.AddHours(2)
        });

        return Ok(new
        {
            accessToken,
            accessExpiry,
            refreshToken,       // you can omit this from body if you rely on cookie only
            refreshExpiry,
            user = new { id = user.Id, username = user.Name, email = user.Email, role = user.Role }
        });
    }



    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequestDto request)
    {
        var accessToken = HttpContext.Request.Cookies["jwtToken"];
        var refreshToken = HttpContext.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token is missing from cookies." });

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken, false);
        var refreshPrincipal = _tokenService.GetPrincipalFromExpiredToken(refreshToken, true);

        if (principal == null || refreshPrincipal == null)
            return Unauthorized(new { message = "Invalid token(s)" });

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var refreshUserId = refreshPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null || userId != refreshUserId)
            return Unauthorized(new { message = "Token mismatch" });

        // refresh token exists and is not expired in DB
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId && u.RefreshToken == refreshToken);

        if (user == null)
            return Unauthorized(new { message = "User not found or token mismatch" });

        if (user.RefreshTokenExpiry == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
            return Unauthorized(new { message = "Refresh token expired" });

        // Rotate tokens
        var (newAccessToken, newAccessExpiry) = await _tokenService.GenerateAccessToken(user);
        var (newRefreshToken, newRefreshExpiry) = await _tokenService.GenerateRefreshToken(user); // updates DB

        Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Expires = newRefreshExpiry
        });

        Response.Cookies.Append("jwtToken", newAccessToken, new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Expires = newAccessExpiry ?? DateTime.UtcNow.AddHours(2)
        });

        return Ok(new
        {
            accessToken = newAccessToken,
            accessExpiry = newAccessExpiry,
            refreshToken = newRefreshToken,
            refreshExpiry = newRefreshExpiry,
            user = new { id = user.Id, username = user.Name, email = user.Email, role = user.Role }
        });
    }

    [HttpPut("update-profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var res = await _authService.UpdateProfileAsync(userId, request);
        if (!res.Success) return BadRequest(new { message = res.Message });

        return Ok(res.Data);
    }


}