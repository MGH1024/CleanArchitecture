using AutoMapper;
using Contract.Services.DatetimeProvider;
using Contract.Services.Public.DTOs.State;
using Domain.Entities.Public;

namespace SecondApi.Mapper;

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
    }
}

