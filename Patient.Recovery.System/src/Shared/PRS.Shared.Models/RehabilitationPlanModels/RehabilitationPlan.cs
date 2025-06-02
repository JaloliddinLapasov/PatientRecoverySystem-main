using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.PatientModels;

namespace PRS.Shared.Models.RehabilitationPlanModels
{
    public class RehabilitationPlan
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        [StringLength(200)]
        public string PlanName { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Range(0, 100)]
        public int Progress { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;
        [StringLength(100)]
        public string TherapistName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }
}