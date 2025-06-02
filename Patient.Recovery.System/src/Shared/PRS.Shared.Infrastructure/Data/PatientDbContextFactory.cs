using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PRS.Shared.Infrastructure.Data
{
    public class PatientDbContextFactory : IDesignTimeDbContextFactory<PatientDbContext>
    {
        public PatientDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PatientDbContext>();

            // Replace with your actual connection string
            optionsBuilder.UseSqlServer("Server=localhost;Database=PRS_Patient_Design;Trusted_Connection=true;");

            return new PatientDbContext(optionsBuilder.Options);
        }
    }
}