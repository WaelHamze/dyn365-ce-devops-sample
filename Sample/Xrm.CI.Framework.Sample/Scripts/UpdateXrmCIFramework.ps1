#
# UpdateXrmCIFramework.ps1
#

param
(
	[string]$SourcePath
)

$ErrorActionPreference = "Stop"

#Script Location
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Write-Verbose "Script Path: $scriptPath"

$scriptsPath = "$SourcePath\MSDYNV9\Xrm.Framework.CI\Xrm.Framework.CI.PowerShell.Scripts"
$cmdletsPath = "$SourcePath\MSDYNV9\Xrm.Framework.CI\Xrm.Framework.CI.PowerShell.Cmdlets\bin\Release"

$matches = Get-ChildItem -Path "$scriptPath\..\packages\XrmCIFramework.*" | Sort-Object Name -Descending
if ($matches.Length -gt 0)
{
	$frameworkPath = $matches[0].FullName
	Write-Verbose "Using XrmCIFramework: $frameworkPath"
}
else
{
	throw "XrmCIFramework not found in nuget packages"
}

$targetPath = "$frameworkPath\tools\"

Copy-Item -Path $scriptsPath\*.ps* -Destination $targetPath
Copy-Item -Path $cmdletsPath\*.dll -Destination $targetPath
Copy-Item -Path $cmdletsPath\..\..\*.psd1 -Destination $targetPath

