using Application.Services.Exception;
using AutoMapper;
using Contract.Services.DatetimeProvider;
using Contract.Services.Public;
using Contract.Services.Public.DTOs.State;
using Contract.Validator;
using Domain.Contract.Models;
using Domain.Contract.Repositories;
using Domain.Entities.Public;
using Utility;

namespace Application.Services.Public;

public class StateService : IStateService
{
    private readonly IStateRep _stateRepository;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public StateService(IStateRep stateRepository, IMapper mapper, IDateTime dateTime)
    {
        _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository)); 
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); 
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));  
    }

    public async Task<IEnumerable<StateDetail>> GetStatesAsync(GetParameter resourceParameter)
    {
        var result = await _stateRepository.GetAllAsync(resourceParameter);
        return _mapper.Map<IEnumerable<StateDetail>>(result);
           
    }

    public async Task<IEnumerable<StateDetail>> GetActiveStatesAsync(GetParameter resourceParameter)
    {
        var result = await _stateRepository.GetAllActiveAsync(resourceParameter);
        return _mapper.Map<IEnumerable<StateDetail>>(result);
    }

    public async Task<IEnumerable<StateDetail>> GetUpdatedStatesAsync(GetParameter resourceParameter)
    {
        var result = await _stateRepository.GetAllUpdatedAsync(resourceParameter);
        return  _mapper.Map<IEnumerable<StateDetail>>(result);
    }

    public async Task<IEnumerable<StateDetail>> GetDeletedStatesAsync(GetParameter resourceParameter)
    {
        var result = await _stateRepository.GetAllDeletedAsync(resourceParameter);
        return  _mapper.Map<IEnumerable<StateDetail>>(result);
          
    }

    public async Task<StateDetail> GetStateAsync(GetStateById getStateById)
    {
        var result = await _stateRepository.GetByIdAsync(getStateById.Id); 
        return  _mapper.Map<StateDetail>(result);
    }

    private async Task<State> GetStateEntityAsync(GetStateById getStateById)
    {
        return await _stateRepository.GetByIdAsync(getStateById.Id);
       
    }

    public async Task CreateStateAsync(CreateState createState)
    {

        //mapping
        var state = _mapper.Map<State>(createState);


        //2do-refactoring
        state.CreatedBy ="system";
        state.CreatedDate = _dateTime.IranNow;
        state.IsActive = true;
        state.IsUpdated = false;
        state.IsDeleted = false;

        
        await _stateRepository.CreateStateAsync(state);
    }

    public async Task UpdateStateAsync(UpdateState updateState)
    {
        var state = await GetStateEntityAsync(_mapper.Map<GetStateById>(updateState));

        //2do refactoring
        state.Description = updateState.Description;
        state.Code = updateState.Code;
        state.Title = updateState.Title;
        state.Order = updateState.Order;
        state.UpdatedBy = "system";
        state.UpdatedDate = _dateTime.IranNow;
        state.IsUpdated = true;
        //
        await _stateRepository.UpdateStateAsync(state);
    }

    public async Task DeleteStateAsync(DeleteState deleteState)
    {
        var state = _mapper.Map<State>(deleteState);

        //2do-refactoring
        state.DeletedBy = "System";
        state.DeletedDate = _dateTime.IranNow;
        state.IsDeleted = true;
        state.IsActive = false;
        //

        await _stateRepository.DeleteStateAsync(state);
    }

    public async Task<bool> IsStateRegistered(string title)
    {
        return await _stateRepository.GetStateByTitle(title) != null;
    }
}
