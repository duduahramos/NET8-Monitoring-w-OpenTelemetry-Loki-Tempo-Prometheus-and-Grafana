using System.Diagnostics.Metrics;
using CentralizedLoggingAndTracingAPI.Services.ProductService;
using Core.Common;
using Core.Data;
using Core.Extensions;
using Core.Middlewares;
using Core.Services.Implementation;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddNamedHttpClient(configuration.GetValue<string>("ApiBaseAddress"), "MyApiClient2");

// Observability setup
services
    .AddOpenTelemetry()
    .AddObservability(builder);

// Add services to the container.
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

services.AddSingleton(provider => new WebMetrics(provider.GetRequiredService<IMeterFactory>(), builder.Environment.ApplicationName));

// adding a database service with configuration -- connection string read from appsettings.json
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Current tenant service with scoped lifetime (created per each request)
services.AddScoped<ICurrentTenantService, CurrentTenantService>();

// Product CRUD service with transient lifetime
services.AddTransient<IProductService, ProductService>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ApiTenantResolver>();

app.MapControllers();

app.Run();
