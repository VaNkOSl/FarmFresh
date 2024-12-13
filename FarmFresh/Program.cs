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
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<IAddressService, AddressService>();

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
    await DBTransactionHelper.ExecuteTransactionAsync(scope.ServiceProvider, async () =>
    {
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        await countryService.UpdateCountriesAsync();
    });
    await DBTransactionHelper.ExecuteTransactionAsync(scope.ServiceProvider, async () =>
    {
        var cityService = scope.ServiceProvider.GetRequiredService<ICityService>();
        await cityService.UpdateCitiesAsync();
    });
    await DBTransactionHelper.ExecuteTransactionAsync(scope.ServiceProvider, async () =>
    {
        var officeService = scope.ServiceProvider.GetRequiredService<IOfficeService>();
        await officeService.UpdateOfficesAsync();
    });
}

app.Run();