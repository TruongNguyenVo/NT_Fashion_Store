#  [update: 12/11/2024]
`Run Project`
#### `how to run project asp core in powershell`: open powershell
```bash
dotnet run --urls "http://0.0.0.0:5000"
```
# `Package`
#### `how to install package asp core in powershell`: open powershell
```bash
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
```
# `Migration to Database`
#### `Run PM: Tools -> Nuget Package Manager -> Package Manager Control`
```bash
    Add-Migration NameYourMigration
```
```bash
    Update-Database -Migration NameYourMigration
```
# `Registration, Login, Logout`
1. Sửa Model User
1.1 Kế thừa IdentityUser
   ```bash
       public class User: IdentityUser 
   ```
1.2 Xóa các trường password (vì trong IdentityUser đã có các trường đó rồi)
2. Sửa Model ApplicationDbContext 
2.1 Kế thừa IdentityDbContext
```bash
    public class ApplicationDbContext : IdentityDbContext<User>
```
3. Cấu hình trong Program.cs
```bash
// Cấu hình Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();
//Cấu hình Authorize
builder.Services.AddAuthorization(options =>
{
    // Policy cho Customer
    options.AddPolicy("CustomerOnly", policy =>
        policy.RequireRole("Customer"));

    // Policy cho Admin
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    // Policy cho Admin và Customer (nếu cần)
    options.AddPolicy("AdminOrCustomer", policy =>
        policy.RequireRole("Admin", "Customer"));
});
// Không có quyền thì bị đá vào Controller Account - Action:AccessDenied
builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/Account/AccessDenied";  // Trang xử lý khi bị từ chối quyền truy cập
});
//các cấu hình builder trên để trên var app = builder.Build();

// Thêm xác thực và ủy quyền
app.UseAuthentication();
app.UseAuthorization();
//các cấu hình app để ở dưới app.Run();
```
4. Rebuild và fix bug (nếu có)
5. Add-Migration
6. Update-database
7. Tạo AccountController và View/Account: để viết và hiển thị các chức năng Register, Login, Logout, Authorize
8. Giới hạn quyền được giới hạn trong trước các Action
   8.1 Chỉ có Customer
   ```bash
   [Authorize(Roles = "Customer")]
   public async Task<IActionResult> Index()
   ```
   8.2 Chỉ có Admin
   ```bash
           [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
   ```
   8.3 Cả 2 người đều được (sử dụng Policy)
   ```bash
   [Authorize(Policy = "AdminOrCustomer")]
	public IActionResult Dashboard()
	{
	    return View();
	}

   ```
