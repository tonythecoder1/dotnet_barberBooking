using dotidentity;
using dotidentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = @"Server=127.0.0.1;Port=3306;Database=dotnet_db;User ID=root;Password=12345678;";

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        mysqlOptions => mysqlOptions.MigrationsAssembly("dotidentity")));

//  Configurar Identity com UserStore customizado
builder.Services.AddIdentityCore<IdentityUser>(options => { })
    .AddUserStore<MyIdentityUserStore>()
    .AddDefaultTokenProviders();

// Registrar UserOnlyStore com o DbContext agora disponível
builder.Services.AddScoped<IUserStore<IdentityUser>, UserOnlyStore<IdentityUser, IdentityDbContext>>();

//  Autenticação por cookies
builder.Services.AddAuthentication("cookies")
    .AddCookie("cookies", options => 
    {
        options.LoginPath = "/Home/Login";
    });

//  MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

//  Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
