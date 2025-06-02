using MassTransit;
using Microsoft.EntityFrameworkCore;
using PRS.NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the event handler (consumer)
// builder.Services.AddMassTransit(x =>
// {
//     x.AddConsumer<MonitoringUpdatedEventHandler>();

//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host("5672", h =>
//         {
//             h.Username("guest");
//             h.Password("guest");
//         });

//         cfg.ReceiveEndpoint("monitoring-updated-queue", e =>
//         {
//             e.ConfigureConsumer<MonitoringUpdatedEventHandler>(context);
//         });
//     });
// });

// Register your custom services (like INotificationService)
// builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();