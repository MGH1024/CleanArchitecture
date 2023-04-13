using Domain.Entities.Identity;

namespace Contract.Services.IdentityProvider;

public interface IPermissionService
{
    List<Permission> GetAllPermission();
}