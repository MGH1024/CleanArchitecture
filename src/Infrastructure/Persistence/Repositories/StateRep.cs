using Dapper;
using Domain.Contract.Models;
using Domain.Contract.Repositories;
using Domain.Entities.Public;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using SqlKata.Execution;
using System.Data;

namespace Persistence.Repositories
{
    public class StateRep : IStateRep
    {

        private readonly AppDbContext _appDbContext;

        public StateRep(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<IEnumerable<State>> GetAllAsync(GetParameter getParameter)
        {

            return await _appDbContext.States.ToListAsync();


        }

        public async Task<IEnumerable<State>> GetAllActiveAsync(GetParameter getParameter)
        {
            return await _appDbContext.States.Where(a => a.IsActive == true).ToListAsync();
        }

        public async Task<IEnumerable<State>> GetAllUpdatedAsync(GetParameter getParameter)
        {
            return await _appDbContext.States.Where(a => a.IsUpdated == true).ToListAsync();
        }

        public async Task<IEnumerable<State>> GetAllDeletedAsync(GetParameter getParameter)
        {
            return await _appDbContext.States.Where(a => a.IsDeleted == true).ToListAsync();
        }

        public async Task<State> GetByIdAsync(int stateId)
        {
            return await _appDbContext.States.Where(a => a.Id == stateId).FirstOrDefaultAsync();
        }

        public async Task CreateStateAsync(State state)
        {
            await _appDbContext.States.AddAsync(state);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateStateAsync(State state)
        {
            _appDbContext.States.Update(state);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteStateAsync(State state)
        {
            _appDbContext.States.Remove(state);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
