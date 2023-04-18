using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Base
{
    public interface ISeedService
    {
        Task SeedIdentityItemsAsync();
    }
    public class SeedService : ISeedService
    {
        public SeedService()
        {
            
        }

        public async Task SeedIdentityItemsAsync()
        {
            //Role administratorRole = new()
            //{
            //    Name = "Administrator",
            //    Description = "مدیر سیستم"
            //};

            //Role userRole = new()
            //{
            //    Name = "User",
            //    Description = "کاربر"
            //};

            //if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            //{
            //    await _roleManager.CreateAsync(administratorRole);
            //}

            //if (_roleManager.Roles.All(r => r.Name != userRole.Name))
            //{
            //    await _roleManager.CreateAsync(userRole);
            //}

            //User administrator = new()
            //{
            //    UserName = "admin",
            //    Email = "admin@admin.com",
            //    Firstname = "admin",
            //    Lastname = "admin",
            //    IsActive = true,
            //    CreatedBy = "System",
            //    CreatedDate = DateTime.UtcNow,
            //    Address = "address",
            //    BirthDate = new DateTime(1988, 09, 10),
            //    CellNumber = "09187108429",
            //    Image = "Image",
            //    PhoneNumber = "77245845",
            //};

            //if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            //{
            //    await _userManager.CreateAsync(administrator, "Abcde@12345");
            //    await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            //    var claims = new List<Claim>
            //{
            //    new Claim("username",administrator.UserName),
            //    new Claim(ClaimTypes.Email, administrator.Email),
            //    new Claim(ClaimTypes.GivenName, administrator.Firstname),
            //    new Claim(ClaimTypes.Surname, administrator.Lastname),
            //};
            //    await _userManager.AddClaimsAsync(administrator, claims);
            //}
        }
    }
}
