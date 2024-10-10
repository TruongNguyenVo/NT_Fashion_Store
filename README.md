# Package 
    + 'How to install package:' right-click in project -> manage nuget package
    + 'Packages: [10/10/2024]'
        Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
        Microsoft.AspNetCore.Identity.EntityFrameworkCore
        Microsoft.AspNetCore.Identity.UI
        Microsoft.EntityFrameworkCore.Sqlite
        Microsoft.EntityFrameworkCore.SqlServer
        Microsoft.EntityFrameworkCore.Tools
        Microsoft.VisualStudio.Web.CodeGeneration.Design
### `Create dummy data`
```bash

php artisan db:seed

php artisan make:seeder *(name of seed)Seeder

php artisan db:seed --class=*(name of seed)Seeder
```