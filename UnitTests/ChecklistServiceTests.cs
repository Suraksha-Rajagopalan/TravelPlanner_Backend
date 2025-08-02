using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Enums;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Implementations;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;
using Xunit;

namespace UnitTests
{
    public class ChecklistServiceTests
    {
        private readonly Mock<IChecklistRepository> _repoMock;
        private readonly Mock<IAccessService> _accessMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly ChecklistService _service;

        public ChecklistServiceTests()
        {
            _repoMock = new Mock<IChecklistRepository>();
            _accessMock = new Mock<IAccessService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChecklistItemModel, ChecklistItemDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _service = new ChecklistService(_repoMock.Object, _accessMock.Object, _mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetChecklistAsync_ShouldReturnChecklist_WhenUserHasAccess()
        {
            int tripId = 1, userId = 2;
            var items = new List<ChecklistItemModel>
            {
                new ChecklistItemModel { Id = 1, Description = "Item 1", TripId = tripId, UserId = userId },
                new ChecklistItemModel { Id = 2, Description = "Item 2", TripId = tripId, UserId = userId }
            };

            _accessMock.Setup(x => x.HasAccessToTripAsync(tripId, userId)).ReturnsAsync(true);
            _repoMock.Setup(x => x.GetByTripAndUserAsync(tripId, userId)).ReturnsAsync(items);
            _accessMock.Setup(x => x.GetAccessLevelAsync(tripId, userId)).ReturnsAsync(AccessLevel.Edit);

            var result = await _service.GetChecklistAsync(tripId, userId);

            result.Should().NotBeNull();
            result.AccessLevel.Should().Be("Edit");
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetChecklistAsync_ShouldReturnNull_WhenUserHasNoAccess()
        {
            _accessMock.Setup(x => x.HasAccessToTripAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await _service.GetChecklistAsync(1, 1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddItemAsync_ShouldAddItem_WhenUserHasAccess()
        {
            var dto = new ChecklistItemDto { TripId = 1, Description = "New Item" };
            var entity = new ChecklistItemModel { Id = 3, TripId = 1, Description = "New Item", UserId = 2 };

            _accessMock.Setup(x => x.HasAccessToTripAsync(dto.TripId, 2)).ReturnsAsync(true);
            _repoMock.Setup(x => x.AddAsync(It.IsAny<ChecklistItemModel>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(1);


            var result = await _service.AddItemAsync(dto, 2);

            result.Should().NotBeNull();
            result.Description.Should().Be("New Item");
        }

        [Fact]
        public async Task AddItemAsync_ShouldReturnNull_WhenUserHasNoAccess()
        {
            var dto = new ChecklistItemDto { TripId = 1, Description = "New Item" };
            _accessMock.Setup(x => x.HasAccessToTripAsync(dto.TripId, 2)).ReturnsAsync(false);

            var result = await _service.AddItemAsync(dto, 2);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateItemAsync_ShouldUpdate_WhenValid()
        {
            var dto = new ChecklistItemUpdateDto { Description = "Updated", IsCompleted = true };
            var item = new ChecklistItemModel { Id = 4, TripId = 1, UserId = 2, Description = "Old", IsCompleted = false };

            _repoMock.Setup(x => x.GetByIdAsync(4)).ReturnsAsync(item);
            _accessMock.Setup(x => x.HasAccessToTripAsync(item.TripId, 2)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(1);


            var result = await _service.UpdateItemAsync(4, dto, 2);

            result.Should().NotBeNull();
            result.Description.Should().Be("Updated");
            result.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateItemAsync_ShouldReturnNull_WhenInvalid()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((ChecklistItemModel)null);

            var result = await _service.UpdateItemAsync(1, new ChecklistItemUpdateDto(), 2);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteItemAsync_ShouldDelete_WhenValid()
        {
            var item = new ChecklistItemModel { Id = 4, TripId = 1, UserId = 2 };

            _repoMock.Setup(x => x.GetByIdAsync(4)).ReturnsAsync(item);
            _accessMock.Setup(x => x.HasAccessToTripAsync(item.TripId, 2)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(1);


            var result = await _service.DeleteItemAsync(4, 2);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteItemAsync_ShouldReturnFalse_WhenInvalid()
        {
            _repoMock.Setup(x => x.GetByIdAsync(5)).ReturnsAsync((ChecklistItemModel)null);

            var result = await _service.DeleteItemAsync(5, 2);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ToggleCompletionAsync_ShouldToggle_WhenValid()
        {
            var item = new ChecklistItemModel { Id = 3, TripId = 1, UserId = 2, IsCompleted = false };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(item);
            _accessMock.Setup(x => x.HasAccessToTripAsync(item.TripId, 2)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.ToggleCompletionAsync(1, 2);

            result.Should().NotBeNull();
            result.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task ToggleCompletionAsync_ShouldReturnNull_WhenInvalid()
        {
            _repoMock.Setup(x => x.GetByIdAsync(5)).ReturnsAsync((ChecklistItemModel)null);

            var result = await _service.ToggleCompletionAsync(5, 2);

            result.Should().BeNull();
        }


        [Fact]
        public void ShouldMapCorrectly()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChecklistItemModel, ChecklistItemDto>().ReverseMap();
            });

            // Act & Assert
            config.AssertConfigurationIsValid();
        }
    }
}
