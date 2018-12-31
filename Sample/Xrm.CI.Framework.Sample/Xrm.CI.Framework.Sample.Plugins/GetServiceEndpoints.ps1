[CmdletBinding()]

param
(
    [string]$CrmConnectionString #The connection string as per CRM Sdk
)

$ErrorActionPreference = "Stop"

#Script Location
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Write-Verbose "Script Path: $scriptPath"

Write-Verbose "ConnectionString = $connectionString"

if ($CrmConnectionString -eq '')
{
	$CrmConnectionString = $Env:CrmConnection
}
$MappingFile = "$scriptPath\ServiceEndpoints.json"
$Timeout = 360

& "$scriptPath\..\packages\XrmCIFramework.9.0.0.27\tools\GetServiceEndpointRegistration.ps1" -Verbose -CrmConnectionString "$CrmConnectionString" -MappingFile "$MappingFile" -Timeout $Timeout
