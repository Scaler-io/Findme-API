using API.Entities;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace API.DataAccess
{
    public class FindmeContextSeed
    {
        public static async Task SeedAsync(IServiceProvider services, ILogger logger, RoleManager<AppRole> roleManager)
        {
            var dataContext = services.GetRequiredService<FindmeContext>();

            logger.Here().Debug("data seeding started");
            try
            {
                //if(! await dataContext.Users.AnyAsync())
                //{
                //    logger.Here().Debug($"data context ready");
                //    var users = FileReadHelper<AppUser>.SeederFileReader("users");
                //    dataContext.Users.AddRange(users);
                //    await dataContext.SaveChangesAsync();
                //    logger.Here().Debug("data seeding complete");
                //}

                if(!await dataContext.Roles.AnyAsync())
                {
                    var roles = new List<AppRole>
                    {
                        new AppRole{Name = "Member"},
                        new AppRole {Name = "Admin"},
                        new AppRole{Name = "Moderator"}
                    };

                    foreach(var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }

                    logger.Here().Information("data seeding completed");
                }
            }catch(Exception e)
            {
                logger.Here().Error("data seeding failed");
            }
        }
    }
}
