using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using PRS.PatientService.Grpc;

namespace PRS.PatientService.Services
{
    public class GrpcPatientService : PatientGrpc.PatientGrpcBase
    {
        private readonly IPatientService _patientService;

        public GrpcPatientService(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public override async Task<PatientExistsResponse> CheckPatientExists(PatientRequest request, ServerCallContext context)
        {
            var exists = await _patientService.CheckPatientExists(request.PatientId);
            return new PatientExistsResponse { Exists = exists };
        }
    }
}