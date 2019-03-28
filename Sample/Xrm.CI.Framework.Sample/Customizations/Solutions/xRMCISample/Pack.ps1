[CmdletBinding()]

param
(
)

$ErrorActionPreference = "Stop"

#Script Location
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Write-Verbose "Script Path: $scriptPath"

Write-Verbose "ConnectionString = $connectionString"

$matches = Get-ChildItem -Path "$scriptPath\..\..\..\packages\XrmCIFramework.*" | Sort-Object Name -Descending
if ($matches.Length -gt 0)
{
	$frameworkPath = $matches[0].FullName
	Write-Verbose "Using XrmCIFramework: $frameworkPath"
}
else
{
	throw "XrmCIFramework not found in nuget packages"
}

& "$frameworkPath\tools\PackSolution.ps1" -Verbose -CoreToolsPath "$scriptPath\..\..\..\packages\Microsoft.CrmSdk.CoreTools.9.0.2.6\content\bin\coretools" -unpackedFilesFolder "$scriptPath\Customizations" -mappingFile "$scriptPath\mapping.xml" -PackageType Both -TreatPackWarningsAsErrors $false -UpdateVersion $false -OutputPath "C:\temp"
