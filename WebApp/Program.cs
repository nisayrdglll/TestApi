using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataModels.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DataAccess.Handlers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection;
using Core;
using DataAccess.Context;
using LogLib;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

// Npgsql için legacy timestamp davranışını etkinleştir
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#if DEBUG
var connectionString = builder.Configuration.GetConnectionString("TestConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
#else
    var connectionString = builder.Configuration.GetConnectionString("ProdConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
#endif

builder.Services.AddDbContext<DbContext>(options =>
               options.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.SetPostgresVersion(12, 9).EnableRetryOnFailure();
                        sqlOptions.CommandTimeout(3600);
                        sqlOptions.EnableRetryOnFailure();
                    }
                    ).UseSnakeCaseNamingConvention());
builder.Services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = true; options.User.RequireUniqueEmail = false; }).AddRoles<ApplicationRole>().AddEntityFrameworkStores<MtIdentityDbContext>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddHttpContextAccessor();

#region RoleAuth
builder.Services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.Cookie.Name = ".AspNetCore.Identity.Mta." + Assembly.GetExecutingAssembly().FullName.Split(',')[0];
    option.ExpireTimeSpan = TimeSpan.FromHours(72);
    option.LoginPath = new PathString("/Account/Login");
    option.SlidingExpiration = true;
});

builder.Services.AddSession();

#region Service Register
//Servisler buradaki ServiceRegister
ServiceRegister.Register(builder.Services);
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddMemoryCache();
#endregion

//Identity Password Konfir�rasyonu
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

builder.Services.AddControllersWithViews();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Logging.AddDbLogger(options =>
{
    builder.Configuration.GetSection("Logging").GetSection("Database").GetSection("Options").Bind(options);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(_ => { });
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

#region Session Transactions
app.UseSession();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
#endregion

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

ServiceActivator.Configure(((IApplicationBuilder)app).ApplicationServices);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
