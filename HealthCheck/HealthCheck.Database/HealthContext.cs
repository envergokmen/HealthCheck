using HealthCheck.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCheck.Database
{
    public class HealthContext : DbContext
    {
        public HealthContext()
        {

        }

        public HealthContext(DbContextOptions<HealthContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //builder.UseSqlServer(@"Server=DESKTOP-UP8JB10;Database=HealthCheckDB;user id=sa; password=123456;MultipleActiveResultSets=true");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TargetApp> TargetApps { get; set; }
    
    }

}
