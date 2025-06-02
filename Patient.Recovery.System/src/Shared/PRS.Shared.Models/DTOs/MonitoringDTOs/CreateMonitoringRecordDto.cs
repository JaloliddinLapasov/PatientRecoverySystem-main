using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.DTOs.MonitoringDTOs
{
    public class CreateMonitoringRecordDto
    {
        public double? Temperature { get; set; }
        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public double? Weight { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string RecordedBy { get; set; } = string.Empty;
    }
}