param (
	[string] $baseFolder
)

$baseFolder = if ([System.IO.Path]::IsPathRooted($baseFolder)) {$baseFolder} else {[System.IO.Path]::GetFullPath((Join-Path $pwd $baseFolder))} 

Write-Host "All subfolders on $baseFolder cleaned now."

$currentFolder = $baseFolder

do 
{
  $dirs = Get-ChildItem $currentFolder -directory -recurse | Where-Object { (Get-ChildItem $_.fullName).count -eq 0 } | Select-Object -expandproperty FullName
  $dirs | Foreach-Object { Remove-Item $_ }
} 
while ($dirs.count -gt 0)

Get-ChildItem $baseFolder -include bin,obj -Recurse |  Remove-Item -Force -Recurse