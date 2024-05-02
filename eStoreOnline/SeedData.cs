using eStoreOnline.Domain.Commons;
using eStoreOnline.Domain.Entities;
using eStoreOnline.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eStoreOnline;

public class SeedData
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public SeedData(ApplicationDbContext context, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        await SeedUsers();
        await SeedRoles();
        await SeedProduct();
    }

    private async Task SeedRoles()
    {
        var isAdminRoleExist = _roleManager.Roles.Any(r => r.Name == RoleConstants.Admin);
        if (!isAdminRoleExist)
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleConstants.Admin,
                NormalizedName = RoleConstants.Admin.ToUpper()
            });
        }
    }

    private async Task SeedUsers()
    {
        var isSysAdminExists = await _context.Users.AnyAsync(x => x.Email == "systemadmin@example.com");
        if (isSysAdminExists)
        {
            return;
        }

        var defaultUser = new IdentityUser()
        {
            Id = Guid.Empty.ToString(),
            Email = "systemadmin@example.com",
            UserName = "systemadmin@example.com",
            NormalizedEmail = "SYSTEMADMIN@EXAMPLE.COM",
            NormalizedUserName = "SYSTEMADMIN@EXAMPLE.COM",
            EmailConfirmed = true
        };

        await CreateUserAsync(defaultUser, "SystemAdmin@123");
        await _userManager.AddToRoleAsync(defaultUser, RoleConstants.Admin);
        
        var isAdminExists = await _context.Users.AnyAsync(x => x.Email == "admin@example.com");
        if (isAdminExists)
        {
            return;
        }
        
        var adminUser = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@example.com",
            UserName = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            NormalizedUserName = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true
        };

        await CreateUserAsync(adminUser, "Admin@123");
        await _userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
    }

    private async Task CreateUserAsync(IdentityUser user, string password)
    {
        var exists = await _userManager.FindByEmailAsync(user.Email ?? string.Empty);
        if (exists == null)
            await _userManager.CreateAsync(user, password);
    }

    private async Task SeedProduct()
    {
        var isProductExists = await _context.Products.AnyAsync();
        if (isProductExists)
        {
            return;
        }

        var sysAdmin = await _context.Users.FirstOrDefaultAsync(x => x.Email == "systemadmin@example.com");

        if (sysAdmin == null)
            return;

        var listProducts = new List<Product>()
        {
        };

        for (var i = 0; i < 50; i++)
        {
            listProducts.Add(new Product
            {
                ProductName = $"Product {i}",
                Sku = $"PRODUCT{i}",
                ShortDescription = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                Description =
                    "orem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
                Price = GetRandomPrice(),
                ImageUrl = $"/images/product-images/product-image{i + 1}.jpg",
                UrlSlug = $"product-{i}",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = sysAdmin.Id,
                ModifiedBy = sysAdmin.Id
            });
        }

        await _context.Products.AddRangeAsync(listProducts);
        await _context.SaveChangesAsync();
    }

    private decimal GetRandomPrice()
    {
        return new Random().Next(100, 1000);
    }
}