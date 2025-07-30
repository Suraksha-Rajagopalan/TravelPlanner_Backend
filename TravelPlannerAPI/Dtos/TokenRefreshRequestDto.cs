namespace TravelPlannerAPI.Dtos
{
    public record TokenRefreshRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
