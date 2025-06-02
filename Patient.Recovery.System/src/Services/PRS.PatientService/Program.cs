using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using PRS.PatientService.Services;
using PRS.Shared.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PatientDbContext>(options =>
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

builder.Services.AddScoped<IPatientService, PatientService>();

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

builder.Services.AddGrpc();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using IServiceScope scope = app.Services.CreateScope();

    EnsureDatabaseUpToDate<PatientDbContext>(scope);
}

app.MapGrpcService<GrpcPatientService>();
app.UseHttpsRedirection();
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