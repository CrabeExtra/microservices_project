Start-Process "dotnet" "run --project ./Identity/Identity.csproj"
Start-Process "dotnet" "run --project ./Activity/Activity.csproj"
Start-Process "./nats-server.exe"