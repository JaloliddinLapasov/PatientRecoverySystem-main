using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRS.Shared.Infrastructure.Data;
using PRS.Shared.Models.RehabilitationPlanModels;

namespace PRS.RehabilitationService.Services
{
    public class RehabilitationPlanService : IRehabilitationPlanService
    {
        private readonly RehabilitationDbContext _context;

        public RehabilitationPlanService(RehabilitationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RehabilitationPlan>> GetAllAsync()
        {
            return await _context.RehabilitationPlans.ToListAsync();
        }

        public async Task<RehabilitationPlan?> GetByIdAsync(int id)
        {
            return await _context.RehabilitationPlans.FindAsync(id);
        }

        public async Task<IEnumerable<RehabilitationPlan>> GetByPatientIdAsync(int patientId)
        {
            return await _context.RehabilitationPlans
                .Where(p => p.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<RehabilitationPlan> AddAsync(RehabilitationPlan plan)
        {
            plan.CreatedAt = DateTime.UtcNow;
            plan.UpdatedAt = DateTime.UtcNow;
            _context.RehabilitationPlans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> UpdateAsync(int id, RehabilitationPlan plan)
        {
            System.Console.WriteLine(plan.Id);
            var existing = await _context.RehabilitationPlans.FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null) return false;

            existing.PlanName = plan.PlanName;
            existing.StartDate = plan.StartDate;
            existing.EndDate = plan.EndDate;
            existing.Progress = plan.Progress;
            existing.Status = plan.Status;
            existing.TherapistName = plan.TherapistName;
            existing.Notes = plan.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.RehabilitationPlans.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.RehabilitationPlans.FindAsync(id);
            if (existing == null) return false;

            _context.RehabilitationPlans.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}