using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.EventHandlers
{
    public class MonitoringUpdatedEvent
    {
        public int PatientId { get; set; }

        public MonitoringUpdatedEvent(int patientId)
        {
            PatientId = patientId;
        }
    }
}