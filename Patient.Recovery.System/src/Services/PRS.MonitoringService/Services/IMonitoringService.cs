using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PRS.Shared.Models.MonitoringModels;

namespace PRS.MonitoringService.Services
{
    public interface IMonitoringService
    {
        Task<IEnumerable<MonitoringRecord>> GetMonitoringRecordsAsync(CancellationToken ct);
        Task<IEnumerable<MonitoringRecord>> GetPatientMonitoringRecordsAsync(int patientId, CancellationToken ct);
        Task<MonitoringRecord?> GetMonitoringRecordByIdAsync(int id, CancellationToken ct);
        Task<MonitoringRecord> CreateMonitoringRecordAsync(MonitoringRecord monitoringRecord, CancellationToken ct);
        Task<IEnumerable<MonitoringRecord>> GetRecentRecordsAsync(int patientId, int hours = 24, CancellationToken ct = default);
        Task<bool> CheckForAlarmsAsync(int patientId, CancellationToken ct);
    }
}