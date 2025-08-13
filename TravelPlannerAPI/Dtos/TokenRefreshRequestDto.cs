namespace TravelPlannerAPI.Dtos
{
    public record TokenRefreshRequestDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}