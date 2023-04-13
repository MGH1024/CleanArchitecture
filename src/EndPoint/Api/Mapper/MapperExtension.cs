using AutoMapper;
using Contract.Services.DatetimeProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Contract.Services.Public.DTOs.State;
using Domain.Entities.Identity;
using Domain.Entities.Public;

namespace Api.Mapper;

public class MapperExtension : Profile
{
    public MapperExtension()
    {
        CreateMap<State, StateDetail>();
        CreateMap<StateDetail, State>();
        CreateMap<CreateState, State>();
        CreateMap<UpdateState, GetStateById>();
        CreateMap<UpdateState, State>();
        CreateMap<DeleteState, GetStateById>();
        CreateMap<DeleteState, State>();

        //user
        CreateMap<CreateUserDto, User>();
        CreateMap<CreateUser, User>()
            .ForMember(dest => dest.UserName, a => a.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, a => a.MapFrom(src => src.Email))
            .ForMember(dest => dest.Firstname, a => a.MapFrom(src => src.Firstname))
            .ForMember(dest => dest.Lastname, a => a.MapFrom(src => src.Lastname))
            .ForMember(dest => dest.CreatedBy, a => a.MapFrom(src => "system"))
            .ForMember(dest => dest.CreatedBy, a => a.MapFrom(src => DateTime.Now));
    }
}