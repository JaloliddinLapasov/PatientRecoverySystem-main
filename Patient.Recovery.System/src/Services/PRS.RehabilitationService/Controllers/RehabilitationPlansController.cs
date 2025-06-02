using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRS.PatientService.Grpc;
using PRS.RehabilitationService.Services;
using PRS.Shared.Models.DTOs.RehabilitationPlanDTOs;
using PRS.Shared.Models.Mappers;
using PRS.Shared.Models.RehabilitationPlanModels;
using static PRS.PatientService.Grpc.PatientGrpc;

namespace PRS.RehabilitationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RehabilitationPlansController : ControllerBase
    {
        private readonly IRehabilitationPlanService _service;
        private readonly PatientGrpcClient _grpcClient;

        public RehabilitationPlansController(IRehabilitationPlanService service, PatientGrpcClient grpcClient)
        {
            _service = service;
            _grpcClient = grpcClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRehabilitationPlans()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRehabilitationPlanById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plan = await _service.GetByIdAsync(id);
            return plan == null ? NotFound() : Ok(plan);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetRehabilitationPlansByPatientId([FromRoute] int patientId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _grpcClient.CheckPatientExists(new PatientRequest { PatientId = patientId });
            if (!response.Exists) return NotFound($"Patient with given ID: {patientId} NOT FOUND");

            var rbPlan = await _service.GetByPatientIdAsync(patientId);

            if (rbPlan is null) return NotFound();

            return Ok(rbPlan);
        }

        [HttpPost("{patientId}")]
        public async Task<IActionResult> CreateRehabilitationPlan([FromRoute] int patientId, [FromBody] RehabilitationPlanDto planDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _grpcClient.CheckPatientExists(new PatientRequest { PatientId = patientId });
            if (!response.Exists) return NotFound($"Patient with given ID: {patientId} NOT FOUND");

            var planModel = planDto.ToRehabilitationPlanFromRehabilitationPlanDto(patientId);

            return Ok(await _service.AddAsync(planModel));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRehabilitationPlan([FromRoute] int id, [FromBody] RehabilitationPlanDto planDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var planModel = planDto.ToRehabilitationPlanFromUpdateRehabilitationPlanDto();

            var success = await _service.UpdateAsync(id, planModel);

            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRehabilitationPlan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}