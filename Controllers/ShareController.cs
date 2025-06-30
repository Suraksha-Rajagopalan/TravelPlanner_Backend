using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Models.Enums;
using Microsoft.AspNetCore.Identity;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TripShareController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public TripShareController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Share a trip with another registered user in TravelPlanner.
    /// </summary>
    [HttpPost("share")]
    public async Task<IActionResult> ShareTrip([FromBody] TripShareRequestDto request)
    {
        try
        {
            Console.WriteLine("[ShareTrip] START");
            Console.WriteLine($"[ShareTrip] Incoming Request: TripId={request.TripId}, SharedWith={request.SharedWithEmail}, AccessLevel={request.AccessLevel}");

            var ownerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            Console.WriteLine($"[ShareTrip] Extracted ownerEmail: {ownerEmail}");

            if (string.IsNullOrEmpty(ownerEmail))
                return Unauthorized("User email not found in token.");

            var owner = await _userManager.FindByEmailAsync(ownerEmail);
            if (owner == null)
            {
                Console.WriteLine("[ShareTrip] Owner user not found in database.");
                return Unauthorized();
            }

            Console.WriteLine($"[ShareTrip] Owner found: ID={owner.Id}, Email={owner.Email}");
            int ownerId = owner.Id;

            var sharedWithUser = await _userManager.FindByEmailAsync(request.SharedWithEmail);
            if (sharedWithUser == null)
            {
                Console.WriteLine("[ShareTrip] SharedWithUser not found.");
                return BadRequest("User to share with not found or not registered.");
            }

            Console.WriteLine($"[ShareTrip] SharedWithUser found: ID={sharedWithUser.Id}, Email={sharedWithUser.Email}");

            if (sharedWithUser.Id == owner.Id)
            {
                Console.WriteLine("[ShareTrip] Attempted to share with self.");
                return BadRequest("You cannot share a trip with yourself.");
            }

            var existingShare = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == request.TripId && s.SharedWithUserId == sharedWithUser.Id);

            if (existingShare != null)
            {
                Console.WriteLine("[ShareTrip] Trip already shared with this user.");
                return BadRequest("This trip is already shared with the user.");
            }

            if (!Enum.TryParse<AccessLevel>(request.AccessLevel, true, out var accessLevel))
            {
                Console.WriteLine("[ShareTrip] Invalid access level provided.");
                return BadRequest("Invalid access level.");
            }

            Console.WriteLine($"[ShareTrip] Access level parsed: {accessLevel}");

            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.Id == request.TripId && t.UserId == ownerId);

            if (trip == null)
            {
                Console.WriteLine($"[ShareTrip] Trip not found for Id={request.TripId} and UserId={ownerId}");
                return NotFound("Trip not found or you do not have permission to share it.");
            }

            Console.WriteLine($"[ShareTrip] Trip found: ID={trip.Id}, Title={trip.Title}");

            var share = new TripShare
            {
                TripId = request.TripId,
                OwnerId = owner.Id,
                SharedWithUserId = sharedWithUser.Id,
                AccessLevel = accessLevel
            };

            _context.TripShares.Add(share);
            Console.WriteLine("[ShareTrip] Share object added to context.");

            await _context.SaveChangesAsync();
            Console.WriteLine("[ShareTrip] Changes saved to database.");

            return Ok("Trip shared successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ShareTrip] ERROR: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get all trips shared with the current user.
    /// </summary>
    [HttpGet("shared-with-me")]
    public async Task<IActionResult> GetTripsSharedWithMe()
    {
        try
        {
            Console.WriteLine("[GetTripsSharedWithMe] START");

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            Console.WriteLine($"[GetTripsSharedWithMe] Extracted Email: {userEmail}");

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("User email not found in token.");

            var currentUser = await _userManager.FindByEmailAsync(userEmail);
            if (currentUser == null)
            {
                Console.WriteLine("[GetTripsSharedWithMe] User not found.");
                return Unauthorized("User not found.");
            }

            Console.WriteLine($"[GetTripsSharedWithMe] Current user: ID={currentUser.Id}, Email={currentUser.Email}");

            var sharedTrips = await _context.TripShares
                .Where(s => s.SharedWithUserId == currentUser.Id)
                .Include(s => s.Trip)
                .Select(s => new
                {
                    s.TripId,
                    s.AccessLevel,
                    TitleName = s.Trip.Title,
                    Destination = s.Trip.Destination,
                    StartDate = s.Trip.StartDate,
                    EndDate = s.Trip.EndDate,
                    TripDescription = s.Trip.Description,
                    TravelMode = s.Trip.TravelMode,
                    TripBudget = s.Trip.Budget,
                    TripNotes = s.Trip.Notes,
                    TripImage = s.Trip.Image,
                    Duration = s.Trip.Duration,
                    BestTime = s.Trip.BestTime,
                    Essentials = s.Trip.Essentials ?? new List<string>(),
                    TouristSpots = s.Trip.TouristSpots ?? new List<string>(),
                })
                .ToListAsync();

            Console.WriteLine($"[GetTripsSharedWithMe] Total shared trips found: {sharedTrips.Count}");

            return Ok(sharedTrips);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GetTripsSharedWithMe] ERROR: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
