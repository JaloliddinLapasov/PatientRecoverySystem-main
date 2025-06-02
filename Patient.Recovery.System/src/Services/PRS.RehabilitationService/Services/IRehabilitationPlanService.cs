using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.RehabilitationPlanModels;

namespace PRS.RehabilitationService.Services
{
    public interface IRehabilitationPlanService
    {
        Task<IEnumerable<RehabilitationPlan>> GetAllAsync();
        Task<RehabilitationPlan?> GetByIdAsync(int id);
        Task<IEnumerable<RehabilitationPlan>> GetByPatientIdAsync(int patientId);
        Task<RehabilitationPlan> AddAsync(RehabilitationPlan plan);
        Task<bool> UpdateAsync(int id, RehabilitationPlan plan);
        Task<bool> DeleteAsync(int id);
    }
}