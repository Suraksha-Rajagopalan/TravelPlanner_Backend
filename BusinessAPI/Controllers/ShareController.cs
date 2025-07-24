using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/tripshare")]
[Authorize]
public class TripShareController : ControllerBase
{
    private readonly ITripShareService _tripShareService;
    private readonly UserManager<User> _userManager;

    public TripShareController(ITripShareService tripShareService, UserManager<User> userManager)
    {
        _tripShareService = tripShareService;
        _userManager = userManager;
    }

    [HttpPost("share")]
    public async Task<IActionResult> ShareTrip([FromBody] TripShareRequestDto request)
    {
        var ownerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(ownerEmail))
            return Unauthorized("User email not found in token.");

        var owner = await _userManager.FindByEmailAsync(ownerEmail);
        if (owner == null) return Unauthorized();

        var sharedWithUser = await _userManager.FindByEmailAsync(request.SharedWithEmail);
        if (sharedWithUser == null)
            return BadRequest("User to share with not found.");

        if (sharedWithUser.Id == owner.Id)
            return BadRequest("You cannot share a trip with yourself.");

        var (success, errorMessage) = await _tripShareService.ShareTripAsync(owner.Id, request);
        if (!success)
            return BadRequest(errorMessage);

        return Ok("Trip shared successfully.");
    }


    [HttpGet("shared-with-me")]
    public async Task<IActionResult> GetTripsSharedWithMe()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized("User email not found in token.");

        var currentUser = await _userManager.FindByEmailAsync(userEmail);
        if (currentUser == null)
            return Unauthorized("User not found.");

        var sharedTrips = await _tripShareService.GetTripsSharedWithUserAsync(currentUser.Id);
        return Ok(sharedTrips);
    }
}
