using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.DiagnosisModels;

namespace PRS.DiagnoosisService.Services
{
    public interface IDiagnosisService
    {
        Task<IEnumerable<Diagnosis>> GetAllDiagnosesAsync();
        Task<Diagnosis?> GetDiagnosisByIdAsync(int id);
        Task<IEnumerable<Diagnosis>> GetDiagnosesByPatientIdAsync(int patientId);
        Task<Diagnosis> CreateDiagnosisAsync(Diagnosis diagnosis);
        Task<Diagnosis> UpdateDiagnosisAsync(int id, Diagnosis diagnosis);
        Task<bool> DeleteDiagnosisAsync(int id);
    }
}