using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.PatientModels;

namespace PRS.PatientService.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync(CancellationToken ct);
        Task<Patient?> GetPatientByIdAsync(int id, CancellationToken ct);
        Task<Patient> CreatePatientAsync(Patient patient, CancellationToken ct);
        Task<Patient?> UpdatePatientAsync(int id, Patient patient, CancellationToken ct);
        Task<bool> DeletePatientAsync(int id, CancellationToken ct);
        Task<bool> CheckPatientExists(int patientId);
    }
}