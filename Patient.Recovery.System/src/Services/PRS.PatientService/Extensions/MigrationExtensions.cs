using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PRS.PatientService.Extensions
{
    public static class MigrationExtensions
    {
        // public static void ApplyMigration<TDbContext>(IServiceScope scope)
        //     where TDbContext : DbContext
        // {
        //     using TDbContext context = scope.ServiceProvider
        //         .GetRequiredService<TDbContext>();

        //     context.Database.Migrate();
        // }
    }
}