using doan1_v1.Models;
using Microsoft.AspNetCore.Identity;
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

			// Seed the role
			modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole
				{
					Id = "585545b1-9ba6-49e6-9638-b7c0d11d04a0",
					Name = "Manager",
					NormalizedName = "MANAGER"
				},
				new IdentityRole
				{
					Id = "f3b3b3b1-9ba6-49e6-9638-b7c0d11d04a0",
					Name = "Customer",
					NormalizedName = "CUSTOMER"
				}
			);
			var passwordHasher = new PasswordHasher<User>();
			modelBuilder.Entity<Manager>().HasData(
				new Manager
				{
					Id = "feef8538-ecae-4502-8fb2-7b3d33068776",
					FullName = "Võ Trường Nguyên",
					Address = "Bình Tân, Vĩnh Long",
					IsDel = false,
					DateOfBrith = new DateOnly(2024, 11, 23),
					Gender = "Nam",
					UserName = "admin",
					NormalizedUserName = "ADMIN",
					Email = "admin@gmail.com",
					PasswordHash = passwordHasher.HashPassword(null, "Admin@1234"),
					LockoutEnabled = true,
					PhoneNumber = "0987654321"

				},
				new Manager
				{
					Id = "feef8538-ecae-4502-8fb2-7b3d33068777",
					FullName = "Võ Trường Nguyên",
					Address = "Bình Tân, Vĩnh Long",
					IsDel = false,
					DateOfBrith = new DateOnly(2002, 11, 23),
					Gender = "Nam",
					UserName = "admin1",
					NormalizedUserName = "ADMIN1",
					Email = "admin@gmail1.com",
					PasswordHash = passwordHasher.HashPassword(null, "Admin@1234"),
					PhoneNumber = "0987654321",
					LockoutEnabled = true,
				}
			);

			// Seed the user role relationship
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>
				{
					UserId = "feef8538-ecae-4502-8fb2-7b3d33068776",  // Manager User ID
					RoleId = "585545b1-9ba6-49e6-9638-b7c0d11d04a0"   // Manager Role ID
				},
				new IdentityUserRole<string>
				{
					UserId = "feef8538-ecae-4502-8fb2-7b3d33068777",  // Manager User ID
					RoleId = "585545b1-9ba6-49e6-9638-b7c0d11d04a0"   // Manager Role ID
				}
			);


			base.OnModelCreating(modelBuilder);
		}


	}
}





