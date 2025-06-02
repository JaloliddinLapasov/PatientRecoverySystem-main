using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.DiagnosisModels;
using PRS.Shared.Models.DTOs.DiagnosisDTOs;

namespace PRS.Shared.Models.Mappers
{
    public static class DiagnosisMappers
    {
        public static Diagnosis ToDiagnosisFromCreateDiagnosisRequest(this CreateDiagnosisRequest create, int patientId)
        {
            return new Diagnosis
            {
                PatientId = patientId,
                DoctorName = create.DoctorName,
                Symptoms = create.Symptoms,
                Notes = create.Notes,
                CreatedAt = DateTime.UtcNow,
                Severity = create.Severity,
                Treatment = create.Treatment,
                DiagnosisDate = DateTime.Today,
                DiagnosisName = create.DiagnosisName,
                Status = create.Status
            };
        }

        public static Diagnosis ToDiagnosisFromDiagnosisUpdateRequest(this DiagnosisUpdateRequest request, int patientId)
        {
            return new Diagnosis
            {
                PatientId = patientId,
                Notes = request.Notes,
                DoctorName = request.DoctorName,
                DiagnosisDate = DateTime.UtcNow,
                DiagnosisName = request.DiagnosisName,
                Severity = request.Severity,
                Treatment = request.Treatment,
                Symptoms = request.Symptoms,
                Status = request.Status
            };
        }
    }
}