namespace TravelPlannerAPI.Dtos
{
    public record AuthResponseDto(bool Success, string? Message, object? Data, object? Errors);
}
