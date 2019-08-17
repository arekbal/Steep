Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$InfoColor = "Yellow"
$ErrorColor = "Red"
$NewLine = [Environment]::NewLine

function Use-Cmd 
{
  param([String]$title, [String]$cmd, [bool]$ignore)
  
  Write-Output ""
  Write-Host $title -ForegroundColor $InfoColor
  Write-Host $cmd -ForegroundColor $InfoColor
  Write-Output ""
  if ($ignore -ne $true)
  {
	  Invoke-Expression $cmd
	  
	  [Console]::ResetColor()
	  
	  if ($LastExitCode -ne 0)
	  {
      ##throw 'failed running the command:"$cmd"'
      $message =  "ERROR: Failed running the command: '$cmd', returns code $LastExitCode"
      Write-Host $message -ForegroundColor $ErrorColor
      exit $LastExitCode
	  }
  }
  
  trap [System.Exception]
  {
    #Write-Error 'ERROR'
	Write-Error $_.Exception
    exit 1
  }
}

function Print
{
  param([String]$title)
  
  Write-Output ""
  Write-Host $title -ForegroundColor $InfoColor
  Write-Output "" 
}

function Show-Var
{
  param([String]$title)
  
  $var = Get-Variable -Name $title -ValueOnly  
  
  Write-Host $ExecutionContext.InvokeCommand.ExpandString('$title = $var') -ForegroundColor $InfoColor
}

function Nuget-PP
{
  param([String]$proj)
  
  Use-Cmd 'GET_LATEST_VERSION' "nuget list $proj | select -last 1" -ignore $true
  $LATEST_VERSION = Invoke-Expression 'nuget list $proj' | Select-Object -last 1
  $LATEST_VERSION = $LATEST_VERSION.Substring($proj.Length + 1) 
  Print-Var 'LATEST_VERSION'
  
  if ([System.Version]::Parse($VERSION).CompareTo([System.Version]::Parse($LATEST_VERSION)) -gt 0)
  {  
    Use-Cmd "PACK $proj" 'dotnet pack "$ROOT\$proj" --no-dependencies /p:PackageVersion=$VERSION -c $BUILD_CONFIGURATION --no-build -v m' 
    $cmd = 'dotnet nuget push "$ROOT\$proj\bin\$BUILD_CONFIGURATION\$proj.$VERSION.nupkg" -s "$NUGET_SERVER"'
    if($NUGET_APIKEY) { $cmd = $cmd + " -k $NUGET_APIKEY" }
    Use-Cmd "PUSH $proj" $cmd
  }
}

$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') } #$PSScriptRoot
$BUILD_CONFIGURATION = if ($env:BUILD_CONFIGURATION) { $env:BUILD_CONFIGURATION } else { 'RELEASE' }
$NET_CORE_APP_VER = if ($env:NET_CORE_APP_VER) { $env:NET_CORE_APP_VER } else { '2.2' }
$BUILD_LOGGER = $env:BUILD_LOGGER

trap [System.Exception]
{
  Write-Host 'ERROR' -ForegroundColor $ErrorColor
	Write-Error $_.Exception	
  exit 1
}
