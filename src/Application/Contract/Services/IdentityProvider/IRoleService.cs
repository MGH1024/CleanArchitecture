using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Identity;

namespace Contract.Services.IdentityProvider;

public interface IRoleService
{
    Task<IdentityResult> AddRoleToUser(User user, int roleId);
    Task<List<Role>> GetRoleListByUser(User user);
    Task<IdentityResult> RemoveRolesByUser(User user);
    Task<IdentityResult> AssignRolesToUser(User user, List<int> roleId);
    Task<Role> GetById(int roleId);
}