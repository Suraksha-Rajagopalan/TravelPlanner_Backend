﻿namespace BusinessAPI.Dtos
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public object? Errors { get; set; }
    }
}
