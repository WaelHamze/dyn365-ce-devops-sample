[CmdletBinding()]

param
(
)

$ErrorActionPreference = "Stop"

#Script Location
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Write-Verbose "Script Path: $scriptPath"

Write-Verbose "ConnectionString = $connectionString"

& "$scriptPath\..\..\..\packages\XrmCIFramework.9.0.0.31\tools\PackSolution.ps1" -Verbose -CoreToolsPath "$scriptPath\..\..\..\packages\Microsoft.CrmSdk.CoreTools.9.0.2.6\content\bin\coretools" -unpackedFilesFolder "$scriptPath\Customizations" -mappingFile "$scriptPath\mapping.xml" -PackageType Both -TreatPackWarningsAsErrors $false -UpdateVersion $false -OutputPath "C:\temp"
