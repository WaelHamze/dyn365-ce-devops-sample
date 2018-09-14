[CmdletBinding()]

param
(
	[string]$CrmConnectionString #The connection string as per CRM Sdk
)

$ErrorActionPreference = "Stop"

if ($CrmConnectionString -eq '')
{
	$CrmConnectionString = $Env:CrmConnection
}

#Script Location
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Write-Verbose "Script Path: $scriptPath"

Write-Verbose "ConnectionString = $connectionString"

& "$scriptPath\..\packages\XrmCIFramework.9.0.0.23\tools\ExtractSolution.ps1" -Verbose -CoreToolsPath "$$scriptPath\..\packages\Microsoft.CrmSdk.CoreTools.9.0.2.4\content\bin" -UnpackedFilesFolder "$scriptPath\..\Customisations" -mappingFile "$scriptPath\mapping.xml" -solutionName "xRMCISample" -connectionString $connectionString -TreatPackWarningsAsErrors $true
