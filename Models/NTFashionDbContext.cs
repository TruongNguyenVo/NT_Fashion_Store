using doan1_v1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace doan1_v1.Models
{
	public class NTFashionDbContext : IdentityDbContext<User>
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

		//ham de tranh loi xoa long trong category cha - con
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Category>()
		   .HasOne(c => c.ParentCategory)
		   .WithMany(c => c.SubCategories)
		   .HasForeignKey(c => c.ParentId)
		   .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.SetNull

			// Đảm bảo kiểu dữ liệu tương thích với SQLite
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				foreach (var property in entityType.GetProperties())
				{
					if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
					{
						property.SetMaxLength(256); // Đặt độ dài tối đa cho string
					}
				}
			}
		}
	}


}





