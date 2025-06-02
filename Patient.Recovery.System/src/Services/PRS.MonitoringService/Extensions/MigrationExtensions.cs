using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRS.Shared.Infrastructure.Data;
using PRS.Shared.Models.MedicalHistoryModels;
using PRS.Shared.Models.MonitoringModels;

namespace PRS.MonitoringService.Extensions
{
    public static class MigrationExtensions
    {
        // public static void ApplyMigrations(this IApplicationBuilder app)
        // {
        //     using IServiceScope scope = app.ApplicationServices.CreateScope();

        //     using MonitoringDbContext dbContext =
        //         scope.ServiceProvider.GetRequiredService<MonitoringDbContext>();

        //     try
        //     {
        //         dbContext.Database.EnsureCreated();
        //         Console.WriteLine("Migration successfully");
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine("Migration failed" + $"\n{e.Message}" + $"\n{e.InnerException}");
        //     }
        // }

        public static class DatabaseInitializer
        {
            public static async Task InitializeDatabasesAsync(IServiceProvider services)
            {
                using var scope = services.CreateScope();

                var monitoringContext = scope.ServiceProvider.GetRequiredService<MonitoringDbContext>();

                try
                {
                    // Apply any pending migrations
                    await monitoringContext.Database.MigrateAsync();

                    Console.WriteLine("Databases migrated successfully.");

                    // Seed data
                    SeedMonitoringData(monitoringContext);

                    Console.WriteLine("Database seeding completed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during database initialization: {ex.Message}");
                    throw;
                }
            }

            private static void SeedPatientData(PatientDbContext context)
            {
                var medicalHistories = new List<MedicalHistory>
                {
                    new MedicalHistory
                    {
                        SurgeryType = "Appendectomy",
                        Surgeon = "Dr. Michael Thompson",
                        Notes = "Routine laparoscopic appendectomy. Patient recovered well.",
                        Complications = "None",
                        Medications = "Post-op: Ibuprofen 400mg, Amoxicillin 500mg",
                        PatientId = 1
                    }
                };

                context.MedicalHistories.AddRange(medicalHistories);
                context.SaveChanges();
            }

            private static void SeedMonitoringData(MonitoringDbContext context)
            {
                if (context.MonitoringRecords.Any()) return;

                var monitoringRecords = new List<MonitoringRecord>
                {
                    new MonitoringRecord
                    {
                        Symptoms = "Mild pain at incision site",
                        Notes = "Patient reports pain level 3/10",
                        Location = "Room 201",
                        RecordedBy = "Nurse Johnson",
                        RecordedAt = DateTime.UtcNow.AddHours(-6)
                    }
                };

                context.MonitoringRecords.AddRange(monitoringRecords);
                context.SaveChanges();
            }
        }
    }
}