

$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

. $ROOT\scripts\_include.ps1
. $ROOT\scripts\_build.ps1

. $ROOT\scripts\_show-vars.ps1

$NUGET_SERVER = if ($env:NUGET_SERVER) { $env:NUGET_SERVER } else { 'https://api.nuget.org/v3/index.json' } # 
$NUGET_APIKEY = $env:NUGET_APIKEY
 
Show-Var 'NUGET_SERVER'
Show-Var 'NUGET_APIKEY'

Build

if ($BRANCH -eq 'master') {
  Nuget-PP 'Steep'
}

Print 'Deploy-Nuget DONE'
