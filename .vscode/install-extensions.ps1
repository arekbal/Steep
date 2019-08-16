code --install-extension ms-vscode.csharp --force
code --install-extension bierner.markdown-preview-github-styles --force
code --install-extension fudge.auto-using --force
code --install-extension gruntfuggly.todo-tree --force

Write-Host "Press any key to continue..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
Write-Host
