using AutoMapper;
using CRM.Application.DTOs.Auth;
using CRM.Application.DTOs.Customers;
using CRM.Application.DTOs.Deals;
using CRM.Application.DTOs.Exceptions;
using CRM.Application.DTOs.Interactions;
using CRM.Application.DTOs.Users;
using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;
using DomainException = CRM.Domain.Entities.Exception;

namespace CRM.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Value));
        
        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => UserRole.FromString(src.Role)))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ReportedExceptions, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedExceptions, opt => opt.Ignore())
            .ForMember(dest => dest.Customers, opt => opt.Ignore())
            .ForMember(dest => dest.Deals, opt => opt.Ignore())
            .ForMember(dest => dest.Interactions, opt => opt.Ignore());
        
        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<Customer, CustomerSummaryDto>();
        CreateMap<CreateCustomerRequest, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Deals, opt => opt.Ignore())
            .ForMember(dest => dest.Interactions, opt => opt.Ignore());
        
        // Exception mappings
        CreateMap<DomainException, ExceptionDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));
        
        CreateMap<DomainException, ExceptionSummaryDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));
        
        CreateMap<ExceptionHistory, ExceptionHistoryDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<AIRecommendation, RecommendationDto>();
        
        // Deal mappings
        CreateMap<Deal, DealDto>()
            .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => src.Stage.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));
        
        CreateMap<CreateDealRequest, Deal>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Stage, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        // Interaction mappings
        CreateMap<Interaction, InteractionDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        
        CreateMap<CreateInteractionRequest, Interaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<InteractionType>(src.Type)));
    }
}

