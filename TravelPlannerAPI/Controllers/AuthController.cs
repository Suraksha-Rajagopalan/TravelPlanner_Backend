using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly IGenericRepository<User> _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IAuthService _authService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        ILogger<AuthController> logger,
        IGenericRepository<User> userRepository,
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

        var newUser = new User
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

        var accessToken = _authService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);


        return Ok(new
        {
            accessToken,
            refreshToken,
            user = new
            {
                id = user.Id,
                username = user.Name,
                email = user.Email,
                role = user.Role
            }
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequestDto request)
    {
        // Get tokens
        var accessToken = request.AccessToken;
        var refreshToken = HttpContext.Request.Cookies["refreshToken"]; // Only from cookie

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token is missing from cookies." });

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken, false);
        var refreshPrincipal = _tokenService.GetPrincipalFromExpiredToken(refreshToken, true);

        if (principal == null || refreshPrincipal == null)
            return Unauthorized(new { message = "Invalid token(s)" });

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var refreshUserId = refreshPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != refreshUserId)
            return Unauthorized(new { message = "Token mismatch" });

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized(new { message = "User not found" });

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken(user);

        Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        Response.Cookies.Append("jwtToken", newAccessToken, new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(1)
        });

        return Ok(new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshToken 
        });
    }




}