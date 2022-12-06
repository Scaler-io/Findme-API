using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace API.DataAccess
{
    public class FindmeContext: DbContext
    {
        public FindmeContext(DbContextOptions<FindmeContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> Users { get; set; }
    }
}