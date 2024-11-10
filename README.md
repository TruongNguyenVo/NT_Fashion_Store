#  [update: 10/11/2024]
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
1. chuột phải project -> add new scafford item -> indetity 
2. lựa chọn layout, chức năng (login, register), dbcontext và model
3. thêm trong program.cs
	using authorize.Areas.Identity.Data; //thu vien
	//// Add services to the container.
	builder.Services.AddControllersWithViews();
	builder.Services.AddRazorPages(); //////////them
	app.MapRazorPages(); /////////////them
