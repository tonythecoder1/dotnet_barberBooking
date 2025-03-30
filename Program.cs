using dotidentity;
using dotidentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ATIVAR LOGGING NO CONSOLE
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // 👈 Adiciona logs no terminal
builder.Logging.SetMinimumLevel(LogLevel.Debug); // 👈 mostra tudo

var connectionString = @"Server=127.0.0.1;Port=3306;Database=dotnet_db;User ID=root;Password=12345678;";

builder.Services.AddDbContext<MyUserDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        mysqlOptions => mysqlOptions.MigrationsAssembly("dotidentity")));

//  Configurar Identity com UserStore customizado
builder.Services.AddIdentity<MyUser, IdentityRole>(options => { })
    .AddEntityFrameworkStores<MyUserDbContext>()    //Use este DbContext (MyUserDbContext) para salvar e buscar os usuários, roles, claims, tokens... no banco.”
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<MyUser>,
    MyUserClaimsFactory>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options => {
    options.TokenLifespan = TimeSpan.FromHours(3);
});


// Registrar UserOnlyStore com o DbContext agora disponível
builder.Services.AddScoped<IUserStore<MyUser>, UserOnlyStore<MyUser, MyUserDbContext>>();

//  Autenticação por cookies
  
    builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");
 

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
