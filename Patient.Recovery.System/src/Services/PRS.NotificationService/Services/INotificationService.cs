using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace PRS.NotificationService.Services
{
    public interface INotificationService
    {
        Task Send(string message, string recipientEmail);
    }
}