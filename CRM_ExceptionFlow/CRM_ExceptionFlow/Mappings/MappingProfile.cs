using AutoMapper;
using CRM_ExceptionFlow.DTOs.Customers;
using CRM_ExceptionFlow.DTOs.Deals;
using CRM_ExceptionFlow.DTOs.Exceptions;
using CRM_ExceptionFlow.DTOs.Interactions;
using CRM_ExceptionFlow.DTOs.Users;
using CRM_ExceptionFlow.Models;
using ExceptionEntity = CRM_ExceptionFlow.Models.Exception;

namespace CRM_ExceptionFlow.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<Customer, CustomerSummaryDto>()
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToUser != null ? src.AssignedToUser.FullName : null));

            CreateMap<Customer, CustomerDetailDto>()
                .IncludeBase<Customer, CustomerSummaryDto>();

            CreateMap<Deal, DealDto>()
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToUser != null ? src.AssignedToUser.FullName : null));

            CreateMap<Interaction, InteractionDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null));

            CreateMap<ExceptionEntity, ExceptionSummaryDto>()
                .ForMember(dest => dest.ReportedBy, opt => opt.MapFrom(src => src.ReportedByUser != null ? src.ReportedByUser.FullName : null))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToUser != null ? src.AssignedToUser.FullName : null));

            CreateMap<ExceptionEntity, ExceptionDetailDto>()
                .IncludeBase<ExceptionEntity, ExceptionSummaryDto>();

            CreateMap<ExceptionHistory, ExceptionHistoryDto>();
            CreateMap<AIRecommendation, RecommendationDto>();
        }
    }
}

