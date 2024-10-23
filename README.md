#  [update: 23/10/2024]`
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
