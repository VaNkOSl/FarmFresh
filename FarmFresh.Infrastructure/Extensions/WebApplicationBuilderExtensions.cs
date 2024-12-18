using FarmFresh.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static FarmFresh.Commons.GeneralApplicationConstants;

namespace FarmFresh.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
    {
        IServiceScope serviceScope = app.ApplicationServices.CreateScope();

        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        UserManager<ApplicationUser> userManager =
            serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        RoleManager<IdentityRole<Guid>> roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

        string defaultPassword = configuration["AdminSettings:DefaultAdminPassword"] ?? "DefaultFallbackPassword123";

        Task.Run(async () =>
        {
            if (await roleManager.RoleExistsAsync(AdminRoleName))
            {
                return;
            }

            IdentityRole<Guid> role = new IdentityRole<Guid>(AdminRoleName);
            await roleManager.CreateAsync(role);

            ApplicationUser? adminUser = await userManager.FindByEmailAsync(email);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "Admin",
                    LastName = "Admin",
                    IsAdmin = true
                };

                await userManager.CreateAsync(adminUser, defaultPassword);
            }

            await userManager.AddToRoleAsync(adminUser, AdminRoleName);
        })
        .GetAwaiter()
        .GetResult();

        return app;
    }
}
