
using Microsoft.EntityFrameworkCore;
using SwiftCart.Domain.Entities;


namespace EmotiaMart.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public Guid currentUserId { get; set; } = Guid.Empty;
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.AddInterceptors(new AuditInterceptor(currentUserId));
            base.OnConfiguring(optionsBuilder);
        }
         public DbSet<Test> Tests { get; set; } = null!;
        // public DbSet<User> Users { get; set; } = null!;
        // public DbSet<Address> Addresses { get; set; } = null!;

    }
}
