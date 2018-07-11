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

### END IMPROBABLE SETUP
### -------------------- ^^^

Write-Host "Installing OpenSSH..."

### The code below is adapted from https://github.com/jen20/packer-aws-windows-ssh/blob/master/files/configure-source-ssh.ps1
### Explanatory article: https://operator-error.com/2018/04/16/windows-amis-with-even/
### -------------------- vvv

# Version and download URL
$openSSHVersion = "7.6.1.0p1-Beta"
$openSSHURL = "https://github.com/PowerShell/Win32-OpenSSH/releases/download/v$openSSHVersion/OpenSSH-Win64.zip"

Set-ExecutionPolicy Unrestricted -Force -Scope Process

# GitHub became TLS 1.2 only on Feb 22, 2018
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12;

# Function to unzip an archive to a given destination
Add-Type -AssemblyName System.IO.Compression.FileSystem
Function Unzip
{
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true, Position=0)]
        [string] $ZipFile,
        [Parameter(Mandatory=$true, Position=1)]
        [string] $OutPath
    )

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipFile, $outPath)
}

# Set various known paths
$openSSHZip = Join-Path $env:TEMP 'OpenSSH.zip'
$openSSHInstallDir = Join-Path $env:ProgramFiles 'OpenSSH'
$openSSHInstallScript = Join-Path $openSSHInstallDir 'install-sshd.ps1'
$openSSHDownloadKeyScript = Join-Path $openSSHInstallDir 'download-key-pair.ps1'
$openSSHDaemon = Join-Path $openSSHInstallDir 'sshd.exe'
$openSSHDaemonConfig = [io.path]::combine($env:ProgramData, 'ssh', 'sshd_config')

# Download and unpack the binary distribution of OpenSSH
Invoke-WebRequest -Uri $openSSHURL `
    -OutFile $openSSHZip `
    -ErrorAction Stop

Unzip -ZipFile $openSSHZip `
    -OutPath "$env:TEMP" `
    -ErrorAction Stop

Remove-Item $openSSHZip `
    -ErrorAction SilentlyContinue

# Move into Program Files
Move-Item -Path (Join-Path $env:TEMP 'OpenSSH-Win64') `
    -Destination $openSSHInstallDir `
    -ErrorAction Stop

# Run the install script, terminate if it fails
& Powershell.exe -ExecutionPolicy Bypass -File $openSSHInstallScript
if ($LASTEXITCODE -ne 0) {
	throw("Failed to install OpenSSH Server")
}

# BEGIN IMPROBABLE - Allow read and execute for Users
$acl = Get-ACL -Path $openSSHInstallDir
$ar = New-Object System.Security.AccessControl.FileSystemAccessRule( `
	"BUILTIN\Users", "ReadAndExecute", "Allow")
$acl.AddAccessRule($ar)
Set-Acl -Path $openSSHInstallDir -AclObject $acl
# END IMPROBABLE

# Add a firewall rule to allow inbound SSH connections to sshd.exe
New-NetFirewallRule -Name sshd `
    -DisplayName "OpenSSH Server (sshd)" `
    -Group "Remote Access" `
    -Description "Allow access via TCP port 22 to the OpenSSH Daemon" `
    -Enabled True `
    -Direction Inbound `
    -Protocol TCP `
    -LocalPort 22 `
    -Program "$openSSHDaemon" `
    -Action Allow `
    -ErrorAction Stop

# Ensure sshd automatically starts on boot
Set-Service sshd -StartupType Automatic `
    -ErrorAction Stop

# Set the default login shell for SSH connections to Powershell
New-Item -Path HKLM:\SOFTWARE\OpenSSH -Force
New-ItemProperty -Path HKLM:\SOFTWARE\OpenSSH `
    -Name DefaultShell `
    -Value "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe" `
    -ErrorAction Stop

$keyDownloadScript = @'
# Download the instance key pair and authorize Administrator logins using it
$openSSHAdminUser = 'c:\ProgramData\ssh'
$openSSHAuthorizedKeys = Join-Path $openSSHAdminUser 'authorized_keys'

If (-Not (Test-Path $openSSHAdminUser)) {
    New-Item -Path $openSSHAdminUser -Type Directory
}

# BEGIN IMPROBABLE - use Google services
# See https://cloud.google.com/compute/docs/storing-retrieving-metadata#default
$keyUrl = "http://metadata.google.internal/computeMetadata/v1/project/attributes/ssh-keys"
# END IMPROBABLE
$keyReq = [System.Net.WebRequest]::Create($keyUrl)
# BEGIN IMPROBABLE - Add Google metadata to allow querying
$keyReq.Headers.Add("Metadata-Flavor", "Google")
# END IMPROBABLE
$keyResp = $keyReq.GetResponse()
$keyRespStream = $keyResp.GetResponseStream()
$streamReader = New-Object System.IO.StreamReader $keyRespStream
$keyMaterial = $streamReader.ReadToEnd()

$keyMaterial | Out-File -Append -FilePath $openSSHAuthorizedKeys -Encoding ASCII

# Ensure access control on authorized_keys meets the requirements
$acl = Get-ACL -Path $openSSHAuthorizedKeys
$acl.SetAccessRuleProtection($True, $True)
Set-Acl -Path $openSSHAuthorizedKeys -AclObject $acl

$acl = Get-ACL -Path $openSSHAuthorizedKeys
$ar = New-Object System.Security.AccessControl.FileSystemAccessRule( `
	"NT Authority\Authenticated Users", "ReadAndExecute", "Allow")
$acl.RemoveAccessRule($ar)
$ar = New-Object System.Security.AccessControl.FileSystemAccessRule( `
	"BUILTIN\Administrators", "FullControl", "Allow")
$acl.RemoveAccessRule($ar)
$ar = New-Object System.Security.AccessControl.FileSystemAccessRule( `
	"BUILTIN\Users", "FullControl", "Allow")
$acl.RemoveAccessRule($ar)
Set-Acl -Path $openSSHAuthorizedKeys -AclObject $acl

Disable-ScheduledTask -TaskName "Download Key Pair"

$sshdConfigContent = @"
# Modified sshd_config, created by Packer provisioner

PasswordAuthentication yes
PubKeyAuthentication yes
PidFile __PROGRAMDATA__/ssh/logs/sshd.pid
AuthorizedKeysFile __PROGRAMDATA__/ssh/authorized_keys
# BEGIN IMPROBABLE - Allow all users
# AllowUsers Administrator
# END IMPROBABLE
Subsystem       sftp    sftp-server.exe
"@

Set-Content -Path C:\ProgramData\ssh\sshd_config `
    -Value $sshdConfigContent

'@
$keyDownloadScript | Out-File $openSSHDownloadKeyScript

# Create Task - Ensure the name matches the verbatim version above
$taskName = "Download Key Pair"
$principal = New-ScheduledTaskPrincipal `
    -UserID "NT AUTHORITY\SYSTEM" `
    -LogonType ServiceAccount `
    -RunLevel Highest
$action = New-ScheduledTaskAction -Execute 'Powershell.exe' `
  -Argument "-NoProfile -File ""$openSSHDownloadKeyScript"""
$trigger =  New-ScheduledTaskTrigger -AtStartup
Register-ScheduledTask -Action $action `
    -Trigger $trigger `
    -Principal $principal `
    -TaskName $taskName `
    -Description $taskName
Disable-ScheduledTask -TaskName $taskName

# Run the install script, terminate if it fails
& Powershell.exe -ExecutionPolicy Bypass -File $openSSHDownloadKeyScript
if ($LASTEXITCODE -ne 0) {
	throw("Failed to download key pair")
}

# Restart to ensure public key authentication works and SSH comes up
Restart-Computer