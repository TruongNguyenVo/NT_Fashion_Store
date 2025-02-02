using doan1_v1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
/////////////////////////////
//var connectionString = builder.Configuration.GetConnectionString("Connectiong_1") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<NTFashionDbContext>(options =>
// options.UseSqlServer(connectionString));

// Thêm DbContext và cấu hình SQLite
builder.Services.AddDbContext<NTFashionDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

/////////////////////////////
///////////////////////////
///cau hinh identity
// Cấu hình Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NTFashionDbContext>()
    .AddDefaultTokenProviders();
//Cấu hình Authorize
builder.Services.AddAuthorization(options =>
{
    // Policy cho Customer
    options.AddPolicy("CustomerOnly", policy =>
        policy.RequireRole("Customer"));

    // Policy cho Manager
    options.AddPolicy("ManagerOnly", policy =>
        policy.RequireRole("Manager"));

    // Policy cho Admin và Customer (nếu cần)
    options.AddPolicy("ManagerOrCustomer", policy =>
        policy.RequireRole("Manager", "Customer"));
});
// Không có quyền thì bị đá vào Controller Account - Action:AccessDenied
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/SignIn"; // Đường dẫn khi người dùng chưa đăng nhập
	options.AccessDeniedPath = "/Account/AccessDenied";  // Trang xử lý khi bị từ chối quyền truy cập
});
//////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Thêm xác thực và ủy quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
