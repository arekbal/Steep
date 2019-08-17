
$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

. $ROOT\scripts\_include.ps1

Use-Cmd 'BENCH' 'dotnet run -p "$ROOT\Steep.Bench\Steep.Bench.csproj" -c RELEASE -v m'

Print 'DONE'
