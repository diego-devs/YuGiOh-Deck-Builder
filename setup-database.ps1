# Creates the YgoDB database on the local SQL Server (reads connection string from YugiohDB/appsettings.json).
# Usage (from repo root):   powershell -ExecutionPolicy Bypass -File .\setup-database.ps1

$ErrorActionPreference = 'Stop'

$projectPath = Join-Path $PSScriptRoot 'YugiohDB\YugiohDB.csproj'
if (-not (Test-Path $projectPath)) {
    throw "Could not find YugiohDB project at: $projectPath"
}

$toolsPath = Join-Path $env:USERPROFILE '.dotnet\tools'
if ($env:PATH -notlike "*$toolsPath*") {
    $env:PATH = "$toolsPath;$env:PATH"
}

Write-Host 'Checking for dotnet-ef global tool...'
$efInstalled = $false
try {
    dotnet ef --version *> $null
    if ($LASTEXITCODE -eq 0) { $efInstalled = $true }
} catch { }

if (-not $efInstalled) {
    Write-Host 'Installing dotnet-ef as a global tool...'
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -ne 0) {
        throw 'Failed to install dotnet-ef.'
    }
}

Write-Host 'Applying EF Core migrations to create YgoDB...'
dotnet ef database update --project $projectPath
if ($LASTEXITCODE -ne 0) {
    throw 'dotnet ef database update failed. Check your connection string in YugiohDB/appsettings.json.'
}

Write-Host ''
Write-Host 'YgoDB database is ready.' -ForegroundColor Green
