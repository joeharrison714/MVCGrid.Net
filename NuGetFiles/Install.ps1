 param($installPath, $toolsPath, $package, $project)

 $templateFilename = "MVCGridConfig.cs.pp"
 $codeFilename = "MVCGridConfig.cs"

# Get the project item for the scripts folder
try {
    $appStartFolderProjectItem = $project.ProjectItems.Item("App_Start")
    $appStartFolderPath = $appStartFolderProjectItem.FileNames(1)

    Write-Host "AppStart: ($appStartFolderPath)"
}
catch {
    # No Scripts folder
    Write-Host "No App_Data folder found"
    exit
}

try{
    $targetPath = Join-Path $appStartFolderPath $codeFilename
    If (Test-Path $targetPath){
        Write-Host "File already exists ($targetPath)"
        exit
    }
}
catch{
    Write-Host "Error checking for file"
    exit
}

try{
    $rootNamespace = $project.Properties.Item("RootNamespace").Value.ToString()
    Write-Host "rootNamespace: ($rootNamespace)"
}
catch{
    Write-Host "Error getting root namespace"
    exit
}

try {
    $sourcePath = Join-Path $toolsPath $templateFilename
    Write-Host "sourcePath: ($sourcePath)"

    $text = Get-Content $sourcePath -Raw
    $text = $text.replace("`$rootnamespace$",$rootNamespace)

    $tempFile=[System.IO.Path]::GetTempFileName()

    Write-Host "temp file: $tempFile"

    $text | Out-File $tempFile

    $appStartFolderProjectItem.ProjectItems.AddFromTemplate($tempFile, $codeFilename)

    Remove-Item $tempFile
}
catch {
    Write-Host "Error adding file: " + $_
    exit
}
