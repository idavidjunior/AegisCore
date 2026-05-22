param(
    [string]$Version = $null
)

# Script for versioning and pushing to GitHub
$ErrorActionPreference = "Stop"

function Get-NextVersion {
    $currentVersion = git describe --tags --abbrev=0 2>$null
    if ($LASTEXITCODE -ne 0) {
        return "1.0.0"
    }
    
    $parts = $currentVersion.Split('.')
    $parts[2] = [int]$parts[2] + 1
    return "$($parts[0]).$($parts[1]).$($parts[2])"
}

if ([string]::IsNullOrEmpty($Version)) {
    $Version = Get-NextVersion
}

Write-Host "Publishing version $Version" -ForegroundColor Green

# Update version in all csproj files
Get-ChildItem -Path src -Filter "*.csproj" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    if ($content -match '<Version>([^<]+)</Version>') {
        $content = $content -replace '<Version>[^<]+</Version>', "<Version>$Version</Version>"
        Set-Content $_.FullName $content -NoNewline
        Write-Host "Updated $($_.Name) to version $Version" -ForegroundColor Cyan
    }
}

# Build the solution
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build --configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Run tests
Write-Host "Running tests..." -ForegroundColor Yellow
dotnet test --configuration Release --no-build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed!" -ForegroundColor Red
    exit 1
}

# Commit and tag
Write-Host "Creating git tag..." -ForegroundColor Yellow
git add .
git commit -m "Release version $Version"
git tag "v$Version"

Write-Host "Pushing to GitHub..." -ForegroundColor Yellow
git push origin main
git push origin "v$Version"

Write-Host "Release $Version published successfully!" -ForegroundColor Green
