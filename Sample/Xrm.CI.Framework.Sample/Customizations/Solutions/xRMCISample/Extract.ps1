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

& "$frameworkPath\tools\ExtractSolution.ps1" -Verbose -CoreToolsPath "$scriptPath\..\..\..\packages\Microsoft.CrmSdk.CoreTools.9.0.2.12\content\bin\coretools" -UnpackedFilesFolder "$scriptPath\Customizations" -mappingFile "$scriptPath\mapping.xml" -solutionName "xRMCISample" -solutionFile $null -connectionString "$CrmConnectionString" -TreatPackWarningsAsErrors $false -PackageType "both"
