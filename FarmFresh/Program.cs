using FarmFresh.Data;
using FarmFresh.Extensions;
using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using FarmFresh.Services.Econt;
using FarmFresh.Services.Econt.APIServices;
using LoggerService.Contacts;
using NLog;

using static FarmFresh.Commons.GeneralApplicationConstants;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
                      "/nlog.config"));

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddHttpClient();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureValidator();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureAccountService();
builder.Services.ConfigureCookieAuthentication();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureServicesCORS();
builder.Services.AddScoped<IEcontNumenclaturesService, EcontNumenclaturesService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(AdminRoleName));
});

builder.Logging.ClearProviders();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseCors("AllowAll");

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
if (app.Environment.IsProduction())
    app.UseHsts();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if(app.Environment.IsDevelopment())
{
    app.SeedAdministrator(DevelopmentAdminEmail);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(config =>
{
    config.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
});

app.MapDefaultControllerRoute();
app.MapRazorPages();

//**TESTING PURPOSES**
//testing econt api calls
using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FarmFreshDbContext>();
    var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
    var cityService = scope.ServiceProvider.GetRequiredService<ICityService>();

    await DBTransactionHelper.ExecuteTransactionAsync(dbContext, async () =>
    {
        await countryService.UpdateCountriesAsync();
        await cityService.UpdateCitiesAsync();
    });
}

app.Run();