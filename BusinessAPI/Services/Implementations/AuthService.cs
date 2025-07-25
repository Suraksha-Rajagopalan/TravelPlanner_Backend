﻿using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> SignupAsync(SignupRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto { Success = false, Message = "Email already registered" };
            }

            var newUser = new User
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                Role = "User",
                IsAdmin = false,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResponseDto { Success = false, Errors = result.Errors };
            }

            return new AuthResponseDto { Success = true, Message = $"User {request.Name} registered successfully!" };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new AuthResponseDto { Success = false, Message = "Invalid credentials" };
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var accessToken = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Data = new
                {
                    Token = accessToken,
                    User = new
                    {
                        id = user.Id,
                        username = user.Name,
                        email = user.Email
                    }
                }
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim("nameid", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
