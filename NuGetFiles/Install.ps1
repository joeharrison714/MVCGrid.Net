 param($installPath, $toolsPath, $package, $project)

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
    If (Test-Path $appStartFolderPath){
        Write-Host "File already exists ($appStartFolderPath)"
        exit
    }
}
catch{
    Write-Host "Error checking for file"
    exit
}

try {
    $sourcePath = Join-Path $toolsPath $codeFilename
    Write-Host "sourcePath: ($sourcePath)"
    $appStartFolderProjectItem.ProjectItems.AddFromTemplate($sourcePath)
}
catch {
    # No Scripts folder
    Write-Host "Error adding file"
    exit
}

#$toolsPath