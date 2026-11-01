# Grabs current path; ensure this script is run from project root.
$dir = (Get-Location).Path

# Read .NET services from shared configuration.
$dotnetServices = Get-Content "$dir/scripts/dotnet-services-list.txt"

# Build Windows Terminal command.
$wtArgs = @()

foreach ($service in $dotnetServices) {
    $wtArgs += @(
        "new-tab"
        "--title"
        $service
        "-d"
        "$dir/$service"
        "powershell.exe"
        "-NoExit"
        "-Command"
        "dotnet run --project $service.csproj"
        ";"
    )
}

# Remove trailing separator.
$wtArgs = $wtArgs[0..($wtArgs.Count - 2)]

# Add frontend and NATS.
$wtArgs += @(
    ";"
    "new-tab"
    "--title"
    "Frontend"
    "-d"
    "$dir/Frontend"
    "powershell.exe"
    "-NoExit"
    "-Command"
    "npm run start"
    ";"
    "new-tab"
    "--title"
    "NATS"
    "-d"
    $dir
    "powershell.exe"
    "-NoExit"
    "-Command"
    "./nats-server.exe"
)

wt.exe @wtArgs