using eStoreOnline;
using eStoreOnline.Application;
using eStoreOnline.Infrastructure;
using eStoreOnline.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion("8.3")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<SeedData>(n =>
{
    var dbContext = n.GetRequiredService<ApplicationDbContext>();
    var userManager = n.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = n.GetRequiredService<RoleManager<IdentityRole>>();
    return new SeedData(dbContext, userManager, roleManager);
});

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<SeedData>().Seed();

var stripeConfiguration = app.Services
    .GetRequiredService<IOptions<eStoreOnline.Infrastructure.Configurations.StripeConfiguration>>();
StripeConfiguration.ApiKey = stripeConfiguration.Value.ApiKey;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "" }
);

app.MapControllerRoute(
    name: "product-index",
    pattern: "product/{pageIndex}",
    defaults: new { controller = "Product", action = "Index" });

app.MapControllerRoute(
    name: "product-slug",
    pattern: "product/detail/{urlSlug}",
    defaults: new { controller = "Product", action = "Detail" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();