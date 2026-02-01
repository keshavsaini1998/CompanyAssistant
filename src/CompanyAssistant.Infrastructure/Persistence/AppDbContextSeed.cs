// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CompanyAssistant.Domain.Entities;
using CompanyAssistant.Infrastructure.Constants;
using CompanyAssistant.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;


namespace CompanyAssistant.Infrastructure.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedDefaultAsync(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (!context.Tenants.Any())
        {
            Tenant tenant = new Tenant() { Id = Guid.NewGuid(), Name = "Codinova Technologies" };
            context.Tenants.Add(tenant);
            await context.SaveChangesAsync();


            var roles = new[] { RoleConstants.Administrator, RoleConstants.Manager };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new AppRole(role));
            }


            var administrator = new AppUser { UserName = "administrator", TenantId = tenant.Id, TenantName = tenant.Name, Email = "admin@company.com", DisplayName = "Keshav", EmailConfirmed = true };
            var demo = new AppUser { UserName = "Manager", TenantId = tenant.Id, TenantName = tenant.Name, Email = "manager@company.com", DisplayName="KK", EmailConfirmed = true };


            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, RoleConstants.DefaultPassword);
                await userManager.AddToRolesAsync(administrator, new[] { RoleConstants.Administrator });
                await userManager.CreateAsync(demo, RoleConstants.DefaultPassword);
                await userManager.AddToRolesAsync(demo, new[] { RoleConstants.Manager });
            }


            if (!context.Projects.Any())
            {
                context.Projects.Add(new Project() { Id = Guid.NewGuid(), Name = "ZenegyTime", TenantId = tenant.Id });
                context.Projects.Add(new Project() { Id = Guid.NewGuid(), Name = "SYU", TenantId = tenant.Id });
                await context.SaveChangesAsync();
            }
        }
    }
}
