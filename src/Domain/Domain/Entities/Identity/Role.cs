using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class Role : IdentityRole<int>
{
    public string Description { get; set; }

    //navigations
    public ICollection<RolePermission> RolePermissions { get; set; }
}