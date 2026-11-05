[CmdletBinding()]
param(
    [string]$ApiRoot = $(if ($env:PERSONAL_SKILLS_API_ROOT) {
        $env:PERSONAL_SKILLS_API_ROOT
    } else {
        "http://localhost:5004/api/personal/skills"
    }),
    [string]$JsonPath = (Join-Path $PSScriptRoot "skills_test.json")
)

$ErrorActionPreference = "Stop"

$AdminKey = $env:PERSONAL_API_ADMIN_KEY
if ([string]::IsNullOrWhiteSpace($AdminKey)) {
    throw "Set the PERSONAL_API_ADMIN_KEY environment variable."
}
if (-not (Test-Path -LiteralPath $JsonPath)) {
    throw "JSON file not found: $JsonPath"
}

$ApiRoot = $ApiRoot.TrimEnd("/")
$SeedData = Get-Content -LiteralPath $JsonPath -Raw -Encoding UTF8 | ConvertFrom-Json

function Invoke-AdminPost {
    param(
        [string]$Endpoint = "",
        [Parameter(Mandatory = $true)][object]$Data
    )

    $Request = @{ Key = $AdminKey; Data = $Data }
    $Body = $Request | ConvertTo-Json -Depth 20 -Compress
    $Uri = if ([string]::IsNullOrEmpty($Endpoint)) { $ApiRoot } else { "$ApiRoot/$Endpoint" }

    try {
        Invoke-RestMethod -Uri $Uri -Method Post `
            -ContentType "application/json; charset=utf-8" `
            -Body ([Text.Encoding]::UTF8.GetBytes($Body))
    } catch {
        $resp = $_.Exception.Response
        if ($resp) {
            $reader = New-Object System.IO.StreamReader($resp.GetResponseStream())
            $errBody = $reader.ReadToEnd()
            Write-Host "--- $Uri ---" -ForegroundColor Yellow
            Write-Host "Status: $([int]$resp.StatusCode)" -ForegroundColor Yellow
            Write-Host "Body:   $errBody" -ForegroundColor Yellow
        }
        throw
    }
}

Write-Host "Creating $($SeedData.skills.Count) skills..." -ForegroundColor Cyan

foreach ($Skill in $SeedData.skills) {
    Write-Host "  Skill: $($Skill.name)"

    Invoke-AdminPost -Endpoint "" -Data @{
        Name        = $Skill.name
        Type        = $Skill.type
        Description = $Skill.description
    }
}

$TotalAccomplishments = ($SeedData.skills | ForEach-Object { $_.accomplishments.Count } | Measure-Object -Sum).Sum
Write-Host "Creating $TotalAccomplishments accomplishments..." -ForegroundColor Cyan

foreach ($Skill in $SeedData.skills) {
    foreach ($Accomplishment in $Skill.accomplishments) {
        $Text = "$($Accomplishment.company): $($Accomplishment.text)"
        Write-Host "  Accomplishment: $($Accomplishment.company) -> $($Skill.name)"

        Invoke-AdminPost -Endpoint "accomplishment" -Data @{
            Text       = $Text
            SkillNames = @($Skill.name)
        }
    }
}

Write-Host "Creating synonyms..." -ForegroundColor Cyan

foreach ($Skill in $SeedData.skills) {
    foreach ($Synonym in $Skill.synonyms) {
        Write-Host "  Synonym: $Synonym -> $($Skill.name)"

        Invoke-AdminPost -Endpoint "synonym" -Data @{
            Name      = $Synonym
            SkillName = $Skill.name
        }
    }
}

Write-Host "Skill seeding completed successfully." -ForegroundColor Green