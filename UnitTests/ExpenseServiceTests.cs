using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Implementations;
using TravelPlannerAPI.UoW;
using Xunit;
using TravelPlannerAPI.Generic;

namespace UnitTests
{
    public class ExpenseServiceTests
    {
        private readonly Mock<IExpenseRepository> _expenseRepoMock;
        private readonly Mock<IGenericRepository<ExpenseModel>> _genericRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ExpenseService _service;

        public ExpenseServiceTests()
        {
            _expenseRepoMock = new Mock<IExpenseRepository>();
            _genericRepoMock = new Mock<IGenericRepository<ExpenseModel>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _service = new ExpenseService(
                _expenseRepoMock.Object,
                _genericRepoMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task AddExpenseAsync_ShouldAddExpense_WhenValid()
        {
            var expense = new ExpenseModel { Description = "Hotel", Amount = 1000 };

            var result = await _service.AddExpenseAsync(1, expense, 2);

            result.Should().NotBeNull();
            result.Description.Should().Be("Hotel");
            result.TripId.Should().Be(1);

            _genericRepoMock.Verify(r => r.AddAsync(expense), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddExpenseAsync_ShouldReturnNull_WhenNullDto()
        {
            var result = await _service.AddExpenseAsync(1, null, 2);

            result.Should().BeNull();
            _genericRepoMock.Verify(r => r.AddAsync(It.IsAny<ExpenseModel>()), Times.Never);
        }

        [Fact]
        public async Task GetExpensesAsync_ShouldReturnExpenses()
        {
            var expenses = new List<ExpenseModel>
            {
                new ExpenseModel { Id = 1, TripId = 1, Amount = 500 },
                new ExpenseModel { Id = 2, TripId = 1, Amount = 300 }
            };

            _expenseRepoMock.Setup(r => r.GetByTripAsync(1)).ReturnsAsync(expenses);

            var result = await _service.GetExpensesAsync(1, 2);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateExpenseAsync_ShouldUpdate_WhenValid()
        {
            var existing = new ExpenseModel { Id = 1, TripId = 1, Description = "Old", Amount = 100 };
            var update = new ExpenseModel { Description = "New", Amount = 200, Category = "Food" };

            _genericRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.UpdateExpenseAsync(1, 1, update, 2);

            result.Should().NotBeNull();
            result.Description.Should().Be("New");
            result.Amount.Should().Be(200);
            result.Category.Should().Be("Food");

            _genericRepoMock.Verify(r => r.Update(existing), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateExpenseAsync_ShouldReturnNull_WhenNotFound()
        {
            _genericRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ExpenseModel)null);

            var result = await _service.UpdateExpenseAsync(1, 99, new ExpenseModel(), 2);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateExpenseAsync_ShouldReturnNull_WhenTripMismatch()
        {
            var existing = new ExpenseModel { Id = 1, TripId = 99 };
            _genericRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.UpdateExpenseAsync(1, 1, new ExpenseModel(), 2);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteExpenseAsync_ShouldDelete_WhenValid()
        {
            var existing = new ExpenseModel { Id = 1, TripId = 1 };

            _genericRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.DeleteExpenseAsync(1, 1, 2);

            result.Should().BeTrue();

            _genericRepoMock.Verify(r => r.Delete(existing), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteExpenseAsync_ShouldReturnFalse_WhenNotFound()
        {
            _genericRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ExpenseModel)null);

            var result = await _service.DeleteExpenseAsync(1, 99, 2);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteExpenseAsync_ShouldReturnFalse_WhenTripMismatch()
        {
            var existing = new ExpenseModel { Id = 1, TripId = 5 };

            _genericRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _service.DeleteExpenseAsync(1, 1, 2);

            result.Should().BeFalse();
        }
    }
}
