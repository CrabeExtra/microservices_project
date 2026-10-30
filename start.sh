#!/bin/bash

set -e

echo "Building .NET projects..."

dotnet build ./Identity/Identity.csproj
dotnet build ./Activity/Activity.csproj
dotnet build ./Audit/Audit.csproj
dotnet build ./Contact/Contact.csproj

echo "Building frontend..."

cd ./Frontend
npm install
npm run build
cd ..

echo "Starting services..."

dotnet run --no-build --project ./Identity/Identity.csproj &
dotnet run --no-build --project ./Activity/Activity.csproj &
dotnet run --no-build --project ./Audit/Audit.csproj &
dotnet run --no-build --project ./Contact/Contact.csproj &
npm --prefix ./Frontend/ run start &
./nats-server &

echo "All services started."

wait