using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.DTOs.PatientDTOs;
using PRS.Shared.Models.PatientModels;

namespace PRS.Shared.Models.Mappers
{
    public static class PatientMappers
    {
        public static Patient ToPatientFromCreatePatientDto(this CreatePatientDto patient)
        {
            return new Patient
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                Email = patient.Email,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth
            };
        }

        public static Patient ToPatientFromUpdatePatientDto(this UpdatePatientDto patient)
        {
            return new Patient
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                Email = patient.Email,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth
            };
        }
    }
}