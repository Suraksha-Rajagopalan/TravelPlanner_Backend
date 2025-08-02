using Microsoft.AspNetCore.Identity;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Implementation;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserModel> _userManager;

    public UnitOfWork(ApplicationDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;

        // Generic repositories
        var tripRepo = new GenericRepository<TripModel>(_context);
        var reviewRepo = new GenericRepository<ReviewModel>(_context);
        var checklistRepo = new GenericRepository<ChecklistItemModel>(_context);
        var itineraryRepo = new GenericRepository<ItineraryItemsModel>(_context);
        var expenseRepo = new GenericRepository<ExpenseModel>(_context);
        var tripShareRepo = new GenericRepository<TripShareModel>(_context);
        var userRepo = new GenericRepository<UserModel>(_context);


        // Assign to properties
        Trips = new TripRepository(_context, tripRepo);
        Reviews = new ReviewRepository(_context, reviewRepo);
        Checklists = new ChecklistRepository(_context, checklistRepo);
        Itineraries = new ItineraryRepository(_context, this, itineraryRepo); 
        Expenses = new ExpenseRepository(_context);
        TripShares = new TripShareRepository(_context, tripShareRepo);
        Access = new AccessRepository(_context);
        Auth = new AuthRepository(_context, _userManager);
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
