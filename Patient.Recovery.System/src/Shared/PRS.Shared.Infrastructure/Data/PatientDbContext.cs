using Microsoft.EntityFrameworkCore;
using PRS.Shared.Models.MedicalHistoryModels;
using PRS.Shared.Models.PatientModels;

namespace PRS.Shared.Infrastructure.Data
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Gender).HasMaxLength(10);
            });

            // Medical History configuration
            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SurgeryType).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Surgeon).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Complications).HasMaxLength(500);
                entity.Property(e => e.Medications).HasMaxLength(1000);

                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.MedicalHistories)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}