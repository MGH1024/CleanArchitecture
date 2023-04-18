using Domain.Contract.Models;
using Domain.Entities.Public;

namespace Domain.Contract.Repositories
{
    public interface IStateRep
    {
        Task<IEnumerable<State>> GetAllAsync(GetParameter getParameter);
        Task<IEnumerable<State>> GetAllActiveAsync(GetParameter getParameter);
        Task<IEnumerable<State>> GetAllUpdatedAsync(GetParameter getParameter);
        Task<IEnumerable<State>> GetAllDeletedAsync(GetParameter getParameter);
        Task<State> GetByIdAsync(int stateId);
        Task CreateStateAsync(State state);
        Task UpdateStateAsync(State state);
        Task DeleteStateAsync(State state);
        Task<State> GetStateByTitle(string title);
    }
}
