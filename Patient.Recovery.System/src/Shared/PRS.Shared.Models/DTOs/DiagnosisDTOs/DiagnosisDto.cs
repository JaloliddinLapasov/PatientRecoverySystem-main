using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.DTOs.DiagnosisDTOs
{
    public class DiagnosisDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Symptoms { get; set; } = string.Empty;
        public string DiagnosisName { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public DateTime DiagnosisDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
    }
}