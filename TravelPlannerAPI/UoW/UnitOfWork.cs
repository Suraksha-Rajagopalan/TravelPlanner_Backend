using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Implementation;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        // Generic repositories
        var tripRepo = new GenericRepository<Trip>(_context);
        var reviewRepo = new GenericRepository<Review>(_context);
        var checklistRepo = new GenericRepository<ChecklistItem>(_context);
        var itineraryRepo = new GenericRepository<ItineraryItem>(_context);
        var expenseRepo = new GenericRepository<Expense>(_context);
        var tripShareRepo = new GenericRepository<TripShare>(_context);
        var userRepo = new GenericRepository<User>(_context);

        // Assign to properties
        Trips = new TripRepository(_context, tripRepo);
        Reviews = new ReviewRepository(_context, reviewRepo);
        Checklists = new ChecklistRepository(_context, checklistRepo);
        Itineraries = new ItineraryRepository(_context, itineraryRepo, this);
        Expenses = new ExpenseRepository(_context, expenseRepo);
        TripShares = new TripShareRepository(_context, tripShareRepo);
        Access = new AccessRepository(_context);
        Auth = new AuthRepository(userManager: null, userRepo);
        Users = new UserRepository(_context); 
    }

    // Properties
    public IUserRepository Users { get; }
    public ITripRepository Trips { get; }
    public IReviewRepository Reviews { get; }
    public IChecklistRepository Checklists { get; }
    public IItineraryRepository Itineraries { get; }
    public IExpenseRepository Expenses { get; }
    public ITripShareRepository TripShares { get; }
    public IAccessRepository Access { get; }
    public IAuthRepository Auth { get; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
}
