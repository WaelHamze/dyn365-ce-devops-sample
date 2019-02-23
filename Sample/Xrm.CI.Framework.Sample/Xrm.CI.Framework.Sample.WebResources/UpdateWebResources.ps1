[CmdletBinding()]

param
(
    [string]$CrmConnectionString, #The connection string as per CRM Sdk
	[string]$Key #The key for the stored connection string
)

$ErrorActionPreference = "Stop"

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

$AssemblyName = 'Xrm.CI.Framework.Sample.Plugins'
$WebResourceFolderPath = "$scriptPath\WebResources"
$SearchPattern = "*.js,*.html"
$RegExToMatchUniqueName = 'ud_.*?{filename}'
$IncludeFileExtensionForUniqueName = $true
$Publish = $false
$SolutionName = "xRMCISample"
$FailIfWebResourceNotFound = $true
$Timeout = 120

& "$frameworkPath\tools\UpdateFoldersWebResources.ps1" -Verbose -CrmConnectionString "$CrmConnectionString" -WebResourceFolderPath $WebResourceFolderPath -SearchPattern $SearchPattern -RegExToMatchUniqueName $RegExToMatchUniqueName -IncludeFileExtensionForUniqueName $IncludeFileExtensionForUniqueName -SolutionName $SolutionName -Publish $Publish -FailIfWebResourceNotFound $FailIfWebResourceNotFound -Timeout $Timeout
