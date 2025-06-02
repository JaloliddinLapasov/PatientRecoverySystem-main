using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PRS.Shared.Models.MonitoringModels;

namespace PRS.Shared.Infrastructure.Data
{
    public class MonitoringDbContext : DbContext
    {
        public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options) : base(options)
        {
           
        }

        public DbSet<MonitoringRecord> MonitoringRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonitoringRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Symptoms).HasMaxLength(1000);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Location).HasMaxLength(50);
                entity.Property(e => e.RecordedBy).HasMaxLength(50);
                entity.Property(e => e.RecordedAt).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}