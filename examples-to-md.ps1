$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

$NewLine = [Environment]::NewLine

Get-ChildItem $ROOT\Steep.Examples\ -Filter *.Example.cs -Recurse |
Foreach-Object {
  $exampleName =  $_.BaseName.substring(0,$_.BaseName.IndexOf('.'))

  $originalContent = [IO.File]::ReadAllText($_.FullName)

  $exampleBegin = $originalContent.IndexOf("//!example-begin")
  $exampleEnd = $originalContent.IndexOf("//!example-end")

  $newContent = $originalContent
  if($exampleBegin -ge 0 -And $exampleEnd -ge 0) {
    $newLineIndex = $originalContent.IndexOf("`n", $exampleBegin) + 1
    $newContent = $originalContent.Substring($newLineIndex, $exampleEnd - $newLineIndex)
  }

  $newPath = ($ROOT + '\docs\examples\' + $exampleName + '.md')

  $newContent | Set-Content $newPath
  
  $lines = [IO.File]::ReadAllLines($newPath)

  # Clear extra indentation
  $indent = 100  

  foreach ($line in $lines){ # TODO: only shows 2 lines...
    if($line -eq '') {
      continue
    }

    $currIndent = 0

    foreach ($char in $line.GetEnumerator()){      
      if($char -eq ' ' -or $char -eq '`n') {
        $currIndent++
        continue
      }
      
    
      break
    }

    if($currIndent -lt $indent) {
      $indent = $currIndent
    }
  }

  Write-Host $indent

  $indentedNewContent = New-Object -TypeName "System.Text.StringBuilder"

  foreach ($line in $lines){
    if($line -eq '') {
      $indentedNewContent.AppendLine()
      continue
    }

    $indentedNewContent.AppendLine($line.substring($indent))
  }

  $s = $indentedNewContent.ToString()

  $content = '# ' + $exampleName + $NewLine + $NewLine + '```csharp' + $NewLine + $s.Trim() + $NewLine + '```'

  #filter and save content to the original file
  # $content | Where-Object {$_ -match 'step[49]'} | Set-Content $_.FullName

  #filter and save content to a new file 
  # $content | Where-Object {$_ -match 'step[49]'} | Set-Content ($_.BaseName + '_out.log')

  mkdir -Force $ROOT\docs\examples\

  $content | Set-Content $newPath
}

"Examples To MD"
