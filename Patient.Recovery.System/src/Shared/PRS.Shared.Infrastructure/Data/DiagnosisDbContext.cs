using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRS.Shared.Models;
using PRS.Shared.Models.DiagnosisModels;
using PRS.Shared.Models.MedicalHistoryModels;
using PRS.Shared.Models.PatientModels;

namespace PRS.Shared.Infrastructure.Data
{
    public class DiagnosisDbContext : DbContext
    {
        public DiagnosisDbContext(DbContextOptions<DiagnosisDbContext> options) : base(options) { }

        public DbSet<Diagnosis> Diagnoses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id).ValueGeneratedOnAdd();

                entity.Property(d => d.Symptoms).IsRequired().HasMaxLength(1000);
                entity.Property(d => d.DiagnosisName).IsRequired().HasMaxLength(500);
                entity.Property(d => d.Treatment).IsRequired().HasMaxLength(1000);
                entity.Property(d => d.Status).IsRequired().HasMaxLength(50);
                entity.Property(d => d.Severity).IsRequired().HasMaxLength(50);
                entity.Property(d => d.DiagnosisDate).IsRequired();
                entity.Property(d => d.CreatedAt).IsRequired();
                entity.Property(d => d.UpdatedAt).IsRequired();

                entity.Property(d => d.Notes).HasMaxLength(500);
                entity.Property(d => d.DoctorName).HasMaxLength(100);

                entity.HasIndex(d => d.PatientId);
                entity.HasIndex(d => d.DiagnosisDate);
                entity.HasIndex(d => d.Status);
            });
        }
    }
}