using Contract.Services.Public.DTOs.State;
using Domain.Contract.Models;
using Domain.Entities.Public;

namespace Contract.Services.Public;

public interface IStateService
{
    Task<IEnumerable<StateDetail>> GetStatesAsync(GetParameter resourceParameter);
    Task<IEnumerable<StateDetail>> GetActiveStatesAsync(GetParameter resourceParameter);
    Task<IEnumerable<StateDetail>> GetUpdatedStatesAsync(GetParameter resourceParameter);
    Task<IEnumerable<StateDetail>> GetDeletedStatesAsync(GetParameter resourceParameter);
    Task<StateDetail> GetStateAsync(GetStateById getStateById);
    Task CreateStateAsync(CreateState createState);
    Task UpdateStateAsync(UpdateState updateState);
    Task DeleteStateAsync(DeleteState deleteState);
    Task<bool> IsStateRegistered(string title);
}