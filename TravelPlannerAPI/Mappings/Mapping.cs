using AutoMapper;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Mappings
{
    public class TripProfile : Profile
    {
        public TripProfile()
        {
            CreateMap<TripModel, TripDto>().ReverseMap();
            CreateMap<TripCreateDto, TripModel>();
            CreateMap<TripUpdateDto, TripModel>()
    .ForMember(dest => dest.Reviews, opt => opt.Ignore())
    .ForMember(dest => dest.SharedUsers, opt => opt.Ignore());
            CreateMap<TripModel, TripCreateDto>();
            CreateMap<TripModel, TripUpdateDto>();
            CreateMap<BudgetDetailsModel, BudgetDetailsDto>().ReverseMap();
            CreateMap<ChecklistItemModel, ChecklistItemDto>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
            CreateMap<ChecklistItemDto, ChecklistItemModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.UserId, o => o.Ignore()); // set in service
            CreateMap<ChecklistItemUpdateDto, ChecklistItemModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.UserId, o => o.Ignore());

            CreateMap<ItineraryItemCreateDto, ItineraryItemsModel>();
        }
    }
}
