[CmdletBinding()]

param
(
)

$ErrorActionPreference = "Stop"

#Script Location
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Write-Verbose "Script Path: $scriptPath"

Write-Verbose "ConnectionString = $connectionString"

& "$scriptPath\..\packages\XrmCIFramework.9.0.0.23\tools\PackSolution.ps1" -Verbose -CoreToolsPath "$scriptPath\..\packages\Microsoft.CrmSdk.CoreTools.9.0.2.4\content\bin\coretools" -unpackedFilesFolder "$scriptPath\..\Customisations" -mappingFile "$scriptPath\mapping.xml" -PackageType Both -TreatPackWarningsAsErrors $false -UpdateVersion $false -OutputPath "C:\temp"
