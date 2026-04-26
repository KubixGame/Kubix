param(
    [switch]$RunServer
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Write-Status {
    param([string]$Message)
    Write-Host "[unity-mcp-bootstrap] $Message"
}

function Remove-IfExists {
    param([string]$Path)
    if ([System.IO.Directory]::Exists($Path)) {
        Remove-Item -LiteralPath $Path -Recurse -Force
    } elseif ([System.IO.File]::Exists($Path)) {
        Remove-Item -LiteralPath $Path -Force
    }
}

function Get-CommandPath {
    param([string[]]$Names)
    foreach ($name in $Names) {
        $command = Get-Command $name -ErrorAction SilentlyContinue
        if ($null -ne $command) {
            return $command.Source
        }
    }

    return $null
}

function Install-FromGit {
    param(
        [string]$GitPath,
        [string]$CheckoutDir
    )

    Write-Status "Cloning HuntNight/unity-mcp-advanced via git..."
    & $GitPath clone --depth 1 "https://github.com/HuntNight/unity-mcp-advanced.git" $CheckoutDir
    if ($LASTEXITCODE -ne 0) {
        throw "git clone failed with exit code $LASTEXITCODE."
    }
}

function Install-FromZip {
    param(
        [string]$CheckoutDir
    )

    $zipUrl = "https://codeload.github.com/HuntNight/unity-mcp-advanced/zip/refs/heads/main"
    $tempRoot = [System.IO.Path]::GetTempPath()
    $zipPath = Join-Path $tempRoot "unity-mcp-advanced-main.zip"
    $extractDir = Join-Path $tempRoot ("unity-mcp-advanced-extract-" + [Guid]::NewGuid().ToString("N"))
    $expandedRoot = Join-Path $extractDir "unity-mcp-advanced-main"

    try {
        Write-Status "Downloading HuntNight/unity-mcp-advanced ZIP from GitHub..."
        Invoke-WebRequest -Uri $zipUrl -OutFile $zipPath -UseBasicParsing
        Write-Status "Expanding downloaded archive..."
        Expand-Archive -LiteralPath $zipPath -DestinationPath $extractDir -Force
        if (-not [System.IO.Directory]::Exists($expandedRoot)) {
            throw "Expanded archive does not contain unity-mcp-advanced-main."
        }

        Move-Item -LiteralPath $expandedRoot -Destination $CheckoutDir
    } finally {
        Remove-IfExists $zipPath
        Remove-IfExists $extractDir
    }
}

function Ensure-Checkout {
    param(
        [string]$CheckoutDir,
        [string]$EntryPoint
    )

    if ([System.IO.File]::Exists($EntryPoint)) {
        return
    }

    Remove-IfExists $CheckoutDir

    $gitPath = Get-CommandPath @("git.exe", "git")
    if ($null -ne $gitPath) {
        try {
            Install-FromGit -GitPath $gitPath -CheckoutDir $CheckoutDir
        } catch {
            Write-Status "git clone failed: $($_.Exception.Message)"
        }
    }

    if (-not [System.IO.File]::Exists($EntryPoint)) {
        Install-FromZip -CheckoutDir $CheckoutDir
    }

    if (-not [System.IO.File]::Exists($EntryPoint)) {
        throw @"
Failed to install HuntNight/unity-mcp-advanced.

Manual recovery:
1. Download https://github.com/HuntNight/unity-mcp-advanced
2. Extract or clone it into:
   $CheckoutDir
3. Make sure this file exists:
   $EntryPoint
"@
    }
}

function Ensure-NodeModules {
    param([string]$UnityMcpDir)

    $nodeModulesDir = Join-Path $UnityMcpDir "node_modules"
    if ([System.IO.Directory]::Exists($nodeModulesDir)) {
        return
    }

    $npmPath = Get-CommandPath @("npm.cmd", "npm")
    if ($null -eq $npmPath) {
        throw "npm was not found in PATH. Install Node.js 18+ and try again."
    }

    Write-Status "Installing Node dependencies in $UnityMcpDir ..."
    Push-Location $UnityMcpDir
    try {
        & $npmPath install
        if ($LASTEXITCODE -ne 0) {
            throw "npm install failed with exit code $LASTEXITCODE."
        }
    } finally {
        Pop-Location
    }
}

function Start-McpServer {
    param(
        [string]$UnityMcpDir,
        [string]$EntryPoint
    )

    $nodePath = Get-CommandPath @("node.exe", "node")
    if ($null -eq $nodePath) {
        throw "node was not found in PATH. Install Node.js 18+ and try again."
    }

    Write-Status "Starting unity-mcp server..."
    Push-Location $UnityMcpDir
    try {
        & $nodePath $EntryPoint
        exit $LASTEXITCODE
    } finally {
        Pop-Location
    }
}

$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
$externalRoot = Join-Path $repoRoot ".external"
$checkoutDir = Join-Path $externalRoot "unity-mcp-advanced"
$unityMcpDir = Join-Path $checkoutDir "unity-mcp"
$entryPoint = Join-Path $unityMcpDir "index.js"

if (-not [System.IO.Directory]::Exists($externalRoot)) {
    New-Item -ItemType Directory -Path $externalRoot | Out-Null
}

Ensure-Checkout -CheckoutDir $checkoutDir -EntryPoint $entryPoint
Ensure-NodeModules -UnityMcpDir $unityMcpDir

if ($RunServer) {
    Start-McpServer -UnityMcpDir $unityMcpDir -EntryPoint $entryPoint
    exit 0
}

Write-Status "unity-mcp-advanced is installed."
Write-Status "Next step inside Unity: install the package at .external/unity-mcp-advanced/unity-mcp/tools/unity-bridge/unity-extension/package.json"
