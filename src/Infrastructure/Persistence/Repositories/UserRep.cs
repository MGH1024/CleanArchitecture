using Domain.Entities.Public;
using Dapper;
using System.Data;
using Domain.Contract.Repositories;
using Domain.Entities.Identity;
using Domain.Contract.Models;
using Persistence.Data;
using Contract.Services.IdentityProvider.DTOs.User;

namespace Persistence.Repositories
{
    public class UserRep : IUserRep
    {
        private readonly IDbConnection _dbConnection;
        private readonly AppDbContext _appDbContext;

        public UserRep(IDbConnection dbConnection, AppDbContext appDbContext)
        {
            _dbConnection = dbConnection;
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsers(GetParameter getParameter)
        {
            var result = await _dbConnection.QueryAsync<User>(
                sql: "User_GetAll",
                param: new
                {
                    getParameter.Search,
                    getParameter.PageNumber,
                    getParameter.PageSize,
                    getParameter.OrderBy,
                    getParameter.OrderType
                },
                 commandTimeout: 300,
               transaction: null,
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<User> GetByIdAsync(int stateId)
        {
            var result = await _dbConnection.QueryAsync<User>(
                sql: "dbo.User_GetById",
                param:
                new
                {
                    StateId = stateId
                },
                commandTimeout: 300,
                transaction: null,
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        public UserRefreshToken GetUserRefreshTokenByUserAndOldToken(User user, string token, string refreshToken)
        {
            var result = _appDbContext.UserRefreshToken.Select(a => a)
                .Where(a => a.UserId == user.Id && !a.IsInvalidated && a.Token == token
                && a.RefreshToken == refreshToken)
                .OrderByDescending(a => a.Id)
                .FirstOrDefault();
            return result;
        }

        public async Task InvalidateRefreshToken(string refreshToken)
        {
            var userRefreshToken = _appDbContext.UserRefreshToken
                .Select(a => a)
                .Where(a => a.RefreshToken == refreshToken)
                .First();

            userRefreshToken.IsInvalidated = true;
            _appDbContext.UserRefreshToken.Update(userRefreshToken);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task InsertUserRefreshToken(UserRefreshToken userRefreshToken)
        {
            _appDbContext.UserRefreshToken.Add(userRefreshToken);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
