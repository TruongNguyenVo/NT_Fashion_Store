using Microsoft.EntityFrameworkCore;

namespace doan1_v1.Models
{
    public class NTFashionDbContext : DbContext
    {
        public NTFashionDbContext(DbContextOptions<NTFashionDbContext> options)
      : base(options)
        {
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProductDetail> OrderProductDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<PurchaseReport> PurchaseReports { get; set; }
        public DbSet<PurchaseReportProductDetail> PurchaseReportProductDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }

}
