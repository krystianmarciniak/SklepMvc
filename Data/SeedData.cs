using Microsoft.AspNetCore.Identity;

namespace SklepMvc.Data;

public static class SeedData
{
  public static async Task EnsureRolesAndAdminAsync(IServiceProvider services)
  {
    using var scope = services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = ["Admin", "User"];

    foreach (var role in roles)
    {
      if (!await roleManager.RoleExistsAsync(role))
        await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@sklep.local";
    var adminPassword = "Admin123!";

    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
      admin = new IdentityUser
      {
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true
      };

      var create = await userManager.CreateAsync(admin, adminPassword);
      if (create.Succeeded)
      {
        await userManager.AddToRoleAsync(admin, "Admin");
      }
    }
    else
    {
      if (!await userManager.IsInRoleAsync(admin, "Admin"))
        await userManager.AddToRoleAsync(admin, "Admin");
    }
  }
}
