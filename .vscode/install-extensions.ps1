code --install-extension ms-vscode.csharp --force
code --install-extension bierner.markdown-preview-github-styles --force
code --install-extension fudge.auto-using --force
code --install-extension gruntfuggly.todo-tree --force
code --install-extension ms-vscode.powershell --force
code --install-extension shd101wyy.markdown-preview-enhanced --force
code --install-extension davidanson.vscode-markdownlint --force
code --install-extension yzhang.markdown-all-in-one --force

Write-Host "Press any key to continue..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
Write-Host
