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
			// Manager
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

				}
			);
			//Customer
			modelBuilder.Entity<Customer>().HasData(
				new Customer
				{
					Id = "feef8538-ecae-4502-8fb2-7b3d33068777",
					FullName = "Võ Trường Nguyên",
					Address = "Bình Tân, Vĩnh Long",
					IsDel = false,
					DateOfBrith = new DateOnly(2000, 11, 23),
					Gender = "Nam",
					UserName = "customer",
					NormalizedUserName = "CUSTOMER",
					Email = "customer@gmail.com",
					PasswordHash = passwordHasher.HashPassword(null, "Customer@1234"),
					LockoutEnabled = true,
					PhoneNumber = "0987654321"
				});
			// Seed the user role relationship
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>
				{
					UserId = "feef8538-ecae-4502-8fb2-7b3d33068776",  // Manager User ID
					RoleId = "585545b1-9ba6-49e6-9638-b7c0d11d04a0"   // Manager Role ID
				},
				new IdentityUserRole<string>
				{
					UserId = "feef8538-ecae-4502-8fb2-7b3d33068777",  // Customer User ID
					RoleId = "f3b3b3b1-9ba6-49e6-9638-b7c0d11d04a0"   // Customer Role ID
				}
			);

			//Seed Cart
			modelBuilder.Entity<Cart>().HasData(
				new Cart { Id = 1, UserId = "feef8538-ecae-4502-8fb2-7b3d33068777" }
			);
			// Seed Categories
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Áo", ParentId = null, Description = null, IsDel = false, UserId = null },
				new Category { Id = 2, Name = "Quần", ParentId = null, Description = null, IsDel = false, UserId = null },
				new Category { Id = 3, Name = "Áo Khoác Nam", ParentId = 1, Description = null, IsDel = false, UserId = null },
				new Category { Id = 4, Name = "Áo Thun Nam", ParentId = 1, Description = null, IsDel = false, UserId = null },
				new Category { Id = 5, Name = "Áo Polo Nam", ParentId = 1, Description = null, IsDel = false, UserId = null },
				new Category { Id = 7, Name = "Áo Sweater Nam", ParentId = 1, Description = null, IsDel = false, UserId = null },
				new Category { Id = 8, Name = "Quần Dài", ParentId = 2, Description = null, IsDel = false, UserId = null },
				new Category { Id = 9, Name = "Quần Jean", ParentId = 2, Description = null, IsDel = false, UserId = null },
				new Category { Id = 10, Name = "Quần Short Nam", ParentId = 2, Description = null, IsDel = false, UserId = null }
			);

			// Seed URL Product
			modelBuilder.Entity<ProductImage>().HasData(
				new ProductImage { Id = 1, ProductId = 1, ImgURL = @"\admin\assets\uploads\ae4c0f1e-d6a9-4cda-9403-931906f871bc.jpg" },
				new ProductImage { Id = 2, ProductId = 1, ImgURL = @"\admin\assets\uploads\925589bd-d7d4-49ce-a424-521bf697ffba.jpg" },
				new ProductImage { Id = 3, ProductId = 1, ImgURL = @"\admin\assets\uploads\4b5184cd-87a5-4151-a96f-9f80a99b1884.jpg" },
				new ProductImage { Id = 4, ProductId = 1, ImgURL = @"\admin\assets\uploads\a8435a63-d56a-4af7-b032-a7132f99d302.jpg" },
				new ProductImage { Id = 5, ProductId = 1, ImgURL = @"\admin\assets\uploads\474793b8-e467-4efb-8412-5bdd2c3c9f26.jpg" },
				new ProductImage { Id = 6, ProductId = 1, ImgURL = @"\admin\assets\uploads\4211d6de-7f25-4289-974d-2a2b37c431f7.jpg" },
				new ProductImage { Id = 7, ProductId = 1, ImgURL = @"\admin\assets\uploads\87b70ad3-9ae9-4ced-85d0-29146f014199.jpg" },
				new ProductImage { Id = 8, ProductId = 2, ImgURL = @"\admin\assets\uploads\6cd2efb0-9afa-4f64-8a22-6567b151b898.jpg" },
				new ProductImage { Id = 9, ProductId = 2, ImgURL = @"\admin\assets\uploads\c8152199-59f0-47a2-a3ad-c9a3b7ee1172.jpg" },
				new ProductImage { Id = 10, ProductId = 2, ImgURL = @"\admin\assets\uploads\f7323b52-aa31-4c5c-8808-dfa77ec2adcc.jpg" },
				new ProductImage { Id = 11, ProductId = 2, ImgURL = @"\admin\assets\uploads\9087eb53-e86d-445e-bdfa-e4d6154c858d.jpg" },
				new ProductImage { Id = 12, ProductId = 3, ImgURL = @"\admin\assets\uploads\00a6cea6-3cd7-4f18-873f-bd9078a0f184.jpg" },
				new ProductImage { Id = 13, ProductId = 3, ImgURL = @"\admin\assets\uploads\60425724-3fde-4071-afe1-159c19ef31d0.jpg" },
				new ProductImage { Id = 14, ProductId = 3, ImgURL = @"\admin\assets\uploads\ea550b3e-c8ce-4b20-96ea-61ce8d9bde2c.jpg" },
				new ProductImage { Id = 15, ProductId = 3, ImgURL = @"\admin\assets\uploads\fede380c-5c8f-4f84-91ce-ba1fc9f69379.jpg" },
				new ProductImage { Id = 16, ProductId = 4, ImgURL = @"\admin\assets\uploads\9c0e7f8b-d8d2-46fe-b5cf-dc9683d55c71.jpg" },
				new ProductImage { Id = 17, ProductId = 4, ImgURL = @"\admin\assets\uploads\afa0fe4a-79b5-488c-b25d-b718de133492.jpg" },
				new ProductImage { Id = 18, ProductId = 4, ImgURL = @"\admin\assets\uploads\f6a5f532-43ec-4d73-95a1-39860e46e30d.jpg" },
				new ProductImage { Id = 19, ProductId = 4, ImgURL = @"\admin\assets\uploads\6111ac5b-22e4-4ca1-ab16-758691f51edd.jpg" },
				new ProductImage { Id = 20, ProductId = 5, ImgURL = @"\admin\assets\uploads\c3bf2c94-4c5f-4aa3-996d-48e0e63c8013.jpg" },
				new ProductImage { Id = 21, ProductId = 5, ImgURL = @"\admin\assets\uploads\38c571f3-ba76-415d-8fba-7a6c7528e31a.jpg" },
				new ProductImage { Id = 22, ProductId = 5, ImgURL = @"\admin\assets\uploads\b8470332-c816-4369-920d-cc1a1b01aa55.jpg" },
				new ProductImage { Id = 23, ProductId = 5, ImgURL = @"\admin\assets\uploads\1c8eac88-ad98-411a-9566-6b5ab7c85fdf.jpg" },
				new ProductImage { Id = 24, ProductId = 6, ImgURL = @"\admin\assets\uploads\98660d05-18b1-4895-b41f-7f634244dced.jpg" },
				new ProductImage { Id = 25, ProductId = 6, ImgURL = @"\admin\assets\uploads\7b52139b-470f-46f8-803b-6cbd6a994102.jpg" },
				new ProductImage { Id = 26, ProductId = 6, ImgURL = @"\admin\assets\uploads\da78525f-34ae-49ae-910f-ac7dff57af76.jpg" },
				new ProductImage { Id = 27, ProductId = 6, ImgURL = @"\admin\assets\uploads\5debc7cd-1da7-4bac-a7cc-02af51cc9135.jpg" },
				new ProductImage { Id = 28, ProductId = 7, ImgURL = @"\admin\assets\uploads\68299cb2-3815-40f7-bb3d-9e15f7046a21.jpg" },
				new ProductImage { Id = 29, ProductId = 7, ImgURL = @"\admin\assets\uploads\fe3ff34c-24a7-4c6b-b906-cde908b1d04d.jpg" },
				new ProductImage { Id = 30, ProductId = 7, ImgURL = @"\admin\assets\uploads\081cc15c-2450-422a-abb4-1b851144a859.jpg" },
				new ProductImage { Id = 31, ProductId = 7, ImgURL = @"\admin\assets\uploads\f79eea94-20f6-4413-a342-65aac8ed52d2.jpg" },
				new ProductImage { Id = 32, ProductId = 8, ImgURL = @"\admin\assets\uploads\20669520-967a-4086-999d-6ad14e53ffca.jpg" },
				new ProductImage { Id = 33, ProductId = 8, ImgURL = @"\admin\assets\uploads\7194a942-e6d9-47c9-9172-e294f93010c1.jpg" },
				new ProductImage { Id = 34, ProductId = 8, ImgURL = @"\admin\assets\uploads\b0e69860-68b6-4ba5-ac2e-2f99493e8c4e.jpg" },
				new ProductImage { Id = 35, ProductId = 8, ImgURL = @"\admin\assets\uploads\7d917168-9c05-45da-b744-155c0888c847.jpg" },
				new ProductImage { Id = 36, ProductId = 9, ImgURL = @"\admin\assets\uploads\1397c047-dd5e-4f38-a380-015a087745a5.jpg" },
				new ProductImage { Id = 37, ProductId = 9, ImgURL = @"\admin\assets\uploads\3b640f8b-7f04-4fae-9bc0-20197b2084dd.jpg" },
				new ProductImage { Id = 38, ProductId = 9, ImgURL = @"\admin\assets\uploads\a010acb8-f721-4997-b39a-b9e2e78f8f68.jpg" },
				new ProductImage { Id = 39, ProductId = 9, ImgURL = @"\admin\assets\uploads\19e47a5f-6002-4af8-95d3-ecfec599fd6c.jpg" },
				new ProductImage { Id = 40, ProductId = 10, ImgURL = @"\admin\assets\uploads\dd69eea9-44d1-4bd9-a250-2d722a85ca01.jpg" },
				new ProductImage { Id = 41, ProductId = 10, ImgURL = @"\admin\assets\uploads\ecfcfec2-5601-4b6e-a379-968c39f1bf10.jpg" },
				new ProductImage { Id = 42, ProductId = 10, ImgURL = @"\admin\assets\uploads\8c7b0064-cd34-4b14-a4e3-c8e2b3f0a605.jpg" },
				new ProductImage { Id = 43, ProductId = 10, ImgURL = @"\admin\assets\uploads\7d5fe1f7-bacd-48f3-80a8-00a414c4a0c9.jpg" },
				new ProductImage { Id = 44, ProductId = 11, ImgURL = @"\admin\assets\uploads\a4c97f5b-dfbf-452e-bd3a-eabf2ab1cd4b.jpg" },
				new ProductImage { Id = 45, ProductId = 11, ImgURL = @"\admin\assets\uploads\64884b05-7701-4a8a-88e7-87b44f53ba21.jpg" },
				new ProductImage { Id = 46, ProductId = 11, ImgURL = @"\admin\assets\uploads\4a9457d5-f608-45fc-80be-d4cd7fc82333.jpg" },
				new ProductImage { Id = 47, ProductId = 11, ImgURL = @"\admin\assets\uploads\d79eda2e-c69a-47a5-82db-1ce7b774453e.jpg" },
				new ProductImage { Id = 48, ProductId = 12, ImgURL = @"\admin\assets\uploads\fb4578e3-56c5-437d-b641-8228592805ca.jpg" },
				new ProductImage { Id = 49, ProductId = 12, ImgURL = @"\admin\assets\uploads\6fbb5223-682c-4978-8ed0-9190fc807fcc.jpg" },
				new ProductImage { Id = 50, ProductId = 12, ImgURL = @"\admin\assets\uploads\3f0a854a-2421-475d-9e55-af3702622cce.jpg" },
				new ProductImage { Id = 51, ProductId = 12, ImgURL = @"\admin\assets\uploads\118288e9-945a-4cf9-a852-17cbe0d97453.jpg" },
				new ProductImage { Id = 52, ProductId = 12, ImgURL = @"\admin\assets\uploads\3a78bbab-1c45-4f08-a4a8-a01c234adba8.jpg" },
				new ProductImage { Id = 53, ProductId = 13, ImgURL = @"\admin\assets\uploads\aa223e15-a88e-4512-8e15-10ed699ced20.jpg" },
				new ProductImage { Id = 54, ProductId = 13, ImgURL = @"\admin\assets\uploads\f751a357-8e21-420c-b5e9-ac455d628a40.jpg" },
				new ProductImage { Id = 55, ProductId = 13, ImgURL = @"\admin\assets\uploads\345733d5-d833-4d4a-8f25-d81b991748bf.jpg" },
				new ProductImage { Id = 56, ProductId = 13, ImgURL = @"\admin\assets\uploads\2ef95b1f-44da-49cd-ba48-8154fa8528b2.jpg" },
				new ProductImage { Id = 57, ProductId = 14, ImgURL = @"\admin\assets\uploads\32370d49-8300-48f4-9705-a579bdee845e.jpg" },
				new ProductImage { Id = 58, ProductId = 14, ImgURL = @"\admin\assets\uploads\11b3a44b-c191-4a84-9e4f-9bc7e3abb59f.jpg" },
				new ProductImage { Id = 59, ProductId = 14, ImgURL = @"\admin\assets\uploads\635f7f11-b88c-4af5-b66a-e52f8bc3eabe.jpg" },
				new ProductImage { Id = 60, ProductId = 14, ImgURL = @"\admin\assets\uploads\9855c1e4-b73a-4a86-8902-4d7d2d8f3191.jpg" },
				new ProductImage { Id = 61, ProductId = 15, ImgURL = @"\admin\assets\uploads\5e321123-78f4-4f06-b2ee-0bf096771a77.jpg" },
				new ProductImage { Id = 62, ProductId = 15, ImgURL = @"\admin\assets\uploads\a026b235-7be3-46d1-959c-ae8f42c3873e.jpg" },
				new ProductImage { Id = 63, ProductId = 15, ImgURL = @"\admin\assets\uploads\2e8e3a5e-a487-404c-9f58-0ffff5175a79.jpg" },
				new ProductImage { Id = 64, ProductId = 15, ImgURL = @"\admin\assets\uploads\18d99482-20ac-4d6d-9dcc-e97c6b432a40.jpg" },
				new ProductImage { Id = 65, ProductId = 16, ImgURL = @"\admin\assets\uploads\5aaf9cc4-3166-4113-992c-8bd1e0a05fbc.jpg" },
				new ProductImage { Id = 66, ProductId = 16, ImgURL = @"\admin\assets\uploads\080ec7fc-e3c7-4636-94b8-2dc5f2b687f4.jpg" },
				new ProductImage { Id = 67, ProductId = 16, ImgURL = @"\admin\assets\uploads\11b65b17-7cc0-496f-96cd-7d673c877a2b.jpg" },
				new ProductImage { Id = 68, ProductId = 17, ImgURL = @"\admin\assets\uploads\58a761c4-5649-49b3-a127-bb79e7218a2c.jpg" },
				new ProductImage { Id = 69, ProductId = 17, ImgURL = @"\admin\assets\uploads\4e3e44c1-b12d-4291-8825-a5a13304369b.jpg" },
				new ProductImage { Id = 70, ProductId = 17, ImgURL = @"\admin\assets\uploads\de56becb-e112-4b5c-97a5-a480084a6d47.jpg" },
				new ProductImage { Id = 71, ProductId = 18, ImgURL = @"\admin\assets\uploads\7ac919fd-9ae3-4cd2-8ec2-9cf553af69ee.jpg" },
				new ProductImage { Id = 72, ProductId = 18, ImgURL = @"\admin\assets\uploads\9da0b929-d624-46fb-a1b6-8ff3025cf031.jpg" },
				new ProductImage { Id = 73, ProductId = 18, ImgURL = @"\admin\assets\uploads\89994d77-ebf5-4cd7-9c20-b739760fca23.jpg" },
				new ProductImage { Id = 74, ProductId = 18, ImgURL = @"\admin\assets\uploads\cb459675-efd2-4ca4-a081-efa3f3dc6068.jpg" },
				new ProductImage { Id = 75, ProductId = 19, ImgURL = @"\admin\assets\uploads\f8731f06-e56a-4872-a829-150c2d7370cc.jpg" },
				new ProductImage { Id = 76, ProductId = 19, ImgURL = @"\admin\assets\uploads\8d73fe43-364e-413e-ac8f-819e804a11ce.jpg" },
			new ProductImage { Id = 77, ProductId = 19, ImgURL = @"\admin\assets\uploads\fc6bbc77-647f-478a-81cf-3b98aea6126e.jpg" },
			new ProductImage { Id = 78, ProductId = 19, ImgURL = @"\admin\assets\uploads\263f48d4-2c6c-45f0-8b58-d009d1923a15.jpg" },
			new ProductImage { Id = 79, ProductId = 20, ImgURL = @"\admin\assets\uploads\d4dd535e-0c24-48d8-ab88-a8e6a7375a4d.jpg" },
			new ProductImage { Id = 80, ProductId = 20, ImgURL = @"\admin\assets\uploads\99af3a6c-2973-458c-98f1-6845e2147348.jpg" },
			new ProductImage { Id = 81, ProductId = 20, ImgURL = @"\admin\assets\uploads\3b2d868b-0c17-468f-8d34-46080e35214c.jpg" },
			new ProductImage { Id = 82, ProductId = 20, ImgURL = @"\admin\assets\uploads\823c9262-46c6-480b-a4b1-cc1ea12c2563.jpg" },
			new ProductImage { Id = 83, ProductId = 21, ImgURL = @"\admin\assets\uploads\d4ba93a4-b933-4625-bca4-9ea848052876.jpg" },
			new ProductImage { Id = 84, ProductId = 21, ImgURL = @"\admin\assets\uploads\73997834-dd68-4cbb-bcfc-a8723ac8f737.jpg" },
			new ProductImage { Id = 85, ProductId = 21, ImgURL = @"\admin\assets\uploads\b644dea4-dee4-4297-b69c-81dd7ba510cd.jpg" },
			new ProductImage { Id = 86, ProductId = 21, ImgURL = @"\admin\assets\uploads\ef48e9c9-d63b-4d56-baf5-7935fb799e1f.jpg" },
			new ProductImage { Id = 87, ProductId = 22, ImgURL = @"\admin\assets\uploads\18d0e9be-a37b-4902-bbc9-e7e264c2db86.jpg" },
			new ProductImage { Id = 88, ProductId = 22, ImgURL = @"\admin\assets\uploads\e443f9ec-1dd4-40fc-a7b8-0db9b2f62a4a.jpg" },
			new ProductImage { Id = 89, ProductId = 22, ImgURL = @"\admin\assets\uploads\c207a3c3-be30-415a-abf1-1c2593cc811d.jpg" },
			new ProductImage { Id = 90, ProductId = 22, ImgURL = @"\admin\assets\uploads\6d0038dc-4e5e-4753-86db-d08469e26f02.jpg" },
			new ProductImage { Id = 91, ProductId = 23, ImgURL = @"\admin\assets\uploads\064421d5-7b95-4996-8cc9-f7514f0d1e8c.jpg" },
			new ProductImage { Id = 92, ProductId = 23, ImgURL = @"\admin\assets\uploads\f492a3c7-6b2b-4d00-b73e-b0e1c016ad29.jpg" },
			new ProductImage { Id = 93, ProductId = 23, ImgURL = @"\admin\assets\uploads\867d9579-70ac-4650-b5da-68bcf6c3284c.jpg" },
			new ProductImage { Id = 94, ProductId = 23, ImgURL = @"\admin\assets\uploads\19fef06c-4252-4748-955e-98e7f6cf23d4.jpg" },
			new ProductImage { Id = 95, ProductId = 24, ImgURL = @"\admin\assets\uploads\47774bf7-53a7-4cec-9ef3-8ec49ba604a0.jpg" },
			new ProductImage { Id = 96, ProductId = 24, ImgURL = @"\admin\assets\uploads\6356b2e1-1813-49b9-bfd1-bae8bafa00a5.jpg" },
			new ProductImage { Id = 97, ProductId = 24, ImgURL = @"\admin\assets\uploads\062b1fcc-4404-4f1c-a09d-cf0c6e16da78.jpg" },
			new ProductImage { Id = 98, ProductId = 24, ImgURL = @"\admin\assets\uploads\9eadee08-4caf-47f6-b4e7-21cc62b267ac.jpg" },
			new ProductImage { Id = 99, ProductId = 25, ImgURL = @"\admin\assets\uploads\52fca2b3-0c38-4f3b-a25b-929660e416a7.jpg" },
			new ProductImage { Id = 100, ProductId = 25, ImgURL = @"\admin\assets\uploads\5185b3c9-c2bc-4fdc-8684-181b3abfae0e.jpg" },
			new ProductImage { Id = 101, ProductId = 25, ImgURL = @"\admin\assets\uploads\25abf9ac-31f0-4f7f-acc0-66502be714f7.jpg" },
			new ProductImage { Id = 102, ProductId = 25, ImgURL = @"\admin\assets\uploads\6ec5ef93-b905-47e5-a20c-510559775ca0.jpg" },
			new ProductImage { Id = 103, ProductId = 26, ImgURL = @"\admin\assets\uploads\2f6fcb25-fc9a-47bc-a2a8-687cbf01a0d1.jpg" },
				new ProductImage { Id = 104, ProductId = 26, ImgURL = @"\admin\assets\uploads\d919e24f-7b89-41ff-a45a-e7ada874b51f.jpg" },
				new ProductImage { Id = 105, ProductId = 26, ImgURL = @"\admin\assets\uploads\b57e1503-3e04-41f4-9fd9-8040c0224195.jpg" },
				new ProductImage { Id = 106, ProductId = 26, ImgURL = @"\admin\assets\uploads\ab448de0-abe5-4ad7-bb4e-919ad6b7e5e3.jpg" },
				new ProductImage { Id = 107, ProductId = 27, ImgURL = @"\admin\assets\uploads\7bc7632f-a7b1-4a37-a691-36b560ac6e67.jpg" },
				new ProductImage { Id = 108, ProductId = 27, ImgURL = @"\admin\assets\uploads\d855d20b-9439-4c3c-922a-1001461e0b0b.jpg" },
				new ProductImage { Id = 109, ProductId = 27, ImgURL = @"\admin\assets\uploads\2664e822-64e1-4150-9929-7619e7f35d84.jpg" },
				new ProductImage { Id = 110, ProductId = 27, ImgURL = @"\admin\assets\uploads\8cb8267d-df4d-4bbc-a868-c0bb16182308.jpg" },
				new ProductImage { Id = 111, ProductId = 28, ImgURL = @"\admin\assets\uploads\4598d952-4df5-4062-ac61-b59c130a6772.jpg" },
				new ProductImage { Id = 112, ProductId = 28, ImgURL = @"\admin\assets\uploads\c4b0b883-38d3-4b08-8620-5de667e0046c.jpg" },
				new ProductImage { Id = 113, ProductId = 28, ImgURL = @"\admin\assets\uploads\2c34dd6d-b8b7-42a0-ada9-d8b3c0eecd2e.jpg" },
				new ProductImage { Id = 114, ProductId = 28, ImgURL = @"\admin\assets\uploads\ee450ea8-06ea-49c3-b4ea-0ddfe92cf3c7.jpg" },
				new ProductImage { Id = 115, ProductId = 29, ImgURL = @"\admin\assets\uploads\d506ad83-25a7-47d2-bffb-40106ea3339b.jpg" },
				new ProductImage { Id = 116, ProductId = 29, ImgURL = @"\admin\assets\uploads\78775601-5530-48da-a854-633fa8d36c1b.jpg" },
				new ProductImage { Id = 117, ProductId = 29, ImgURL = @"\admin\assets\uploads\6f7da74c-bbfa-4627-b740-17cde0ca932c.jpg" },
				new ProductImage { Id = 118, ProductId = 29, ImgURL = @"\admin\assets\uploads\a1e1d780-9a2b-440b-a50d-93ca39b1180e.jpg" },
				new ProductImage { Id = 119, ProductId = 30, ImgURL = @"\admin\assets\uploads\21a9563e-db56-473d-92d3-61f51847aed9.jpg" },
				new ProductImage { Id = 120, ProductId = 30, ImgURL = @"\admin\assets\uploads\fdc419e4-f376-4991-b1d8-e838f10c618b.jpg" },
				new ProductImage { Id = 121, ProductId = 30, ImgURL = @"\admin\assets\uploads\9b6574ce-9c5b-419b-8934-040db136368c.jpg" },
				new ProductImage { Id = 122, ProductId = 30, ImgURL = @"\admin\assets\uploads\46945a0d-af4d-4024-985f-55bec3a11645.jpg" },
				new ProductImage { Id = 123, ProductId = 30, ImgURL = @"\admin\assets\uploads\045cfbad-3610-460b-8568-25f65f050e71.jpg" },
				new ProductImage { Id = 124, ProductId = 38, ImgURL = @"\admin\assets\uploads\4fbd31ef-c1fe-46bc-9498-65f0836ed459.png" },
				new ProductImage { Id = 125, ProductId = 38, ImgURL = @"\admin\assets\uploads\bb64df9d-51c4-41bc-8104-b10a1c89b619.jpg" },
				new ProductImage { Id = 126, ProductId = 38, ImgURL = @"\admin\assets\uploads\3373fe8f-8299-4012-82b4-a730a8e37b01.jpg" },
				new ProductImage { Id = 127, ProductId = 38, ImgURL = @"\admin\assets\uploads\b581f567-a46e-418c-9e3f-ee7ddbba4e02.jpg" },
				new ProductImage { Id = 128, ProductId = 36, ImgURL = @"\admin\assets\uploads\b387e3da-4343-4e94-9005-0b60e2c30bc1.jpg" },
				new ProductImage { Id = 129, ProductId = 36, ImgURL = @"\admin\assets\uploads\aa7cc3d3-8f6e-4809-9557-9cdea8e87753.jpg" },
				new ProductImage { Id = 130, ProductId = 36, ImgURL = @"\admin\assets\uploads\a60e82d3-45ac-413c-8dda-cc27b6b51585.jpg" }
				);

			// Seed Product
			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Name = "Áo Polo Nam Bo Sọc Kiểu Hai Màu MPO 1043",
					Description = "Áo Polo Nam Bo Sọc Kiểu Hai Màu MPO 1043 mang đến phong cách thanh lịch với thiết kế bo sọc tinh tế. Sản phẩm có màu hồng nhạt nhẹ nhàng, phù hợp với mọi dịp từ công sở đến dạo phố. Chất liệu vải 230 GSM gồm 57% Cotton, 38% Polyester và 5% Spandex giúp áo mềm mại, co giãn tốt và bền màu theo thời gian. Phom dáng vừa vặn cùng đường may tỉ mỉ mang lại sự thoải mái và tự tin khi mặc. Sản xuất tại Việt Nam, sản phẩm đảm bảo chất lượng và sự phù hợp với khí hậu nóng ẩm.",
					Color = "HỒNG NHẠT",
					Dimension = "XL",
					Material = "57% Cotton + 38% Polyester + 5% Spandex – 230 GSM.",
					Quantity = 100,
					Price = 429000,
					Productor = "Việt Nam",
					IsDel = true,
					CategoryId = 5
				},
				new Product
				{
					Id = 2,
					Name = "Áo Polo Nam Iscra MPO 1044",
					Description = "Áo Polo Nam Iscra MPO 1044 sở hữu gam màu trắng tinh tế, dễ dàng kết hợp với nhiều trang phục khác nhau. Chất liệu Pique Iscra với 70% Cotton và 30% Iscra mang lại sự mềm mại, thoáng khí và khả năng giữ form vượt trội. Thiết kế cổ điển kết hợp phom dáng hiện đại giúp tôn lên vẻ ngoài thanh lịch và năng động. Đường may tỉ mỉ cùng chất liệu cao cấp đảm bảo sự thoải mái và bền bỉ khi sử dụng. Sản xuất tại Việt Nam, sản phẩm đáp ứng tiêu chuẩn chất lượng cao và phù hợp với khí hậu Việt Nam.",
					Color = "TRẮNG",
					Dimension = "XL",
					Material = "Pique Iscra – 70% Cotton + 30% Iscra.",
					Quantity = 98,
					Price = 449000,
					Productor = "Việt Nam",
					IsDel = false,
					CategoryId = 5
				},
				new Product
				{
					Id = 3,
					Name = "Áo Polo Nam Regular Body MPO 1350",
					Description = "Áo Polo Nam Regular Body MPO 1350 mang sắc màu be trang nhã, tạo nên phong cách thanh lịch và dễ phối đồ. Chất liệu cao cấp với 85% Nylon và 15% Spandex có độ dày 170 GSM, mang lại cảm giác nhẹ nhàng, thoáng mát và co giãn linh hoạt. Thiết kế phom dáng Regular Fit hiện đại giúp áo vừa vặn, tôn lên vóc dáng mà vẫn thoải mái khi vận động. Đường may sắc nét và tỉ mỉ đảm bảo chất lượng bền đẹp theo thời gian. Sản xuất tại Việt Nam, sản phẩm phù hợp với phong cách thời trang đa năng của phái mạnh.",
					Color = "BE",
					Dimension = "XL",
					Material = "85% Nylon + 15% Spandex – 170 GSM.",
					Quantity = 95,
					Price = 429000,
					Productor = "Việt Nam",
					IsDel = false,
					CategoryId = 5
				},
				new Product
				{
					Id = 4,
					Name = "Áo Polo Nam Iscra MPO 1036",
					Description = "Áo Polo Nam Iscra MPO 1036 có màu trắng tinh tế, mang lại vẻ ngoài thanh lịch và dễ dàng phối hợp với nhiều trang phục. Chất liệu cao cấp gồm 70% Cotton và 30% Iscra giúp áo mềm mại, thoáng khí và giữ form tốt. Thiết kế cổ điển với phom dáng hiện đại tạo sự thoải mái và phù hợp cho mọi hoạt động. Đường may tỉ mỉ đảm bảo độ bền và sự chỉnh chu trong từng chi tiết. Sản xuất tại Việt Nam, sản phẩm đáp ứng tiêu chuẩn chất lượng cao, lý tưởng cho khí hậu Việt Nam.",
					Color = "TRẮNG",
					Dimension = "XL",
					Material = "70% Cotton + 30% Iscra",
					Quantity = 96,
					Price = 449000,
					Productor = "Việt Nam",
					IsDel = false,
					CategoryId = 5
				},
				new Product
				{
					Id = 5,
					Name = "Áo Polo Nam Basic Regular Fit MPO 1038",
					Description = "Áo Polo Nam Basic Regular Fit MPO 1038 có màu đen sang trọng, là lựa chọn hoàn hảo cho phong cách tối giản và hiện đại. Chất liệu thun Pique với 95% Polyester và 5% Spandex, trọng lượng 210 GSM, mang đến độ bền, co giãn nhẹ và thoáng khí tối ưu. Thiết kế Regular Fit giúp áo vừa vặn, phù hợp với nhiều dáng người mà vẫn thoải mái khi mặc. Đường may sắc nét cùng phom dáng chuẩn tạo nên vẻ ngoài chỉnh chu, tinh tế. Sản xuất tại Việt Nam, sản phẩm đáp ứng nhu cầu thời trang và chất lượng cao.",
					Color = "ĐEN",
					Dimension = "XL",
					Material = "Thun Pique. thành phần 95% Polyester. 5% Spandex trọng lượng 210gsm",
					Quantity = 100,
					Price = 349000,
					Productor = "Việt Nam",
					IsDel = false,
					CategoryId = 5
				},
	new Product { Id = 6, Name = "Áo Polo Nam Basic MPO 1033", Description = "Áo Polo Nam Basic MPO 1033 sở hữu màu trắng tinh tế, mang đến vẻ ngoài thanh lịch và dễ dàng phối đồ. Chất liệu Pique CVC với thành phần 70% Cotton, 25% Polyester và 5% Spandex giúp áo mềm mại, thoáng khí, co giãn tốt và bền màu. Thiết kế tối giản với phom dáng hiện đại phù hợp cho cả môi trường công sở và dạo phố. Đường may chắc chắn và tỉ mỉ tạo nên sự bền bỉ và chỉnh chu trong từng chi tiết. Sản xuất tại Việt Nam, áo đáp ứng tiêu chuẩn chất lượng cao với giá cả hợp lý.", Color = "TRẮNG", Dimension = "XL", Material = "Pique CVC. thành phần 70% Cotton + 25% Polyester + 5% Spandex", Quantity = 100, Price = 279000, Productor = "Việt Nam", IsDel = false, CategoryId = 5 },
	new Product { Id = 7, Name = "Áo Polo Nam In Tấm Họa Tiết Summer MPO 1039", Description = "Áo Polo Nam In Tấm Họa Tiết Summer MPO 1039 có màu xanh đen trẻ trung, nổi bật với họa tiết in tấm summer đầy năng động. Chất liệu thun Pique với 65% Polyester và 35% Cotton, trọng lượng 195 GSM, mang lại sự thoáng mát, nhẹ nhàng và thoải mái khi mặc. Thiết kế cổ điển kết hợp với họa tiết in ấn tạo điểm nhấn, giúp bạn dễ dàng tạo phong cách thời trang riêng. Đường may sắc nét và tỉ mỉ, đảm bảo độ bền lâu dài. Sản xuất tại Việt Nam, sản phẩm thích hợp cho những ngày hè năng động và thoải mái.", Color = "XANH ĐEN", Dimension = "XL", Material = "Thun Pique thành phần 65% Polyester. 35% Cotton trọng lượng 195 GSM", Quantity = 100, Price = 314000, Productor = "Việt Nam", IsDel = false, CategoryId = 5 },
	new Product { Id = 8, Name = "Áo Polo Nam Jersey Relax Fit In Typo Trước Ngực MPO 1025", Description = "Áo Polo Nam Jersey Relax Fit In Typo Trước Ngực MPO 1025 màu trắng tinh tế, nổi bật với họa tiết in typo ở phía trước ngực, tạo điểm nhấn thời trang. Chất liệu thun xớ thô OE18, 100% Cotton, trọng lượng 230 GSM mang lại cảm giác mềm mại, thoáng khí và dễ chịu khi mặc. Phom dáng Relax Fit giúp áo thoải mái, không bó sát, phù hợp với nhiều dáng người. Đường may chắc chắn và chi tiết tỉ mỉ, đảm bảo độ bền cao và form áo ổn định. Sản xuất tại Việt Nam, áo là lựa chọn lý tưởng cho phong cách năng động và trẻ trung.", Color = "TRẮNG", Dimension = "XL", Material = "Thun xớ thô OE18. thành phần 100% Cotton. trọng lượng 230GSM", Quantity = 100, Price = 314000, Productor = "Việt Nam", IsDel = false, CategoryId = 5 },
	new Product { Id = 9, Name = "Quần Jean Nam Taper Fit MJE 1019", Description = "Quần Jean Nam Taper Fit MJE 1019 màu xanh đậm với thiết kế taper fit hiện đại, ôm vừa vặn từ phần đùi xuống cổ chân, tôn lên dáng người mạnh mẽ và thời trang. Chất liệu jeans 100% Cotton mang lại sự thoải mái, độ bền cao và khả năng thấm hút mồ hôi tốt. Quần có đường may tỉ mỉ, chắc chắn, giữ form đẹp qua nhiều lần giặt. Phù hợp cho cả môi trường công sở lẫn những buổi đi chơi, tạo nên phong cách năng động và trẻ trung. Sản xuất tại Việt Nam, sản phẩm đảm bảo chất lượng và sự vừa vặn cho người mặc.", Color = "XANH ĐẬM", Dimension = "31", Material = "100% Cotton", Quantity = 100, Price = 699000, Productor = "Việt Nam", IsDel = false, CategoryId = 8 },
	new Product { Id = 10, Name = "Quần Dài Nam Khaki Chinos MPA 1006", Description = "Quần Dài Nam Khaki Chinos MPA 1006 màu be thanh lịch, thiết kế đơn giản nhưng tinh tế, phù hợp cho nhiều dịp từ công sở đến dạo phố. Chất liệu khaki 100% Cotton mang lại sự thoải mái, mềm mại và bền bỉ, đồng thời dễ dàng giặt giũ và giữ form. Phom dáng vừa vặn, tạo cảm giác thoải mái nhưng vẫn tôn lên vóc dáng của người mặc. Đường may sắc nét và chắc chắn, đảm bảo độ bền cao. Sản xuất tại Việt Nam, quần là lựa chọn lý tưởng cho phong cách thời trang hàng ngày.", Color = "BE", Dimension = "31", Material = "100%Cotton", Quantity = 100, Price = 349000, Productor = "Việt Nam", IsDel = false, CategoryId = 8 },
	new Product { Id = 11, Name = "Quần Dài Nam Jeans Slim Fit Co Giãn MJE 1017", Description = "Quần Dài Nam Jeans Slim Fit Co Giãn MJE 1017 màu xanh đậm với thiết kế slim fit ôm vừa vặn, tôn lên vẻ ngoài năng động và hiện đại. Chất liệu jeans 98% Cotton và 2% Spandex mang đến sự co giãn nhẹ nhàng, tạo cảm giác thoải mái và dễ dàng vận động. Đường may tỉ mỉ và chắc chắn, giúp quần giữ form tốt và bền bỉ qua thời gian. Quần thích hợp cho cả môi trường công sở và dạo phố, dễ dàng phối hợp với nhiều kiểu áo khác nhau. Sản xuất tại Việt Nam, sản phẩm đảm bảo chất lượng và sự vừa vặn cho người mặc.", Color = "XANH ĐẬM", Dimension = "31", Material = "98% Cotton + 2% Spandex", Quantity = 99, Price = 649000, Productor = "Việt Nam", IsDel = false, CategoryId = 8 },
	new Product { Id = 12, Name = "Quần Jeans Nam Skinny Rách MJE 1010", Description = "Quần Jeans Nam Skinny Rách MJE 1010 màu xám thời trang, thiết kế ôm sát theo phong cách skinny, tôn lên vẻ ngoài mạnh mẽ và cá tính. Chất liệu thun single 4 chiều xớ mịn với 95% Cotton và 5% Spandex, trọng lượng 190 GSM, mang lại sự co giãn thoải mái, dễ dàng vận động. Điểm nhấn rách ở ống quần thêm phần trẻ trung và năng động. Đường may chắc chắn, đảm bảo độ bền và form quần ổn định sau nhiều lần giặt. Sản xuất tại Việt Nam, quần là lựa chọn hoàn hảo cho những ai yêu thích phong cách hiện đại, cá tính.", Color = "XÁM", Dimension = "31", Material = "Thun single 4 chiều xớ mịn. 95% Cotton – 5% Spandex. trọng lượng 190gsm", Quantity = 100, Price = 414000, Productor = "Việt Nam", IsDel = false, CategoryId = 8 },
	new Product { Id = 13, Name = "Áo Khoác Nam UV Pro Windbreaker MOK 1058", Description = "Áo Khoác Nam UV Pro Windbreaker MOK 1058 màu xám hiện đại, thiết kế nhẹ nhàng và năng động, phù hợp cho các hoạt động ngoài trời. Chất liệu 100% Recycle Poly thân thiện với môi trường, mang đến khả năng chống tia UV hiệu quả và bảo vệ làn da dưới ánh nắng. Áo có đặc tính chống gió, thoáng khí, giúp bạn luôn thoải mái trong mọi điều kiện thời tiết. Đường may tỉ mỉ, phom dáng chuẩn tạo sự vừa vặn và phong cách thời thượng. Sản xuất tại Việt Nam, sản phẩm đảm bảo chất lượng và tiêu chuẩn bền vững.", Color = "XÁM", Dimension = "XL", Material = "100% Recycle Poly.", Quantity = 100, Price = 749000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 14, Name = "Áo Khoác Nam Công Nghệ X-Jacket Version 4 MOP 1049", Description = "Áo Khoác Nam Công Nghệ X-Jacket Version 4 MOP 1049 mang sắc trắng tinh tế, thiết kế hiện đại và đa năng, phù hợp cho phong cách thời trang năng động. Áo được làm từ chất liệu cao cấp với công nghệ tiên tiến, mang lại khả năng chống gió và giữ ấm tối ưu trong mọi điều kiện thời tiết. Thiết kế tỉ mỉ với các đường may sắc nét, đảm bảo độ bền và tính thẩm mỹ cao. Phom dáng chuẩn giúp tôn lên vẻ ngoài thanh lịch và mạnh mẽ của người mặc. Sản xuất tại Việt Nam, sản phẩm là lựa chọn hoàn hảo cho các hoạt động thường ngày hoặc thể thao ngoài trời.", Color = "TRẮNG", Dimension = "XL", Material = "100% Recycle Poly.", Quantity = 100, Price = 649000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 15, Name = "Áo Khoác Nam Thun Sớ Điểm MOK 1053", Description = "Áo Khoác Nam Thun Sớ Điểm MOK 1053 sở hữu màu nâu café sang trọng, thiết kế hiện đại phù hợp cho phong cách thời trang năng động và thanh lịch. Chất liệu thun sớ điểm với thành phần 62% Poly, 33% Cotton và 5% Spandex mang lại sự thoải mái, co giãn tốt và giữ form ổn định. Áo có khả năng giữ ấm nhẹ nhàng, thoáng khí, phù hợp với nhiều điều kiện thời tiết. Đường may tinh tế, tỉ mỉ, đảm bảo độ bền và tăng tính thẩm mỹ. Sản xuất tại Việt Nam, sản phẩm là lựa chọn lý tưởng cho các hoạt động thường ngày hoặc đi làm.", Color = "NÂU CAFÉ", Dimension = "XL", Material = "62% Poly + 33% Cotton + 5% Spandex.", Quantity = 100, Price = 649000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 16, Name = "Áo Khoác Nam Dù Trượt Nước Phối Màu MOP 1048", Description = "Áo Khoác Nam Dù Trượt Nước Phối Màu MOP 1048 màu xanh cobalt nổi bật, thiết kế hiện đại và trẻ trung, là lựa chọn hoàn hảo cho phong cách thời trang năng động. Chất liệu dù 100% Polyester với đặc tính trượt nước ưu việt, bảo vệ hiệu quả trong những ngày mưa nhẹ hoặc gió lạnh. Áo có thiết kế phối màu độc đáo, đường may tinh tế, tạo điểm nhấn thời thượng và đảm bảo độ bền. Phom dáng chuẩn, phù hợp với nhiều dáng người, dễ dàng kết hợp với các trang phục khác. Sản xuất tại Việt Nam, sản phẩm là lựa chọn lý tưởng cho các hoạt động ngoài trời hoặc dạo phố.", Color = "XANH COBALT", Dimension = "XL", Material = "Dù 100% Polyester", Quantity = 100, Price = 799000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 17, Name = "Áo Khoác Thun Nam 2 Da Airlayer MOK 1047", Description = "Áo Khoác Thun Nam 2 Da Airlayer MOK 1047 mang màu xám đốm melange tinh tế, thiết kế hiện đại và linh hoạt, phù hợp cho phong cách thời trang hàng ngày. Chất liệu thun 2 da với thành phần 80% Polyester, 15% Visco và 5% Spandex đạt trọng lượng 300GSM, mang lại cảm giác thoải mái, mềm mại và độ co giãn tốt. Áo được gia công tỉ mỉ, đảm bảo độ bền cao và giữ form chuẩn trong suốt quá trình sử dụng. Thiết kế tối giản với phom dáng gọn gàng, dễ phối đồ và phù hợp với nhiều hoạt động. Sản xuất tại Việt Nam, đây là lựa chọn hoàn hảo cho những ngày thời tiết mát mẻ hoặc se lạnh.", Color = "XÁM ĐỐM MELANGE", Dimension = "XL", Material = "THUN 2 DA - 80% Polyester + 15% Visco + 5% Spandex 300GSM", Quantity = 100, Price = 649000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 18, Name = "Áo Hoodie Nam Basic MHO 1123", Description = "Áo Hoodie Nam Basic MHO 1020 sở hữu màu đen mạnh mẽ và thiết kế tối giản, mang lại phong cách hiện đại và trẻ trung. Chất liệu thun 2 da với thành phần 80% Polyester, 15% Rayon và 5% Spandex, trọng lượng 300GSM, tạo cảm giác mềm mại, co giãn và giữ ấm tốt. Đường may sắc nét và chất liệu cao cấp giúp áo giữ được phom dáng ổn định trong thời gian dài. Thiết kế nón rộng và túi trước tiện lợi, phù hợp cho các hoạt động thường ngày hay tập luyện. Sản xuất tại Việt Nam, sản phẩm là sự lựa chọn lý tưởng cho mùa thu đông.", Color = "ĐEN", Dimension = "XL", Material = "Thun 2 da. 80% Polyester + 15% Rayon + 5% Spandex. 300GSM.", Quantity = 100, Price = 629000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 19, Name = "Áo Khoác Nam Regular Fit Anti UV MOK 1020", Description = "Áo Khoác Nam Regular Fit Anti UV MOK 1020 với màu vàng đồng nổi bật, thiết kế hiện đại và phù hợp với phong cách thể thao năng động. Chất liệu vải Tricot UV với thành phần 60% Cotton và 40% Polyester, trọng lượng 240GSM, giúp bảo vệ da khỏi tác hại của tia UV, mang lại sự thoải mái và mát mẻ cho người mặc. Đặc điểm co giãn tốt, giúp áo ôm sát cơ thể nhưng vẫn linh hoạt trong các chuyển động. Phom dáng regular fit phù hợp với nhiều kiểu dáng cơ thể, dễ dàng phối với các trang phục khác. Sản xuất tại Việt Nam, áo là sự lựa chọn lý tưởng cho các hoạt động ngoài trời, bảo vệ sức khỏe và thời trang.", Color = "VÀNG ĐỒNG", Dimension = "XL", Material = "Vải Tricot UV. thành phần 60% Cotton - 40% Polyester. trọng lượng 240GSM", Quantity = 100, Price = 659000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 20, Name = "Áo Khoác Nam Dù Basic Regular Fit MOP 1040", Description = "Áo Khoác Nam Dù Basic Regular Fit MOP 1040 màu be đơn giản, phù hợp với nhiều phong cách thời trang khác nhau. Chất liệu dù 100% Polyester nhẹ nhàng và bền bỉ, mang lại sự thoải mái khi mặc, đặc biệt trong những ngày thời tiết se lạnh hoặc mưa nhẹ. Thiết kế phom regular fit giúp áo vừa vặn, dễ dàng phối với các trang phục khác. Áo có túi tiện lợi và khóa kéo chắc chắn, tạo nên sự năng động và hiện đại. Sản xuất tại Việt Nam, áo là lựa chọn lý tưởng cho các hoạt động ngoài trời hoặc đi dạo phố.", Color = "BE", Dimension = "XL", Material = "Dù thành phần 100% Polyester", Quantity = 100, Price = 399000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 21, Name = "Áo Khoác Nam Dù Regular Fit Pocketable UltraLight MOP 1036", Description = "Áo Khoác Nam Dù Regular Fit Pocketable UltraLight MOP 1036 màu xanh dương nổi bật, thiết kế nhẹ nhàng và dễ dàng mang theo khi cần. Chất liệu dù gân mỏng, 100% Nylon, có khả năng chống thấm nhẹ, giúp bảo vệ người mặc khỏi gió và mưa nhẹ. Đặc biệt, áo có thể gấp gọn lại thành túi nhỏ, rất tiện lợi khi di chuyển hoặc khi cần mang theo trong balo. Phom regular fit tạo cảm giác thoải mái, dễ dàng phối với nhiều trang phục. Sản xuất tại Việt Nam, áo thích hợp cho các hoạt động ngoài trời hoặc đi du lịch.", Color = "XANH DƯƠNG", Dimension = "XL", Material = "Dù gân mỏng. 100% Nylon", Quantity = 100, Price = 459000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 22, Name = "Áo Khoác Dù Nam Bomber Túi Hộp MOP 1028", Description = "Áo Khoác Dù Nam Bomber Túi Hộp MOP 1028 màu nâu cam mang đến sự trẻ trung và năng động với thiết kế bomber cổ điển. Chất liệu dù HengJi 100% Polyester, trọng lượng 215GSM, bền và nhẹ, giúp bạn cảm thấy thoải mái trong suốt cả ngày. Với các túi hộp tiện lợi ở hai bên, áo không chỉ đẹp mà còn rất tiện dụng. Phom áo vừa vặn với xu hướng thời trang hiện đại, dễ dàng phối đồ với các trang phục khác. Sản xuất tại Việt Nam, áo là lựa chọn lý tưởng cho những buổi đi dạo phố hoặc các hoạt động ngoài trời.", Color = "NÂU CAM", Dimension = "XL", Material = "Dù HengJi. thành phần 100% Polyester. trọng lượng 215GSM", Quantity = 100, Price = 399000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 23, Name = "Áo Khoác Nam Dù Raglan Phối Màu MOP 1033", Description = "Áo Khoác Nam Dù Raglan Phối Màu MOP 1033 màu cam mang đến vẻ ngoài năng động và tươi mới với thiết kế raglan đặc trưng. Chất liệu dù 100% Polyester, bền bỉ và nhẹ nhàng, giúp người mặc cảm thấy thoải mái trong các hoạt động ngoài trời. Với phần phối màu nổi bật ở vai và thân áo, áo tạo điểm nhấn thu hút, dễ dàng phối cùng nhiều trang phục khác. Thiết kế đơn giản nhưng đầy tính ứng dụng, phù hợp cho những chuyến dạo phố hay các hoạt động thể thao nhẹ. Sản xuất tại Việt Nam, áo là lựa chọn tuyệt vời cho mùa thu đông.", Color = "CAM", Dimension = "XL", Material = "Dù thành phần 100% Polyester", Quantity = 100, Price = 399000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 24, Name = "Áo Khoác Nam Thun Gân Chéo MOK 1041", Description = "Áo Khoác Nam Thun Gân Chéo MOK 1041 màu cam đất nổi bật với chất liệu thun gân chéo, mang lại vẻ ngoài vừa cá tính, vừa thanh lịch. Được làm từ 60% Cotton và 40% Polyester, áo có độ bền cao và khả năng thấm hút mồ hôi tốt, giữ cho người mặc luôn thoải mái suốt cả ngày. Trọng lượng vải 245GSM giúp áo có độ dày vừa phải, phù hợp cho mùa thu và mùa đông. Thiết kế đơn giản, dễ phối đồ, có thể kết hợp với nhiều loại trang phục khác nhau, tạo nên phong cách năng động và hiện đại. Sản phẩm được sản xuất tại Việt Nam, đảm bảo chất lượng.", Color = "CAM ĐẤT", Dimension = "XL", Material = "Thun gân chéo. thành phần 60% Cotton - 40% Polyester. trọng lượng 245GSM", Quantity = 100, Price = 649000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 25, Name = "Áo Khoác Nam Jeans Nhuộm Màu MOF 1032", Description = "Áo Khoác Nam Jeans Nhuộm Màu MOF 1032 mang đến phong cách trẻ trung và mạnh mẽ với chất liệu jeans nhuộm màu, làm từ 100% cotton, tạo cảm giác thoải mái và bền bỉ. Với màu nâu đất độc đáo, áo dễ dàng phối hợp với nhiều trang phục khác nhau, từ quần jeans đến quần chinos. Chất liệu jeans cao cấp giúp áo giữ form dáng lâu dài và có độ bền cao. Được sản xuất tại Việt Nam, sản phẩm đảm bảo chất lượng, phù hợp cho cả những ngày dạo phố hay những dịp tụ tập bạn bè.", Color = "NÂU ĐẤT", Dimension = "XL", Material = "Jeans nhuộm màu. thành phần 100% Cotton", Quantity = 100, Price = 479000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
		new Product { Id = 26, Name = "Áo Khoác Dù Nam Ghilet Túi Hộp In Typo MOF 1031", Description = "Áo Khoác Dù Nam Ghilet Túi Hộp In Typo MOF 1031 với chất liệu dù 100% polyester, mang đến sự nhẹ nhàng, thoải mái nhưng vẫn đảm bảo độ bền cao. Với thiết kế ghilet năng động, áo có túi hộp tiện lợi và phần in typo nổi bật, tạo nên phong cách trẻ trung, hiện đại. Màu xanh đậm dễ dàng phối hợp với nhiều trang phục khác nhau. Sản phẩm có trọng lượng 128GSM, giúp người mặc cảm thấy thoáng mát và dễ chịu trong suốt cả ngày dài. Được sản xuất tại Việt Nam, áo là sự lựa chọn lý tưởng cho các hoạt động ngoài trời hoặc dạo phố.", Color = "XANH ĐẬM", Dimension = "XL", Material = "Dù, thành phần 100% Polyester. trọng lượng 128GSM", Quantity = 100, Price = 449000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 27, Name = "Áo Khoác Nam Jean Wash Rách Túi MOF 1008", Description = "Áo Khoác Nam Jean Wash Rách Túi MOF 1008 được làm từ chất liệu jeans 100% cotton, trọng lượng 13.3oz, mang lại cảm giác bền bỉ, chắc chắn nhưng vẫn rất thoải mái khi mặc. Với thiết kế phá cách, áo có các chi tiết wash rách độc đáo cùng túi hộp tiện dụng, tạo nên phong cách năng động và cá tính. Màu xanh nhạt dễ dàng phối đồ và thích hợp cho nhiều dịp. Áo khoác này sẽ là một lựa chọn hoàn hảo để bạn tạo nên một diện mạo thời trang và ấn tượng. Sản phẩm được sản xuất tại Việt Nam, cam kết chất lượng và tính thẩm mỹ cao.", Color = "XANH NHẠT", Dimension = "XL", Material = "Jeans 100% Cotton. trọng lượng 13.3oz", Quantity = 100, Price = 499000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 28, Name = "Áo Khoác Dù Nam Relax Fit Dệt Lỗ Thoáng Khí MOP 1022", Description = "Áo Khoác Dù Nam Relax Fit Dệt Lỗ Thoáng Khí MOP 1022 được làm từ chất liệu dù lỗ thoáng khí 100% polyester, giúp cơ thể luôn mát mẻ, thoải mái trong suốt ngày dài. Thiết kế relax fit mang đến sự dễ chịu cho người mặc, đồng thời tạo phong cách năng động, hiện đại. Màu xanh aqua tươi mát dễ dàng phối hợp với nhiều trang phục khác, phù hợp cho các hoạt động ngoài trời hay dạo phố. Sản phẩm được sản xuất tại Việt Nam, đảm bảo chất lượng và độ bền.", Color = "XANH AQUA", Dimension = "XL", Material = "Dù lỗ thoáng khi 100% polyester", Quantity = 100, Price = 414000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 29, Name = "Áo Khoác Trượt Nước Nam 12in1 Transform Jacket MOP 1020", Description = "Áo Khoác Trượt Nước Nam 12in1 Transform Jacket MOP 1020 là một lựa chọn lý tưởng cho những ai yêu thích các hoạt động ngoài trời, đặc biệt là khi thời tiết thay đổi. Với thiết kế thông minh kết hợp 4 lớp chất liệu, bao gồm dù trượt nước bề mặt, dù thời trang với 15% Polyester tái chế, lớp TPU chống thấm bên trong và thun lưới lót, áo có khả năng bảo vệ bạn khỏi mưa, gió và giữ cho cơ thể luôn thoải mái.", Color = "CAM", Dimension = "XL", Material = "Kết cấu 4 lớp: • Dù trượt nước bề mặt • Dù thời trang 15% Polyester tái chế - 85% Polyester thông thường • Lớp TPU chống thấm bên trong • Thun lưới lót", Quantity = 100, Price = 799000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 30, Name = "Áo Khoác Oversize Kaki Xớ Chéo Diễu Nẹp MOF 1004", Description = "Áo khoác Oversize Kaki Xớ Chéo Diễu Nẹp MOF 1004 được làm từ chất liệu khaki xớ chéo dày dặn 100% cotton, mang đến cảm giác thoải mái và độ bền cao. Thiết kế oversize hiện đại, phù hợp với nhiều phong cách thời trang khác nhau, giúp bạn dễ dàng phối hợp với nhiều loại trang phục. Màu cam tươi sáng, năng động, thích hợp cho các hoạt động ngoài trời hoặc dạo phố. Áo khoác này được sản xuất tại Việt Nam, đảm bảo chất lượng và độ bền.", Color = "CAM", Dimension = "XL", Material = "Khaki Xớ chéo dày dặn 100% cotton", Quantity = 100, Price = 499000, Productor = "Việt Nam", IsDel = false, CategoryId = 3 },
	new Product { Id = 34, Name = "Áo Hoodie Nam Basic MHO 1020", Description = null, Color = "ĐEN", Dimension = "XL", Material = "Thun 2 da. 80% Polyester + 15% Rayon + 5% Spandex. 300GSM.", Quantity = 100, Price = 629000, Productor = "Việt Nam", IsDel = false, CategoryId = 7 },
	new Product { Id = 35, Name = "product 1", Description = null, Color = "1", Dimension = "1", Material = "1", Quantity = 0, Price = null, Productor = "1", IsDel = true, CategoryId = 7 },
	new Product { Id = 36, Name = "Áo Thun Nam Minions Dream Team MTS 1376", Description = "Áo Thun Nam Minions Dream Team MTS 1376 mang phong cách trẻ trung, năng động với màu hồng khói (Rose Smoke) độc đáo. Sản phẩm được làm từ 100% cotton cao cấp, mang lại sự thoáng mát, mềm mại và thoải mái khi mặc. Thiết kế họa tiết Minions 'Dream Team' đáng yêu và nổi bật giúp bạn dễ dàng tạo điểm nhấn trong mọi set đồ. Được sản xuất tại Việt Nam với mức giá 329.000 VND, đây là lựa chọn lý tưởng cho những tín đồ thời trang yêu thích sự mới lạ và cá tính.", Color = "HỒNG KHÓI ROSE SMOKE", Dimension = "XL", Material = "100% Cotton", Quantity = 97, Price = 329000, Productor = "Việt Nam", IsDel = false, CategoryId = 4 },
	new Product { Id = 38, Name = "Áo Thun Nam Typo In Thêu Think Green MTS 1303", Description = "Áo Thun Nam Typo In Thêu Think Green MTS 1303 với chất liệu cao cấp từ 79.36% sợi bắp - cotton và 20.64% ISCRA mang đến sự thoáng mát, mềm mại và thân thiện với môi trường. Thiết kế màu trắng tinh tế kết hợp họa tiết typo 'Think Green' được in thêu tỉ mỉ, tạo điểm nhấn trẻ trung, hiện đại. Sản xuất tại Việt Nam với chất lượng gia công chuẩn chỉnh, sản phẩm không chỉ bền đẹp mà còn truyền tải thông điệp sống xanh đầy ý nghĩa.", Color = "TRẮNG", Dimension = "XL", Material = "SỢI BẮP - 79.36% COTTON; 20.64% ISCRA", Quantity = 100, Price = 329000, Productor = "Việt Nam", IsDel = false, CategoryId = 4 });

			// Seed PurchaseReportProductDetail
			modelBuilder.Entity<PurchaseReportProductDetail>().HasData(
				new PurchaseReportProductDetail { Id = 1, ProductId = 1, PurchaseReportId = 1, Quantity = 100, PricePurchase = 429000 },
				new PurchaseReportProductDetail { Id = 2, ProductId = 2, PurchaseReportId = 1, Quantity = 100, PricePurchase = 449000 },
				new PurchaseReportProductDetail { Id = 3, ProductId = 3, PurchaseReportId = 1, Quantity = 100, PricePurchase = 429000 },
				new PurchaseReportProductDetail { Id = 4, ProductId = 4, PurchaseReportId = 1, Quantity = 100, PricePurchase = 449000 },
				new PurchaseReportProductDetail { Id = 5, ProductId = 5, PurchaseReportId = 1, Quantity = 100, PricePurchase = 349000 },
				new PurchaseReportProductDetail { Id = 6, ProductId = 6, PurchaseReportId = 1, Quantity = 100, PricePurchase = 279000 },
				new PurchaseReportProductDetail { Id = 7, ProductId = 7, PurchaseReportId = 1, Quantity = 100, PricePurchase = 314000 },
				new PurchaseReportProductDetail { Id = 8, ProductId = 8, PurchaseReportId = 1, Quantity = 100, PricePurchase = 314000 },
				new PurchaseReportProductDetail { Id = 9, ProductId = 9, PurchaseReportId = 2, Quantity = 100, PricePurchase = 699000 },
				new PurchaseReportProductDetail { Id = 10, ProductId = 10, PurchaseReportId = 2, Quantity = 100, PricePurchase = 349000 },
				new PurchaseReportProductDetail { Id = 11, ProductId = 11, PurchaseReportId = 2, Quantity = 100, PricePurchase = 649000 },
				new PurchaseReportProductDetail { Id = 12, ProductId = 12, PurchaseReportId = 2, Quantity = 100, PricePurchase = 414000 },
				new PurchaseReportProductDetail { Id = 13, ProductId = 13, PurchaseReportId = 3, Quantity = 100, PricePurchase = 749000 },
				new PurchaseReportProductDetail { Id = 14, ProductId = 14, PurchaseReportId = 3, Quantity = 100, PricePurchase = 649000 },
				new PurchaseReportProductDetail { Id = 15, ProductId = 15, PurchaseReportId = 3, Quantity = 100, PricePurchase = 649000 },
				new PurchaseReportProductDetail { Id = 16, ProductId = 16, PurchaseReportId = 3, Quantity = 100, PricePurchase = 799000 },
				new PurchaseReportProductDetail { Id = 17, ProductId = 17, PurchaseReportId = 3, Quantity = 100, PricePurchase = 649000 },
				new PurchaseReportProductDetail { Id = 18, ProductId = 18, PurchaseReportId = 3, Quantity = 100, PricePurchase = 629000 },
				new PurchaseReportProductDetail { Id = 19, ProductId = 19, PurchaseReportId = 3, Quantity = 100, PricePurchase = 659000 },
				new PurchaseReportProductDetail { Id = 20, ProductId = 20, PurchaseReportId = 3, Quantity = 100, PricePurchase = 399000 },
				new PurchaseReportProductDetail { Id = 21, ProductId = 21, PurchaseReportId = 3, Quantity = 100, PricePurchase = 459000 },
				new PurchaseReportProductDetail { Id = 22, ProductId = 22, PurchaseReportId = 3, Quantity = 100, PricePurchase = 399000 },
				new PurchaseReportProductDetail { Id = 23, ProductId = 23, PurchaseReportId = 3, Quantity = 100, PricePurchase = 399000 },
				new PurchaseReportProductDetail { Id = 24, ProductId = 24, PurchaseReportId = 3, Quantity = 100, PricePurchase = 649000 },
				new PurchaseReportProductDetail { Id = 25, ProductId = 25, PurchaseReportId = 3, Quantity = 100, PricePurchase = 479000 },
				new PurchaseReportProductDetail { Id = 26, ProductId = 26, PurchaseReportId = 3, Quantity = 100, PricePurchase = 449000 },
				new PurchaseReportProductDetail { Id = 27, ProductId = 27, PurchaseReportId = 3, Quantity = 100, PricePurchase = 499000 },
				new PurchaseReportProductDetail { Id = 28, ProductId = 28, PurchaseReportId = 3, Quantity = 100, PricePurchase = 414000 },
				new PurchaseReportProductDetail { Id = 29, ProductId = 29, PurchaseReportId = 3, Quantity = 100, PricePurchase = 799000 },
				new PurchaseReportProductDetail { Id = 30, ProductId = 30, PurchaseReportId = 3, Quantity = 100, PricePurchase = 499000 },
				new PurchaseReportProductDetail { Id = 34, ProductId = 34, PurchaseReportId = 4, Quantity = 100, PricePurchase = 629000 },
				new PurchaseReportProductDetail { Id = 36, ProductId = 36, PurchaseReportId = 5, Quantity = 100, PricePurchase = 329000 },
				new PurchaseReportProductDetail { Id = 38, ProductId = 38, PurchaseReportId = 6, Quantity = 100, PricePurchase = 329000 },
				new PurchaseReportProductDetail { Id = 39, ProductId = 36, PurchaseReportId = 6, Quantity = 100, PricePurchase = 329000 }
				);

			// Seed PurchaseReport
			modelBuilder.Entity<PurchaseReport>().HasData(
				 new PurchaseReport
				 {
					 Id = 1,
					 CodePurchaseReport = "HD001",
					 DatePurchase = new DateOnly(2024, 10, 1),
					 OtherCost = 0,
					 Note = null,
					 IsUpdate = true,
					 IsDel = false,
					 SupplierId = 3,
					 UserId = null
				 },
				new PurchaseReport
				{
					Id = 2,
					CodePurchaseReport = "HD002",
					DatePurchase = new DateOnly(2024, 9, 1),
					OtherCost = 0,
					Note = null,
					IsUpdate = true,
					IsDel = false,
					SupplierId = 5,
					UserId = null
				},
				new PurchaseReport
				{
					Id = 3,
					CodePurchaseReport = "HD003",
					DatePurchase = new DateOnly(2024, 12, 1),
					OtherCost = 0,
					Note = null,
					IsUpdate = true,
					IsDel = false,
					SupplierId = 1,
					UserId = null
				},
				new PurchaseReport
				{
					Id = 4,
					CodePurchaseReport = "HD004",
					DatePurchase = new DateOnly(2024, 8, 1),
					OtherCost = 0,
					Note = null,
					IsUpdate = true,
					IsDel = false,
					SupplierId = 4,
					UserId = null
				},
				new PurchaseReport
				{
					Id = 5,
					CodePurchaseReport = "HD0012",
					DatePurchase = new DateOnly(2024, 12, 17),
					OtherCost = 0,
					Note = null,
					IsUpdate = false,
					IsDel = false,
					SupplierId = 2,
					UserId = null
				},
				new PurchaseReport
				{
					Id = 6,
					CodePurchaseReport = "HD321",
					DatePurchase = new DateOnly(2024, 12, 16),
					OtherCost = 0,
					Note = null,
					IsUpdate = true,
					IsDel = false,
					SupplierId = 2,
					UserId = null
				}
				);

			// Seed Supplier
			modelBuilder.Entity<Supplier>().HasData(
				 new Supplier
				 {
					 Id = 1,
					 Name = "Công Ty TNHH Thời Trang Áo Khoác Nam Hòa Bình",
					 Address = "123 Đường Lê Duẩn, Khu Phố 1, Quận 1, TP. Hồ Chí Minh",
					 Phone = "0901112233",
					 IsDel = false
				 },
				new Supplier
				{
					Id = 2,
					Name = "Công Ty Cổ Phần Thời Trang Áo Thun Nam Thành Công",
					Address = "456 Đường Nguyễn Thái Học, Phường 12, Quận 10, TP. Hồ Chí Minh",
					Phone = "0902223344",
					IsDel = false
				},
				new Supplier
				{
					Id = 3,
					Name = "Công Ty TNHH Áo Polo Nam Nguyên Khôi Fashion",
					Address = "789 Đường Trần Hưng Đạo, Phường 6, Quận 5, TP. Hồ Chí Minh",
					Phone = "0903334455",
					IsDel = false
				},
				new Supplier
				{
					Id = 4,
					Name = "Công Ty TNHH Áo Sweater Nam Tâm An Apparel",
					Address = "321 Đường Hùng Vương, Phường 9, Quận 10, TP. Hồ Chí Minh",
					Phone = "0904445566",
					IsDel = false
				},
				new Supplier
				{
					Id = 5,
					Name = "Công Ty TNHH Quần Dài Nam Minh Tuấn Clothing",
					Address = "654 Đường Nguyễn Văn Cừ, Phường 2, Quận 5, TP. Hồ Chí Minh",
					Phone = "0905556677",
					IsDel = false
				},
				new Supplier
				{
					Id = 6,
					Name = "Công Ty Cổ Phần Quần Jean Nam Vĩnh Phúc Fashion 1",
					Address = "852 Đường Cách Mạng Tháng Tám, Phường 8, Quận 3, TP. Hồ Chí Minh",
					Phone = "0906667788",
					IsDel = false
				}
				);

			//Seed Order
			modelBuilder.Entity<Order>().HasData(
				new Order
				{
					Id = 1,
					DateOrder = new DateOnly(2024, 11, 25),
					DateReceive = new DateOnly(2024, 11, 25),
					DeliveryCost = 50000,
					Status = "Đã thanh toán",
					Note = null,
					IsDel = false,
					AdminId = null,
					CustomerId = "feef8538-ecae-4502-8fb2-7b3d33068777"
				},
				new Order
				{
					Id = 2,
					DateOrder = new DateOnly(2024, 11, 25),
					DateReceive = null,
					DeliveryCost = 50000,
					Status = "Đã đặt hàng",
					Note = null,
					IsDel = false,
					AdminId = null,
					CustomerId = "feef8538-ecae-4502-8fb2-7b3d33068777"
				},
				new Order
				{
					Id = 3,
					DateOrder = new DateOnly(2024, 11, 30),
					DateReceive = null,
					DeliveryCost = 50000,
					Status = "Đã đặt hàng",
					Note = null,
					IsDel = false,
					AdminId = null,
					CustomerId = "feef8538-ecae-4502-8fb2-7b3d33068777"
				},
				new Order
				{
					Id = 4,
					DateOrder = new DateOnly(2024, 12, 10),
					DateReceive = new DateOnly(2024, 12, 10),
					DeliveryCost = 50000,
					Status = "Đã thanh toán",
					Note = null,
					IsDel = false,
					AdminId = null,
					CustomerId = "feef8538-ecae-4502-8fb2-7b3d33068777"
				},
				new Order
				{
					Id = 5,
					DateOrder = new DateOnly(2024, 12, 16),
					DateReceive = new DateOnly(2024, 12, 16),
					DeliveryCost = 50000,
					Status = "Đã thanh toán",
					Note = null,
					IsDel = false,
					AdminId = null,
					CustomerId = "feef8538-ecae-4502-8fb2-7b3d33068777"
				},
				new Order
				{
					Id = 6,
					DateOrder = new DateOnly(2024, 12, 31),
					DateReceive = new DateOnly(2024, 12, 31),
					DeliveryCost = 50000,
					Status = "Đã thanh toán",
					Note = null,
					IsDel = false,
					AdminId = null,
					CustomerId = "feef8538-ecae-4502-8fb2-7b3d33068777"
				}
				);
			// Seeder OrderDetail
			modelBuilder.Entity<OrderProductDetail>().HasData(
				new OrderProductDetail
				{
					Id = 1,
					Quantity = 2,
					PriceSale = 449000,
					ProductId = 2,
					OrderId = 1
				},
				new OrderProductDetail
				{
					Id = 3,
					Quantity = 1,
					PriceSale = 429000,
					ProductId = 3,
					OrderId = 2
				},
				new OrderProductDetail
				{
					Id = 4,
					Quantity = 2,
					PriceSale = 429000,
					ProductId = 3,
					OrderId = 2
				},
				new OrderProductDetail
				{
					Id = 5,
					Quantity = 2,
					PriceSale = 429000,
					ProductId = 3,
					OrderId = 3
				},
				new OrderProductDetail
				{
					Id = 6,
					Quantity = 4,
					PriceSale = 449000,
					ProductId = 4,
					OrderId = 3
				},
				new OrderProductDetail
				{
					Id = 7,
					Quantity = 1,
					PriceSale = 649000,
					ProductId = 11,
					OrderId = 4
				},
				new OrderProductDetail
				{
					Id = 8,
					Quantity = 1,
					PriceSale = 329000,
					ProductId = 36,
					OrderId = 5
				},
				new OrderProductDetail
				{
					Id = 9,
					Quantity = 2,
					PriceSale = 329000,
					ProductId = 36,
					OrderId = 6
				}
				);
			base.OnModelCreating(modelBuilder);
		}


	}
}





