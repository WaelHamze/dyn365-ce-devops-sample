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

#Print Parameters
Write-Verbose "ConnectionString = $CrmConnectionString"
Write-Verbose "Key = $Key"

#Variables
$DataFile = "$env:TEMP\reference_data_$(get-date -f yyyyMMdd-HHmmss).zip"

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

$exportParams = @{
	crmConnectionString = "$CrmConnectionString"
	crmConnectionTimeout = 360
	dataFile = $DataFile
	schemaFile = "$scriptPath\Schema.xml"
	logsDirectory = "$env:TEMP"
	configurationMigrationModulePath = "C:\Wael\Libraries\Microsoft.Xrm.Tooling.ConfigurationMigration\1.0.0.26"
	toolingConnectorModulePath = "C:\Wael\Libraries\Microsoft.Xrm.Tooling.CrmConnector.PowerShell\3.3.0.874"
}

& "$frameworkPath\tools\ExportCMData.ps1" @exportParams

$extractParams = @{
	dataFile = $DataFile
	extractFolder = "$scriptPath\Reference"
	sortExtractedData = $true
	splitExtractedData = $true
}

& "$frameworkPath\tools\ExtractCMData.ps1" @extractParams