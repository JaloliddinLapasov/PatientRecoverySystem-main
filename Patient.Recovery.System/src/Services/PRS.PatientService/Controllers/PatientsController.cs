using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using PRS.PatientService.Services;
using PRS.Shared.Models.DTOs.PatientDTOs;
using PRS.Shared.Models.EventHandlers;
using PRS.Shared.Models.Mappers;
using RabbitMQ.Client;

namespace PRS.PatientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients(CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _patientService.GetAllPatientsAsync(ct);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _patientService.GetPatientByIdAsync(id, ct);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto createPatientDto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patientModel = createPatientDto.ToPatientFromCreatePatientDto();

            var result = await _patientService.CreatePatientAsync(patientModel, ct);

            return CreatedAtAction(nameof(GetPatientById), new { id = patientModel.Id }, patientModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDto updatePatientDto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patientModel = updatePatientDto.ToPatientFromUpdatePatientDto();

            var result = await _patientService.UpdatePatientAsync(id, patientModel, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id, CancellationToken ct)
        {
            var result = await _patientService.DeletePatientAsync(id, ct);

            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("exists/{id}")]
        public async Task<IActionResult> CheckPatientExists([FromRoute] int patientId)
        {
            var existPatient = await _patientService.CheckPatientExists(patientId);
            return existPatient ? Ok() : NotFound();
        }
    }
}