using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess
{
    public class FindmeContext: DbContext
    {
        public FindmeContext(DbContextOptions<FindmeContext> options)
            :base(options)
        {
            
        }

        public DbSet<AppUser> Users { get; set; }
    }
}