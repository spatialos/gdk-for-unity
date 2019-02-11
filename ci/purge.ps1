# This script is used to kill all dangling procceses at the end of a Buildkite build.
# 
# It does this by using [handle](https://docs.microsoft.com/en-us/sysinternals/downloads/handle)
# to find all processes of a particular name which have file locks on our build (project) directory.
# We can then attempt to kill each of these processes if any exist.

$SCRIPT_LOCATION = $MyInvocation.MyCommand.Path
$SCRIPT_DIR = Split-Path $SCRIPT_LOCATION
cd "${SCRIPT_DIR}/.."

function Kill-Dangling-Processes {
	param( [string]$ProcessName )

	echo "Looking for ${ProcessName} processes to kill..."
	${DIR}=$(pwd).Path
	${OUT}=$(handle64 -accepteula -p "${ProcessName}.exe" ${DIR})
	ForEach ($line in $($OUT -split "`r`n"))
	{
		$result = $([regex]::Match("${line}", "pid: (.*) type"))
		if ($result.Success)
		{
			$ppid = $result.Groups[1].Value
			taskkill /f /pid $ppid
		}
	}
}

$PROCESSES_TO_KILL = @("dotnet", "Unity", "UnityPackageManager")

ForEach ($Process in $PROCESSES_TO_KILL) 
{
	Kill-Dangling-Processes $Process
}
