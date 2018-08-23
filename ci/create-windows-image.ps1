### This script is a guide for Improbable's internal build machine images.
### If you don't work at Improbable, this may be interesting as a guide to what software versions we use for our
### automation, but not much more than that.

### Run from an Administrator command prompt with:
### powershell -executionpolicy Unrestricted ci/create-image.sh
### The machine will reboot after the script runs.

### Required env vars:
### $env:UNITY_SERIAL
### $env:UNITY_USERNAME
### $env:UNITY_PASSWORD

# https://chocolatey.org/docs/how-to-setup-offline-installation
$CHOCOLATEY_VERSION="0.10.11"
$GIT_VERSION="2.18.0"
$RESHARPER_CLI_VERSION="2018.1.2"
$SPATIAL_VERSION="1.1.9"

# From https://public-cdn.cloud.unity3d.com/hub/prod/releases-win32.json
$UNITY_VERSION="2018.2.0b10"
$UNITY_RELEASE_HASH="4bc57476174c"

# Direct link to installer at https://my.visualstudio.com/Downloads?q=Visual%20Studio%202017
# VS Professional 15.7
$VISUAL_STUDIO_HASH="09e0efec-0cfd-4015-b732-e8543589a6e0/1c89b3fa4ca0d287138916dca1625a0e"

### To get a logfile in "c:\programdata\ssh\logs", add the following in the embedded script segment named $sshdConfigContent, below
###     SyslogFacility LOCAL0
###     LogLevel DEBUG

### TODO: Install Improbable robots SSH keys for github
### TODO: Install spatial service tokens

Import-Module BitsTransfer
$ErrorActionPreference = "Stop"

mkdir c:\build

Write-Host "Installing Chocolatey..."
$env:chocolateyVersion = "$CHOCOLATEY_VERSION"
Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

Write-Host "Installing git..."
Start-Process -Wait -NoNewWindow -FilePath "choco" -ArgumentList "install", "git", "--yes", "--version=$GIT_VERSION"

Write-Host "Installing Resharper CLI tools..."
Start-Process -Wait -NoNewWindow -FilePath "choco" -ArgumentList "install", "resharper-clt.portable", "--yes", "--version=$RESHARPER_CLI_VERSION"

Write-Host "Installing Spatial CLI..."
Start-Process -Wait -NoNewWindow -FilePath "choco" -ArgumentList "install", "spatial", "--yes", "--version=$SPATIAL_VERSION"

Start-BitsTransfer -Source "https://download.unity3d.com/download_unity/$UNITY_RELEASE_HASH/Windows64EditorInstaller/UnitySetup64-$UNITY_VERSION.exe" -Destination "c:\build\unity-installer.exe"
Start-BitsTransfer -Source "https://download.unity3d.com/download_unity/$UNITY_RELEASE_HASH/TargetSupportInstaller/UnitySetup-Linux-Support-for-Editor-$UNITY_VERSION.exe" -Destination "c:\build\unity-linux-installer.exe"
Start-BitsTransfer -Source "https://download.unity3d.com/download_unity/$UNITY_RELEASE_HASH/TargetSupportInstaller/UnitySetup-Mac-Mono-Support-for-Editor-$UNITY_VERSION.exe" -Destination "c:\build\unity-mac-installer.exe"
Start-BitsTransfer -Source "https://download.visualstudio.microsoft.com/download/pr/$VISUAL_STUDIO_HASH/vs_professional.exe" -Destination "c:\build\vs-installer.exe"

Write-Host "Installing Unity..."
Start-Process -Wait -NoNewWindow -FilePath /build/unity-installer.exe -ArgumentList "/S"

Write-Host "Activating Unity..."
Start-Process -Wait -NoNewWindow -FilePath "C:\Program Files\Unity\Editor\Unity.exe" -ArgumentList `
 "-quit", `
 "-batchmode", `
 "-serial", "$env:UNITY_SERIAL", `
 "-username", "$env:UNITY_USERNAME", `
 "-password", "$env:UNITY_PASSWORD", `
 "-logFile", "c:\build\unity-activation.log"
if ($LASTEXITCODE -ne 0) {
	throw("Failed to activate Unity")
}

Write-Host "Installing Unity Mac support..."
Start-Process -Wait -NoNewWindow -FilePath /build/unity-mac-installer.exe -ArgumentList "/S"
if ($LASTEXITCODE -ne 0) {
	throw("Failed to install Unity Mac support")
}

Write-Host "Installing Unity Linux support..."
Start-Process -Wait -NoNewWindow -FilePath /build/unity-linux-installer.exe -ArgumentList "/S"
if ($LASTEXITCODE -ne 0) {
	throw("Failed to install Unity Linux support")
}

Write-Host "Installing Visual studio..."
Start-Process -Wait -NoNewWindow /build/vs-installer.exe -ArgumentList `
    "--add", "Microsoft.VisualStudio.Workload.ManagedGame", `
    "--add", "Microsoft.VisualStudio.Workload.NativeDesktop", `
    "--add", "Microsoft.VisualStudio.Component.VC.Tools.x86.x64", `
    "--add", "Microsoft.VisualStudio.Component.Windows10SDK.16299.Desktop", `
    "--add", "Microsoft.VisualStudio.Workload.NetCoreTools", `
    "--quiet", "--norestart", "--wait"
if ($LASTEXITCODE -ne 0) {
    throw("Failed to install Visual Studio")
}
