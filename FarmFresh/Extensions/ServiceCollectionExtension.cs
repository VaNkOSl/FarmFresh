﻿using FarmFresh.Controllers;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.DataValidator;
using LoggerService;
using LoggerService.Contacts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FarmFresh.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<FarmFreshDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddControllersWithViews()
       .AddApplicationPart(typeof(HomeController).Assembly);

        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
    {
        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<FarmFreshDbContext>();

        return services;
    }

    public static IServiceCollection ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static IServiceCollection ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager,ServiceManager>();

    public static IServiceCollection ConfigureValidator(this IServiceCollection services) =>
        services.AddScoped<IValidateEntity,CRUDDataValidator>();

    public static IServiceCollection ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static IServiceCollection ConfigureAccountService(this IServiceCollection services) =>
        services.AddScoped<IAccountService, AccountService>();

    public static IServiceCollection ConfigureCookieAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Account/Login";
                        options.LogoutPath = "/Account/Logout";
                        options.ExpireTimeSpan = TimeSpan.FromDays(30);
                        options.SlidingExpiration = true;
                    });

        return services;
    }         
}
