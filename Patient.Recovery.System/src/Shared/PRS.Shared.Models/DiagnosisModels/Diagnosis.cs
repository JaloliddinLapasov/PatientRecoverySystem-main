using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.PatientModels;

namespace PRS.Shared.Models.DiagnosisModels
{
    public class Diagnosis
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        [StringLength(1000)]
        public string Symptoms { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string DiagnosisName { get; set; } = string.Empty;
        [Required]
        [StringLength(1000)]
        public string Treatment { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Severity { get; set; } = string.Empty;
        [Required]
        public DateTime DiagnosisDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        [StringLength(100)]
        public string DoctorName { get; set; } = string.Empty;
    }
}