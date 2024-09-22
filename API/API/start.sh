#!/bin/bash

# Wait for SQL Server to be ready (if necessary)
echo "Waiting for SQL Server to be ready..."
sleep 30 # Adjust this as necessary

# Run migrations
echo "Running application migrations..."
dotnet ef database update --context ApplicationDbContext
echo "Running tenant migrations..."
dotnet ef database update --context TenantDbContext

# Start the application
echo "Starting application..."
exec dotnet YourApplication.dll