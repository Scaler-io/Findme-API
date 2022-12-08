using API.Entities;
using API.Extensions;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace API.DataAccess
{
    public class FindmeContextSeed
    {
        public static async Task SeedAsync(IServiceProvider services, ILogger logger)
        {
            var dataContext = services.GetRequiredService<FindmeContext>();

            logger.Here().Debug("data seeding started");
            try
            {
                if(! await dataContext.Users.AnyAsync())
                {
                    logger.Here().Debug($"data context ready");
                    var users = FileReadHelper<AppUser>.SeederFileReader("users");
                    dataContext.Users.AddRange(users);
                    await dataContext.SaveChangesAsync();
                    logger.Here().Debug("data seeding complete");
                }
            }catch(Exception e)
            {
                logger.Here().Error("data seeding failed");
            }
        }
    }
}
