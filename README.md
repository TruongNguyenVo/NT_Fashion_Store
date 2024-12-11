#  [update: 11/12/2024]
# `Start project`
## Tạo project 
```bash
dotnet new webapi -o MyAspNetApp
cd MyAspNetApp
```
### Kiểm tra ứng dụng hoạt động:
Với hướng localhost:
```bash
dotnet run
```
Với hướng tất cả các IP đều có thể truy cập được:
```bash
dotnet run --urls "http://0.0.0.0:5000"
```
Nếu muốn truy cập thì thay `0.0.0.0` thành IP của máy chủ
## Cài đặt các package
how to install package asp core in powershell: open `powershell`
```bash
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
```
## Migration to Database
Run PM: `Tools` -> `Nuget Package Manager` -> `Package Manager Control`
```bash
    Add-Migration NameYourMigration
```
```bash
    Update-Database -Migration NameYourMigration
```
# `Registration, Login, Logout`
## 1. Sửa Model User  
  1.1 Kế thừa IdentityUser
   ```bash
       public class User: IdentityUser 
   ```
  1.2 Xóa các trường password (vì trong IdentityUser đã có các trường đó rồi)
## 2. Sửa Model ApplicationDbContext 
2.1 Kế thừa IdentityDbContext
```bash
    public class ApplicationDbContext : IdentityDbContext<User>
```
## 3. Cấu hình trong Program.cs
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
```
```bash
// Thêm xác thực và ủy quyền
app.UseAuthentication();
app.UseAuthorization();
//các cấu hình app để ở dưới app.Run();
```
## 4. Rebuild và fix bug (nếu có)
## 5. Add-Migration
## 6. Update-database
## 7. Tạo AccountController và View/Account: để viết và hiển thị các chức năng Register, Login, Logout, Authorize
## 8. Giới hạn quyền được giới hạn trong trước các Action
### 8.1 Chỉ có Customer
```bash
   [Authorize(Roles = "Customer")]
   public async Task<IActionResult> Index()
```
### 8.2 Chỉ có Admin
```bash
           [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
```
### 8.3 Cả 2 người đều được (sử dụng Policy)
```bash
   [Authorize(Policy = "AdminOrCustomer")]
	public IActionResult Dashboard()
	{
	    return View();
	}
```
## 9. Kiểm tra người dùng đã đăng nhập hay chưa
```bash
   @if (User.Identity.IsAuthenticated)

	{
   @using Microsoft.AspNetCore.Identity;
    @using Microsoft.AspNetCore.Mvc;
    @inject UserManager<User> UserManager
	    var user = await UserManager.GetUserAsync(User);
	    <p>@user?.Id</p>// ID người dùng
	    <p>@user?.UserName</p>
   }
```
# `Chạy project ASP.NET CORE độc lập mà không cần cài môi trường .NET runtime (self-contained deployment)`
## 1. Tạo project mới
### 1.1 Tạo project 
```bash
dotnet new webapi -o MyAspNetApp
cd MyAspNetApp
```
### 1.2 Kiểm tra ứng dụng hoạt động: Mở trình duyệt và truy cập http://localhost:5000 hoặc http://localhost:5001 (HTTPS) để kiểm tra.
```bash
dotnet run
```
## 2. Cấu hình cho Self-Contained Deployment
#### 2.1 Chỉnh sửa file .csproj:
Mở file `MyAspNetApp.csproj` và thêm thuộc tính sau nếu muốn tích hợp nhiều `RuntimeIdentifiers` (số nhiều):
```bash
		<PropertyGroup>
		  <PublishSingleFile>true</PublishSingleFile>
		  <SelfContained>true</SelfContained>
		  <RuntimeIdentifiers>win-x64;linux-x64;osx-x64</RuntimeIdentifiers>
		</PropertyGroup>
```
#### 2.2. Cấu hình Kestrel để lắng nghe tất cả địa chỉ IP: Trong `Program.cs`, bạn có thể thêm cấu hình cho Kestrel:
```bash
      	var builder = WebApplication.CreateBuilder(args);	
	builder.WebHost.UseKestrel()
	       .UseUrls("http://0.0.0.0:5000"); // Lắng nghe trên tất cả các IP	
	var app = builder.Build();
	app.Run();
```
#### 2.3. Build riêng cho từng Runtime:
```bash
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r linux-x64
dotnet publish -c Release -r osx-x64
```
#### 2.4 Thư mục xuất (ở trong chính project luôn): thư mục sẽ một file thực thi duy nhất (MyAspNetApp.exe trên Windows hoặc MyAspNetApp trên Linux/Mac).
```bash
/bin/Release/net7.0/win-x64/publish/
/bin/Release/net7.0/linux-x64/publish/
/bin/Release/net7.0/osx-x64/publish/
 ```
## 3. Triển khai và Chạy Ứng dụng
### 3.1 Copy File sang Máy Chủ:
Copy toàn bộ nội dung thư mục publish/ sang máy chủ đích.  
Trên Linux, đảm bảo file thực thi có quyền chạy:
```bash
chmod +x MyAspNetApp
```
### 3.2 Chạy ứng dụng
Window: chạy file ` .exe `  
Linux: chạy thực thi trong terminal: `./MyAspNetApp`
## 4. Nếu muốn ứng dụng tự động chạy khi khởi động máy chủ (Linux):
### 4.1 Tạo Service File ( `/etc/systemd/system/myaspnetapp.service` ):
```bash
sudo nano /etc/systemd/system/myaspnetapp.service
```
Nội dung file:
```bash
[Unit]
Description=My ASP.NET Core Application
After=network.target
[Service]
ExecStart=/path/to/MyAspNetApp
WorkingDirectory=/path/to/
Restart=always
User=yourusername
Environment=ASPNETCORE_ENVIRONMENT=Production
[Install]
WantedBy=multi-user.target
```
### 4.2 Reload và Khởi động Service:
```bash
sudo systemctl daemon-reload
sudo systemctl start myaspnetapp
sudo systemctl enable myaspnetapp
```
### 4.3 Kiểm tra trạng thái:
```bash
sudo systemctl status myaspnetapp
```
