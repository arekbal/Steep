
$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

. $ROOT\scripts\_include.ps1
. $ROOT\scripts\_build.ps1

. $ROOT\scripts\_show-vars.ps1

Build


if ($BRANCH -eq 'master')
{
  Nuget-PP 'Steep'
}

Print 'DONE'
