using System.Threading.Tasks;
using TravelPlannerAPI.Repository.Interface;

namespace TravelPlannerAPI.UoW
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ITripRepository Trips { get; }
        IReviewRepository Reviews { get; }
        IChecklistRepository Checklists { get; }
        IItineraryRepository Itineraries { get; }
        IExpenseRepository Expenses { get; }
        ITripShareRepository TripShares { get; }
        IAccessRepository Access { get; }
        IAuthRepository Auth { get; }

        Task<int> CompleteAsync();
    }
}
