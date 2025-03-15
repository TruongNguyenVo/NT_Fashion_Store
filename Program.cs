using doan1_v1.Helpers;
using doan1_v1.Models;
using doan1_v1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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

// Thêm dịch vụ Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
	var googleAuth = builder.Configuration.GetSection("Authentication:Google");
	options.ClientId = googleAuth["ClientId"];
	options.ClientSecret = googleAuth["ClientSecret"];
	options.CallbackPath = new PathString(new Uri(googleAuth["RedirectUri"]).AbsolutePath);
});

// Đăng ký service
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.SameSite = SameSiteMode.None;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// paypal
builder.Services.AddDistributedMemoryCache(); // Dùng bộ nhớ trong làm cache
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
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


//using (var scope = app.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;
//	await SeederRoleUser.Initialize(services); //tao cac role ke thua user
//}
app.UseSession(); // Thêm middleware để kích hoạt session
app.Run();
