using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.DTOs.DiagnosisDTOs
{
    public class SymptomReportDto
    {
        public int SymptomId { get; set; }
        public int Severity { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}