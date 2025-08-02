using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Implementations;
using TravelPlannerAPI.UoW;
using Xunit;

namespace UnitTests
{
    public class ItineraryServiceTests
    {
        private readonly Mock<IItineraryRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly ItineraryService _service;

        public ItineraryServiceTests()
        {
            _repoMock = new Mock<IItineraryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItineraryItemCreateDto, ItineraryItemsModel>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _service = new ItineraryService(_repoMock.Object, _mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetItineraryItemsByTripIdAsync_ShouldReturnItems()
        {
            var items = new List<ItineraryItemsModel>
            {
                new ItineraryItemsModel { Id = 1, TripId = 1, Title = "Visit Museum" },
                new ItineraryItemsModel { Id = 2, TripId = 1, Title = "Lunch at Cafe" }
            };

            _repoMock.Setup(x => x.GetByTripIdAsync(1)).ReturnsAsync(items);

            var result = await _service.GetItineraryItemsByTripIdAsync(1);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetItineraryItemByIdAsync_ShouldReturnItem()
        {
            var item = new ItineraryItemsModel { Id = 1, TripId = 1, Title = "Visit Museum" };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(item);

            var result = await _service.GetItineraryItemByIdAsync(1);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task AddItineraryItemAsync_ShouldAddItem()
        {
            var dto = new ItineraryItemCreateDto { Title = "New Activity" };

            _repoMock.Setup(x => x.AddAsync(It.IsAny<ItineraryItemsModel>())).Returns(Task.CompletedTask);

            var result = await _service.AddItineraryItemAsync(1, dto);

            result.Should().NotBeNull();
            result.TripId.Should().Be(1);
            result.Title.Should().Be("New Activity");
        }

        [Fact]
        public async Task UpdateItineraryItemAsync_ShouldUpdate_WhenExists()
        {
            var dto = new ItineraryItemCreateDto { Title = "Updated Activity" };
            var item = new ItineraryItemsModel { Id = 1, TripId = 1, Title = "Old Activity" };

            _repoMock.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(item);
            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<ItineraryItemsModel>())).Returns(Task.CompletedTask);

            var result = await _service.UpdateItineraryItemAsync(1, dto);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateItineraryItemAsync_ShouldReturnFalse_WhenNotExists()
        {
            _repoMock.Setup(x => x.ExistsAsync(1)).ReturnsAsync(false);

            var result = await _service.UpdateItineraryItemAsync(1, new ItineraryItemCreateDto());

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteItineraryItemAsync_ShouldDelete_WhenExists()
        {
            var item = new ItineraryItemsModel { Id = 1, TripId = 1, Title = "Activity" };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(item);
            _repoMock.Setup(x => x.DeleteAsync(item)).Returns(Task.CompletedTask);

            var result = await _service.DeleteItineraryItemAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteItineraryItemAsync_ShouldReturnFalse_WhenNotExists()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((ItineraryItemsModel)null);

            var result = await _service.DeleteItineraryItemAsync(1);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetSharedItineraryAsync_ShouldReturnItems()
        {
            var items = new List<ItineraryItemsModel>
            {
                new ItineraryItemsModel { Id = 1, TripId = 1, Title = "Shared Activity" }
            };

            _repoMock.Setup(x => x.GetSharedItineraryAsync(1, 2)).ReturnsAsync(items);

            var result = await _service.GetSharedItineraryAsync(1, 2);

            result.Should().NotBeNull();
            result.Should().ContainSingle();
        }

        [Fact]
        public void ShouldMapCorrectly()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItineraryItemsModel, ItineraryItemCreateDto>().ReverseMap();
            });

            // Act & Assert
            config.AssertConfigurationIsValid();
        }
    }
}
