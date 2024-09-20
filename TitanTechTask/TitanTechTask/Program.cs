using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TitanTechTask.Data;
using TitanTechTask.Manager;
using TitanTechTask.Data.SeedData;
using TitanTechTask.Data.Migration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using TitanTechTask.Domain.Account;
using TitanTechTask.Data.Providers;
using TitanTechTask.Data.Models;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});

builder.Services.AddIdentity<UserDomain, IdentityRole>()
    .AddRoleStore<CustomRoleStore>() // Use custom RoleStore implementation
    .AddUserStore<CustomUserStore>() // Use your custom UserStore
    .AddSignInManager<SignInManager<UserDomain>>() // SignInManager for login
    .AddDefaultTokenProviders();

// Configure cookie authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Register the custom ApplicationDbContext for ADO.NET
builder.Services.AddSingleton<ApplicationDbContext>();

builder.Services.ConfigureDataModule(configuration);

builder.Services.ConfigureManagerModule(configuration);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Run the migrations using ADO.NET
MigrationsExecution.RunMigrations(connectionString);

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Register your custom middleware

// Create a scope to resolve services
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeederExecutor seederExecutor = new SeederExecutor(context);
    seederExecutor.ExecuteSeeders();
}

// Continue with the rest of your application logic
Console.WriteLine("All seeding done.");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

//app.UseMvcWithDefaultRoute();

app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();
