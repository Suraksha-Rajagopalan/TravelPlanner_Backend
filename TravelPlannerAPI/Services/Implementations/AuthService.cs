using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;

public class AuthService : IAuthService
{
    //private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        //_authRepository = authRepository;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto> SignupAsync(SignupRequest request)
    {
        var existingUser = await _unitOfWork.Auth.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponseDto ( false, "Email already registered", null, null );
        }

        var newUser = new UserModel
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name,
            Role = "User",
            IsAdmin = false,
            IsActive = true
        };

        var (succeeded, errors) = await _unitOfWork.Auth.CreateUserAsync(newUser, request.Password);
        if (!succeeded)
        {
            return new AuthResponseDto ( false, null, null, errors);
        }

        return new AuthResponseDto ( true, $"User {request.Name} registered successfully!", null, null );
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork.Auth.GetByEmailAsync(request.Email);
        if (user == null || !await _unitOfWork.Auth.CheckPasswordAsync(user, request.Password))
        {
            return new AuthResponseDto (false, "Invalid credentials", null, null );
        }

        user.LastLoginDate = DateTime.UtcNow;
        await _unitOfWork.Auth.UpdateUserAsync(user);

        var accessToken = GenerateJwtToken(user);

        return new AuthResponseDto
        (
            true, null, null,
            new
            {
                Token = accessToken,
                User = new
                {
                    id = user.Id,
                    username = user.Name,
                    email = user.Email
                }
            }
        );
    }

    public string GenerateJwtToken(UserModel user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
