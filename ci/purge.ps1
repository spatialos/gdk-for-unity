# This script is used to kill all dangling procceses at the end of a Buildkite build.
# 
# It does this by using [handle](https://docs.microsoft.com/en-us/sysinternals/downloads/handle)
# to find all processes of a particular name which have file locks on our build (project) directory.
# We can then attempt to kill each of these processes if any exist.

$ScriptLocation = $MyInvocation.MyCommand.Path
$ScriptDir = Split-Path $ScriptLocation
cd "$ScriptDir/.."

function Kill-Dangling-Processes {
	param( [string]$ProcessName )

	echo "Looking for ${ProcessName} processes to kill..."
	$Dir=$(pwd).Path
	$Out=$(handle64 -accepteula -p "$($ProcessName).exe" $Dir)
	ForEach ($line in $($OUT -split "`r`n"))
	{
		$Result = $([regex]::Match("$line", "pid: (.*) type"))
		if ($Result.Success)
		{
			$ppid = $Result.Groups[1].Value
			taskkill /f /pid $ppid
		}
	}
}

$ProcessesToKill = @("dotnet", "Unity", "UnityPackageManager")

ForEach ($Process in $ProcessesToKill) 
{
	Kill-Dangling-Processes $Process
}
