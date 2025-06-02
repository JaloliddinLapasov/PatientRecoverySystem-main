using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.PatientModels
{
    public class BasePatientModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}