using AutoMapper;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Mappings
{
    public class TripProfile : Profile
    {
        public TripProfile()
        {
            CreateMap<Trip, TripDto>().ReverseMap();
            CreateMap<TripCreateDto, Trip>();
            CreateMap<TripUpdateDto, Trip>()
    .ForMember(dest => dest.Reviews, opt => opt.Ignore())
    .ForMember(dest => dest.SharedUsers, opt => opt.Ignore());
            CreateMap<Trip, TripCreateDto>();
            CreateMap<Trip, TripUpdateDto>();
            CreateMap<BudgetDetails, BudgetDetailsDto>().ReverseMap();
            CreateMap<ChecklistItem, ChecklistItemDto>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Text));
            CreateMap<ChecklistItemDto, ChecklistItem>()
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.UserId, o => o.Ignore()); // set in service
            CreateMap<ChecklistItemUpdateDto, ChecklistItem>()
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.UserId, o => o.Ignore());
        }
    }
}
