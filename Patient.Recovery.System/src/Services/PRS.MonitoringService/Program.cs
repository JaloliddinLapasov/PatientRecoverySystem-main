using MassTransit;
using Microsoft.EntityFrameworkCore;
using PRS.MonitoringService.Services;
using PRS.Shared.Infrastructure.Data;
using static PRS.PatientService.Grpc.PatientGrpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MonitoringDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        })
);

builder.Services.AddScoped<IMonitoringService, MonitoringService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddGrpcClient<PatientGrpcClient>(o =>
{
    o.Address = new Uri("https://localhost:7013");
});

// builder.Services.AddMassTransit(x =>
// {
//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host("5672", h =>
//         {
//             h.Username("guest");
//             h.Password("guest");
//         });
//     });
// });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using IServiceScope scope = app.Services.CreateScope();

    EnsureDatabaseUpToDate<MonitoringDbContext>(scope);
}


// app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.MapControllers();

app.Run();

static void EnsureDatabaseUpToDate<TDbContext>(IServiceScope scope)
    where TDbContext : DbContext
{
    using TDbContext context = scope.ServiceProvider
        .GetRequiredService<TDbContext>();

    try
    {
        // Try to apply migrations first (if any exist)
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            context.Database.Migrate();
        }
        else if (!context.Database.CanConnect() || !context.Database.GetAppliedMigrations().Any())
        {
            // No migrations exist, create database from model
            context.Database.EnsureCreated();
        }
    }
    catch
    {
        // Fallback to EnsureCreated if migrations fail
        context.Database.EnsureCreated();
    }
}