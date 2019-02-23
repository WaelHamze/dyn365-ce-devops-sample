[CmdletBinding()]

param
(
    [string]$CrmConnectionString, #The connection string as per CRM Sdk
	[string]$Key #The key for the stored connection string
)

$ErrorActionPreference = "Stop"

#Script Location
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Write-Verbose "Script Path: $scriptPath"

Write-Verbose "ConnectionString = $connectionString"

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

Import-Module "$frameworkPath\tools\Xrm.Framework.CI.PowerShell.Cmdlets.psd1"

if ($CrmConnectionString)
{
	Write-Verbose "Using supplied connection string"
}
else
{
	Write-Verbose "Using connection store"
	$CrmConnectionString = GetXrmConnectionFromConfig($key);
}

$AssemblyPath = "$scriptPath\bin\Debug\Xrm.CI.Framework.Sample.WFActivities.dll"
$SolutionName = 'xRMCISample'
$MappingFile = "$scriptPath\PluginRegistration.json"
$RegistrationType = "upsert"
$Timeout = 360

& "$frameworkPath\tools\PluginRegistration.ps1" -Verbose -CrmConnectionString "$CrmConnectionString" -AssemblyPath "$AssemblyPath" -MappingFile "$MappingFile" -SolutionName $SolutionName -RegistrationType $RegistrationType -Timeout $Timeout
