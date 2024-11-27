using FarmFresh.Extensions;
using FarmFresh.Infrastructure.Extensions;
using LoggerService.Contacts;
using NLog;
using static FarmFresh.Commons.GeneralApplicationConstants;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
                      "/nlog.config"));


builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureValidator();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureAccountService();
builder.Services.ConfigureCookieAuthentication();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Administrator")); 
});


builder.Logging.ClearProviders();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

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

app.Run();