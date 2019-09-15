. $ROOT\scripts\_include.ps1

function Build {    

  Use-Cmd 'VERSION TAG' '(git describe --abbrev=0 --match v*).substring(1)' -ignore $true
  $VERSION = (Invoke-Expression 'git describe --abbrev=0 --match v*').substring(1)
  Show-Var 'VERSION'

  Use-Cmd 'BRANCH' 'git symbolic-ref --short HEAD' -ignore $true
  $BRANCH = Invoke-Expression 'git symbolic-ref --short HEAD'  
  Show-Var 'BRANCH'

  Update-AssemblyInfo "$ROOT/Steep"

  Use-Cmd 'CLEAN' 'dotnet clean "$ROOT\Steep.sln" -v m'

  Use-Cmd 'BUILD' 'dotnet build "$ROOT\Steep.sln" -c $BUILD_CONFIGURATION -v m'

  Use-Cmd 'TESTS' 'dotnet test Steep.Tests -c $BUILD_CONFIGURATION -v m'
 
 
  Print 'DONE'
}
