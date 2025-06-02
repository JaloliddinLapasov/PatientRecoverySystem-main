using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.DiagnosisModels;

namespace PRS.Shared.Models.DTOs.DiagnosisDTOs
{
    public class DiagnosisUpdateRequest
    {
        [StringLength(1000)]
        public string Symptoms { get; set; } = string.Empty;
        [StringLength(500)]
        public string DiagnosisName { get; set; } = string.Empty;
        [StringLength(1000)]
        public string Treatment { get; set; } = string.Empty;
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;
        [StringLength(50)]
        public string Severity { get; set; } = string.Empty;
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        [StringLength(100)]
        public string DoctorName { get; set; } = string.Empty;
    }
}