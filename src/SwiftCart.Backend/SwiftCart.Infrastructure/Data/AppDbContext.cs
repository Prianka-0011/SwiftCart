
using Microsoft.EntityFrameworkCore;
using SwiftCart.Domain.Entities;


namespace EmotiaMart.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        public Guid currentUserId { get; set; } = Guid.Empty;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.AddInterceptors(new AuditInterceptor(currentUserId));
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>(); // Stores "Admin" in DB, not integer 1

                        modelBuilder.Entity<Order>(eb =>
                        {
                                eb.HasOne(o => o.ShippingAddress)
                                    .WithMany()
                                    .HasForeignKey(o => o.ShippingAddressId)
                                    .OnDelete(DeleteBehavior.Restrict);

                                eb.HasOne(o => o.User)
                                    .WithMany()
                                    .HasForeignKey(o => o.UserId)
                                    .OnDelete(DeleteBehavior.Restrict);

                                 
                        });

                        modelBuilder.Entity<OrderItem>(ib =>
                        {
                                ib.Property(i => i.UnitPrice).HasPrecision(18, 2);
                        });

        }


        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<ProductReview> ProductReviews { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;


    }
}
