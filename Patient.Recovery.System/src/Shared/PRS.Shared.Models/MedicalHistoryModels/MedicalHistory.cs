using System;
using System.ComponentModel.DataAnnotations;
using PRS.Shared.Models.PatientModels;

namespace PRS.Shared.Models.MedicalHistoryModels
{
    public class MedicalHistory
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string SurgeryType { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Surgeon { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Complications { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Medications { get; set; } = string.Empty;
        // Foreign key
        public int PatientId { get; set; }
        // Navigation property
        public Patient? Patient { get; set; }
    }
}