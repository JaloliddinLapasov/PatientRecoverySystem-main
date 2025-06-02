using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.DTOs.DiagnosisDTOs
{
    public class EmergencyAlertResponse
    {
        public bool IsEmergency { get; set; }
        public string AlertLevel { get; set; } = string.Empty;
        public string RecommendedAction { get; set; } = string.Empty;
        public List<string> CriticalSymptoms { get; set; } = new();
    }
}