
$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

. $ROOT\scripts\_include.ps1

Use-Cmd 'TESTS' 'dotnet test -c $BUILD_CONFIGURATION'

Print 'DONE'
