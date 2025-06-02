using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRS.Shared.Infrastructure.Data;
using PRS.Shared.Models.PatientModels;

namespace PRS.PatientService.Services
{
    public class PatientService : IPatientService
    {
        private readonly PatientDbContext _context;

        public PatientService(PatientDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> CreatePatientAsync(Patient patient, CancellationToken ct)
        {
            _context.Add(patient);
            await _context.SaveChangesAsync(ct);

            return patient;
        }

        public async Task<bool> DeletePatientAsync(int id, CancellationToken ct)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync(CancellationToken ct)
        {

            var patients = await _context.Patients.ToListAsync(ct);

            return patients;
        }

        public async Task<Patient?> GetPatientByIdAsync(int id, CancellationToken ct)
        {
            var patient = await _context.Patients.FindAsync(id, ct);

            if (patient == null)
                return null;

            return patient;
        }

        public async Task<Patient?> UpdatePatientAsync(int id, Patient patient, CancellationToken ct)
        {
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);

            if (existingPatient == null)
                return null;

            existingPatient.Address = patient.Address;
            existingPatient.Email = patient.Email;
            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.Gender = patient.LastName;
            existingPatient.MedicalHistories = patient.MedicalHistories;
            existingPatient.PhoneNumber = patient.PhoneNumber;

            await _context.SaveChangesAsync();

            return existingPatient;
        }

        public async Task<bool> CheckPatientExists(int patientId)
        {
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(patient => patient.Id == patientId);

            if (existingPatient is null) return false;

            return true;
        }
    }
}