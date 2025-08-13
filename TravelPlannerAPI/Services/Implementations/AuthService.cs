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
    private readonly ITokenService _tokenService;

    public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        //_authRepository = authRepository;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> SignupAsync(SignupRequest request)
    {
        var existingUser = await _unitOfWork.Auth.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponseDto(false, "Email already registered", null, null);
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
            return new AuthResponseDto(false, null, null, errors);
        }

        return new AuthResponseDto(true, $"User {request.Name} registered successfully!", null, null);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork.Auth.GetByEmailAsync(request.Email);
        if (user == null || !await _unitOfWork.Auth.CheckPasswordAsync(user, request.Password))
        {
            return new AuthResponseDto(false, "Invalid credentials", null, null);
        }

        user.LastLoginDate = DateTime.UtcNow;
        await _unitOfWork.Auth.UpdateUserAsync(user);

        var (accessToken, accessTokenExpiry) = await _tokenService.GenerateAccessToken(user);

        return new AuthResponseDto
        (
            true, null, null,
            new
            {
                Token = accessToken,
                Expiry = accessTokenExpiry,
                User = new
                {
                    id = user.Id,
                    username = user.Name,
                    email = user.Email
                }
            }
        );
    }
    public async Task<AuthResponseDto> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await _unitOfWork.Auth.GetByIdAsync(userId); // you may need to add this to repo
        if (user == null) return new AuthResponseDto(false, "User not found", null, null);

        if (!string.IsNullOrWhiteSpace(dto.Username)) user.Name = dto.Username;
        if (!string.IsNullOrWhiteSpace(dto.Email)) user.Email = dto.Email;
        if (!string.IsNullOrWhiteSpace(dto.Phone)) user.PhoneNumber = dto.Phone;
        user.ProfileImage = dto.ProfileImage ?? user.ProfileImage;
        user.Address = dto.Address ?? user.Address;
        user.Bio = dto.Bio ?? user.Bio;

        await _unitOfWork.Auth.UpdateUserAsync(user);
        // If your UpdateUserAsync uses UserManager.UpdateAsync it already persists,
        // otherwise call await _unitOfWork.CompleteAsync();

        return new AuthResponseDto(true, "Profile updated", null, new
        {
            id = user.Id,
            username = user.Name,
            email = user.Email,
            phone = user.PhoneNumber,
            profileImage = user.ProfileImage,
            address = user.Address,
            bio = user.Bio
        });
    }

}