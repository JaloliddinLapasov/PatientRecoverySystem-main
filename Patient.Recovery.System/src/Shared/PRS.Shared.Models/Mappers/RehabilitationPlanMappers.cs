using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.DTOs.RehabilitationPlanDTOs;
using PRS.Shared.Models.PatientModels;
using PRS.Shared.Models.RehabilitationPlanModels;

namespace PRS.Shared.Models.Mappers
{
    public static class RehabilitationPlanMappers
    {
        public static RehabilitationPlan ToRehabilitationPlanFromRehabilitationPlanDto(this RehabilitationPlanDto rehabilitationPlanDto, int patientId)
        {
            return new RehabilitationPlan
            {
                PatientId = patientId,
                PlanName = rehabilitationPlanDto.PlanName,
                StartDate = rehabilitationPlanDto.StartDate,
                EndDate = rehabilitationPlanDto.EndDate,
                Progress = rehabilitationPlanDto.Progress,
                Status = rehabilitationPlanDto.Status,
                Notes = rehabilitationPlanDto.Notes,
                TherapistName = rehabilitationPlanDto.TherapistName
            };
        }

        public static RehabilitationPlan ToRehabilitationPlanFromUpdateRehabilitationPlanDto(this RehabilitationPlanDto rehabilitationPlanDto)
        {
            return new RehabilitationPlan
            {
                PlanName = rehabilitationPlanDto.PlanName,
                StartDate = rehabilitationPlanDto.StartDate,
                EndDate = rehabilitationPlanDto.EndDate,
                Progress = rehabilitationPlanDto.Progress,
                Status = rehabilitationPlanDto.Status,
                Notes = rehabilitationPlanDto.Notes,
                TherapistName = rehabilitationPlanDto.TherapistName
            };
        }
    }
}