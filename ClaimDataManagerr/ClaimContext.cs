using Microsoft.EntityFrameworkCore;

namespace ClaimDataManager
{
    public class ClaimContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=claims.db");
        }
    }
}