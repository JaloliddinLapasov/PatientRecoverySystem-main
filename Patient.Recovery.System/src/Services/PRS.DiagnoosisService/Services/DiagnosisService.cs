using Microsoft.EntityFrameworkCore;
using PRS.Shared.Infrastructure.Data;
using PRS.Shared.Models.DiagnosisModels;

namespace PRS.DiagnoosisService.Services
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly DiagnosisDbContext _context;
        public DiagnosisService(DiagnosisDbContext context)
        {
            _context = context;
        }
        public async Task<Diagnosis> CreateDiagnosisAsync(Diagnosis diagnosis)
        {
            _context.Diagnoses.Add(diagnosis);
            await _context.SaveChangesAsync();

            return diagnosis;
        }
        public async Task<bool> DeleteDiagnosisAsync(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null) return false;

            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Diagnosis>> GetAllDiagnosesAsync()
        {
            return await _context.Diagnoses
                .ToListAsync();
        }
        public async Task<IEnumerable<Diagnosis>> GetDiagnosesByPatientIdAsync(int patientId)
        {
            return await _context.Diagnoses
                .Where(d => d.PatientId == patientId)
                .OrderByDescending(d => d.DiagnosisDate)
                .ToListAsync();
        }
        public async Task<Diagnosis?> GetDiagnosisByIdAsync(int id)
        {
            var diagnosis = await _context.Diagnoses
                .FirstOrDefaultAsync(d => d.Id == id);

            if (diagnosis is null)
                return null;

            return diagnosis;
        }
        public async Task<Diagnosis> UpdateDiagnosisAsync(int id, Diagnosis diagnosis)
        {
            _context.Entry(diagnosis).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return diagnosis;
        }
    }
}