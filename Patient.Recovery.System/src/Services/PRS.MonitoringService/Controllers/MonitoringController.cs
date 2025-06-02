using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PRS.MonitoringService.Services;
using PRS.PatientService.Grpc;
using PRS.Shared.Models.DTOs.MonitoringDTOs;
using PRS.Shared.Models.EventHandlers;
using PRS.Shared.Models.Mappers;
using static PRS.PatientService.Grpc.PatientGrpc;

namespace PRS.MonitoringService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitoringController : ControllerBase
    {
        private readonly IMonitoringService _monitoringService;
        private readonly PatientGrpcClient _grpcClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public MonitoringController(IMonitoringService monitoringService, PatientGrpcClient grpcClient, IPublishEndpoint publishEndpoint)
        {
            _monitoringService = monitoringService;
            _grpcClient = grpcClient;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetMonitoringRecords(CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recentRecords = await _monitoringService.GetMonitoringRecordsAsync(ct);

            return Ok(recentRecords);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientMonitoringRecords(int patientId, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _monitoringService.GetPatientMonitoringRecordsAsync(patientId, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonitoringRecordById(int id, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _monitoringService.GetMonitoringRecordByIdAsync(id, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("patient/{patientId}/recent")]
        public async Task<IActionResult> GetRecentRecords(int patientId, [FromQuery] int hours = 24, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _monitoringService.GetRecentRecordsAsync(patientId, hours, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("{patientId}")]
        public async Task<IActionResult> CreateMonitoringRecord([FromRoute] int patientId, [FromBody] CreateMonitoringRecordDto createDto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _grpcClient.CheckPatientExistsAsync(new PatientRequest { PatientId = patientId });

            if (!response.Exists)
                return NotFound($"Patient NOT FOUND with given ID: {patientId}");

            var monitoringRecord = createDto.ToMonitoringRecordFromCreateMonitoringRecordDto(patientId);

            var result = await _monitoringService.CreateMonitoringRecordAsync(monitoringRecord, ct);

            await _publishEndpoint.Publish(new MonitoringUpdatedEvent(monitoringRecord.PatientId));

            return CreatedAtAction(nameof(GetMonitoringRecordById), new { id = result.Id }, result);
        }

        [HttpGet("patient/{patientId}/check-alarms")]
        public async Task<IActionResult> CheckForAlarms(int patientId, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _monitoringService.CheckForAlarmsAsync(patientId, ct);

            if (result)
                return NotFound();

            return Ok(result);
        }
    }
}