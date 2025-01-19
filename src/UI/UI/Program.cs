using System.Diagnostics.Metrics;
using Core.Common;
using Core.Data;
using Core.Extensions;
using Core.Middlewares;
using Core.Services.Implementation;
using Core.Services.Interfaces;
using Core.UOW;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Tracing and Metrics Setup
services
    .AddOpenTelemetry()
    .AddObservability(builder);


// Add services to the container.
services.AddNamedHttpClient(configuration.GetValue<string>("ApiBaseAddress"), "MyApiClient");

services.AddControllersWithViews();

services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton(provider => new WebMetrics(provider.GetRequiredService<IMeterFactory>(), builder.Environment.ApplicationName));

services.AddScoped<IUnitOfWork, UnitOfWork>();

services.AddTransient<UiTenantMiddleware>();

// adding a database service with configuration -- connection string read from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("localDb")));
//builder.Services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("localDb")));

// Current tenant service with scoped lifetime (created per each request)
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();

services.AddTransient<ITenantService>(provider =>
    new TenantService(provider.GetRequiredService<IUnitOfWork>()));

services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseMiddleware<UiTenantMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
