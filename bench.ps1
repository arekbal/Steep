param([object]$f)

$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

. $ROOT\scripts\_include.ps1
. $ROOT\scripts\_show-vars.ps1

if($f -ne $null)
{
  Use-Cmd 'BENCH' 'dotnet run -p "$ROOT\Steep.Bench\Steep.Bench.csproj" -c RELEASE -v m -- -m -f *$f*'
}
else
{
  Use-Cmd 'BENCH' 'dotnet run -p "$ROOT\Steep.Bench\Steep.Bench.csproj" -c RELEASE -v m -- -m -f ** --join'
}

Print 'DONE'
