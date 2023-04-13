using Domain.Contract.Models;
using Domain.Entities.Identity;

namespace Domain.Contract.Repositories
{
    public interface IUserRep
    {
        Task<IEnumerable<User>> GetAllUsers(GetParameter getParameter);
        Task<User> GetByIdAsync(int stateId);
        Task InsertUserRefreshToken(UserRefreshToken userRefreshToken);
        UserRefreshToken GetUserRefreshTokenByUserAndOldToken(User user, string token, string refreshToken);
        Task InvalidateRefreshToken(string refreshToken);
    }
}
