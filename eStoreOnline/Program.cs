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
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

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