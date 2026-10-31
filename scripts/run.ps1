# grabs current path, ensure this script is run from project root.
$dir = (Get-Location).Path

# Starts processes for nats, the frontend, and each microservice. each is run from its respective directory, this ensures restarting just one microservice is a one step process.
wt.exe `
    new-tab --title "Identity" -d "$dir/Identity" powershell.exe -NoExit -Command "dotnet run --project Identity.csproj" `
    `; new-tab --title "Activity" -d "$dir/Activity" powershell.exe -NoExit -Command "dotnet run --project Activity.csproj" `
    `; new-tab --title "Audit" -d "$dir/Audit" powershell.exe -NoExit -Command "dotnet run --project Audit.csproj" `
    `; new-tab --title "Contact" -d "$dir/Contact" powershell.exe -NoExit -Command "dotnet run --project Contact.csproj" `
    `; new-tab --title "Frontend" -d "$dir/Frontend" powershell.exe -NoExit -Command "npm run start" `
    `; new-tab --title "NATS" -d "$dir" powershell.exe -NoExit -Command "./nats-server.exe"