using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRS.Shared.Infrastructure.Data;
using PRS.Shared.Models.Common;
using PRS.Shared.Models.MonitoringModels;

namespace PRS.MonitoringService.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly MonitoringDbContext _context;
        private readonly ILogger<MonitoringService> _logger;
        public MonitoringService(MonitoringDbContext context, ILogger<MonitoringService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CheckForAlarmsAsync(int patientId, CancellationToken ct)
        {
            var latestRecord = await _context.MonitoringRecords
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.RecordedAt)
                .FirstOrDefaultAsync(ct);

            if (latestRecord == null)
                return false;

            bool hasAlarm = false;
            var alarmReasons = new List<string>();

            // Check temperature
            if (latestRecord.Temperature.HasValue)
            {
                if (latestRecord.Temperature > 38.5 || latestRecord.Temperature < 35.0)
                {
                    hasAlarm = true;
                    alarmReasons.Add($"Temperature: {latestRecord.Temperature}°C");
                }
            }

            // Check blood pressure
            if (latestRecord.BloodPressureSystolic.HasValue && latestRecord.BloodPressureDiastolic.HasValue)
            {
                if (latestRecord.BloodPressureSystolic > 180 || latestRecord.BloodPressureSystolic < 90 ||
                    latestRecord.BloodPressureDiastolic > 110 || latestRecord.BloodPressureDiastolic < 60)
                {
                    hasAlarm = true;
                    alarmReasons.Add($"Blood Pressure: {latestRecord.BloodPressureSystolic}/{latestRecord.BloodPressureDiastolic}");
                }
            }

            // Check heart rate
            if (latestRecord.HeartRate.HasValue)
            {
                if (latestRecord.HeartRate > 120 || latestRecord.HeartRate < 50)
                {
                    hasAlarm = true;
                    alarmReasons.Add($"Heart Rate: {latestRecord.HeartRate} bpm");
                }
            }

            if (hasAlarm)
            {
                _logger.LogWarning("Health alarm detected for patient {PatientId}: {Reasons}",
                    patientId, string.Join(", ", alarmReasons));

                // Here you would typically send notifications or alerts
                // For now, just log the alarm
            }

            return hasAlarm;
        }

        public async Task<MonitoringRecord> CreateMonitoringRecordAsync(MonitoringRecord monitoringRecord, CancellationToken ct)
        {
            _context.MonitoringRecords.Add(monitoringRecord);
            await _context.SaveChangesAsync();

            // Check for alarms after creating the record
            var result = await CheckForAlarmsAsync(monitoringRecord.PatientId, ct);

            return monitoringRecord;
        }

        public async Task<MonitoringRecord?> GetMonitoringRecordByIdAsync(int id, CancellationToken ct)
        {
            var record = await _context.MonitoringRecords.FirstOrDefaultAsync(m => m.Id == id);

            if (record == null)
            {
                return null;
            }

            return record;
        }

        public async Task<IEnumerable<MonitoringRecord>> GetMonitoringRecordsAsync(CancellationToken ct)
        {
            var recentRecords = await _context.MonitoringRecords.ToListAsync(ct);

            return recentRecords;
        }

        public async Task<IEnumerable<MonitoringRecord>> GetPatientMonitoringRecordsAsync(int patientId, CancellationToken ct)
        {
            var records = await _context.MonitoringRecords
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.RecordedAt)
                .ToListAsync(ct);

            return records;
        }

        public async Task<IEnumerable<MonitoringRecord>> GetRecentRecordsAsync(int patientId, int hours = 24, CancellationToken ct = default)
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-hours);

            var records = await _context.MonitoringRecords
                .Where(m => m.PatientId == patientId && m.RecordedAt >= cutoffTime)
                .OrderByDescending(m => m.RecordedAt)
                .ToListAsync(ct);

            return records;
        }
    }
}