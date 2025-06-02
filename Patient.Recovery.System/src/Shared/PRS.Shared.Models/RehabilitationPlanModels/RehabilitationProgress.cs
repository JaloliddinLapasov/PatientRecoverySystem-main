using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.RehabilitationPlanModels
{
    public class RehabilitationProgress
    {
        public int Id { get; set; }
        public int RehabilitationPlanId { get; set; }

        [StringLength(200)]
        public string Activity { get; set; } = string.Empty;

        public int Duration { get; set; } // in minutes

        [StringLength(1000)]
        public string Notes { get; set; } = string.Empty;

        [Range(1, 10)]
        public int PainLevel { get; set; } = 1;

        [Range(1, 10)]
        public int DifficultyLevel { get; set; } = 1;

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual RehabilitationPlan RehabilitationPlan { get; set; } = null!;
    }
}