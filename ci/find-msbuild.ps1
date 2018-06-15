# VS2017+ place MSBuild in a different location than 2015 and prior.

$msb2017 = Resolve-Path "${env:ProgramFiles(x86)}\Microsoft Visual Studio\*\*\MSBuild\*\bin\msbuild.exe" -ErrorAction SilentlyContinue | Select-Object -first 1
if($msb2017) {
    Write-Host $msb2017
    return
}

$msBuild2015 = "${env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe"

if(-not (Test-Path $msBuild2015)) {
    throw 'Could not find MSBuild 2015 or later.'
}

Write-Host $msBuild2015
