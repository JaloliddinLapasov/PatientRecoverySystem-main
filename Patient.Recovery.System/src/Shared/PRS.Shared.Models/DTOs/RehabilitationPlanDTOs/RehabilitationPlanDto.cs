using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.DTOs.RehabilitationPlanDTOs
{
    public class RehabilitationPlanDto
    {
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string TherapistName { get; set; } = string.Empty;
    }
}