using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Models.Enums;
using BusinessAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Implementations
{
    public class ChecklistService : IChecklistService
    {
        private readonly ApplicationDbContext _context;

        public ChecklistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool CanView, bool CanEdit)> GetAccessAsync(int tripId, int userId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) return (false, false);

            if (trip.UserId == userId)
                return (true, true);

            var share = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

            if (share == null)
                return (false, false);

            return (true, share.AccessLevel == AccessLevel.Edit);
        }

        public async Task<IEnumerable<ChecklistItemDto>> GetChecklistAsync(int tripId, int userId)
        {
            var (canView, _) = await GetAccessAsync(tripId, userId);
            if (!canView) return null;

            return await _context.ChecklistItems
                .Where(item => item.TripId == tripId)
                .Select(item => new ChecklistItemDto
                {
                    Id = item.Id,
                    TripId = item.TripId,
                    Description = item.Text,
                    IsCompleted = item.IsCompleted,
                    UserId = item.UserId
                })
                .ToListAsync();
        }

        public async Task<ChecklistItemDto> AddItemAsync(int tripId, ChecklistItemDto itemDto, int userId)
        {
            var (_, canEdit) = await GetAccessAsync(tripId, userId);
            if (!canEdit) return null;

            var item = new ChecklistItem
            {
                TripId = tripId,
                Text = itemDto.Description,
                IsCompleted = itemDto.IsCompleted,
                UserId = userId
            };

            _context.ChecklistItems.Add(item);
            await _context.SaveChangesAsync();

            itemDto.Id = item.Id;
            itemDto.UserId = userId;

            return itemDto;
        }

        public async Task<bool> UpdateItemAsync(int tripId, int id, ChecklistItemUpdateDto updateDto, int userId)
        {
            var item = await _context.ChecklistItems.FindAsync(id);
            if (item == null || item.TripId != tripId)
                return false;

            var (_, canEdit) = await GetAccessAsync(tripId, userId);
            if (!canEdit) return false;

            item.Text = updateDto.Description;
            item.IsCompleted = updateDto.IsCompleted;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItemAsync(int tripId, int id, int userId)
        {
            var item = await _context.ChecklistItems.FindAsync(id);
            if (item == null || item.TripId != tripId)
                return false;

            var (_, canEdit) = await GetAccessAsync(tripId, userId);
            if (!canEdit) return false;

            _context.ChecklistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ChecklistItemDto>> GetSharedTripChecklistAsync(int tripId, int userId)
        {
            var share = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

            if (share == null || share.AccessLevel != AccessLevel.View)
                return null;

            return await _context.ChecklistItems
                .Where(item => item.TripId == tripId)
                .Select(item => new ChecklistItemDto
                {
                    Id = item.Id,
                    TripId = item.TripId,
                    Description = item.Text,
                    IsCompleted = item.IsCompleted,
                    UserId = item.UserId
                })
                .ToListAsync();
        }
    }
}
