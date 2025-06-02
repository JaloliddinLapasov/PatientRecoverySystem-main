using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using PRS.Shared.Models.EventHandlers;

namespace PRS.NotificationService.Services
{
    public class MonitoringUpdatedEventHandler : IConsumer<MonitoringUpdatedEvent>
    {
        private readonly INotificationService _notificationService;

        public MonitoringUpdatedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<MonitoringUpdatedEvent> context)
        {
            var message = $"Patient {context.Message.PatientId} has new monitoring data.";
            await _notificationService.Send(message, "sherzod@gmail.com");
        }
    }
}