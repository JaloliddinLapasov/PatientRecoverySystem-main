using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRS.Shared.Models.RehabilitationPlanModels;

namespace PRS.Shared.Infrastructure.Data
{
    public class RehabilitationDbContext : DbContext
    {
        public RehabilitationDbContext(DbContextOptions<RehabilitationDbContext> options) : base(options) { }

        public DbSet<RehabilitationPlan> RehabilitationPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}