#!/bin/bash
echo "Applying EF Core migrations..."

# Run migration on the correct project and context
dotnet ef database update --project ../Shared/PRS.Infrastructure --startup-project . --context PatientDbContext

echo "Starting the application..."
exec dotnet PRS.PatientService.dll
