. $ROOT\scripts\_include.ps1

function Build
{    
  if($IsWindows -eq $true)
  {
    Use-Cmd 'GIT VERSION INSTALL' 'choco install GitVersion.Portable --no-progress'
    Use-Cmd 'GIT VERSION' 'gitversion "$ROOT" -updateassemblyinfo'
  }
  else
  {
    Write-Host 'GIT VERSION - NOT IMPLEMENTED FOR Linux/MacOS, relying on plain shell commands there' -ForegroundColor 'DarkCyan'
  }

  Use-Cmd 'CLEAN' 'dotnet clean "$ROOT\Steep.sln" -v m'

  Use-Cmd 'BUILD' 'dotnet build "$ROOT\Steep.sln" -c $BUILD_CONFIGURATION -v m'

  Use-Cmd 'TESTS' 'dotnet test Steep.Tests -c $BUILD_CONFIGURATION -v m'
  

  if($IsWindows -eq $true)
  {
    Use-Cmd 'GET_GIT_VERSION' 'gitversion "$ROOT" -showvariable MajorMinorPatch' -ignore $true
    $VERSION = Invoke-Expression 'gitversion "$ROOT" -showvariable MajorMinorPatch'
    Show-Var 'VERSION'

    Use-Cmd 'GET_GIT_BRANCHNAME' 'git symbolic-ref --short HEAD' -ignore $true
    $BRANCH = Invoke-Expression 'gitversion "$ROOT" -showvariable BranchName'  
    Show-Var 'BRANCH'
  }
  else
  {
    Use-Cmd 'GET_GIT_VERSION' '(git describe --abbrev=0 --match v*).substring(1)' -ignore $true
    $VERSION = (Invoke-Expression 'git describe --abbrev=0 --match v*').substring(1)
    Show-Var 'VERSION'

    Use-Cmd 'GET_GIT_BRANCHNAME' 'git symbolic-ref --short HEAD' -ignore $true
    $BRANCH = Invoke-Expression 'git symbolic-ref --short HEAD'  
    Show-Var 'BRANCH'

    # Write-Host 'GIT VERSION - NOT IMPLEMENTED FOR Linux/MacOS' -ForegroundColor 'Red'
  }

  Print 'DONE'
}
