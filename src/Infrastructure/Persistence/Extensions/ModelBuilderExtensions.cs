using Domain.Entities.Identity;
using Domain.Entities.Public;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Persistence.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<State>()
            .HasData(
                new State
                {
                    Id = 1,
                    Title = "st1",
                    Description = "desc1",
                    CreatedBy = "administrator@localhost",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Code = 1,
                    Order = 2
                });   
        }
    }
}
