 param($installPath, $toolsPath, $package, $project)

 $templateFilename = "MVCGridConfig.tt"
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

$rootNamespace = $project.Properties["RootNamespace"]
Write-Host "rootNamespace: ($rootNamespace)"

try {
    $sourcePath = Join-Path $toolsPath $templateFilename
    Write-Host "sourcePath: ($sourcePath)"
    $appStartFolderProjectItem.ProjectItems.AddFromTemplate($sourcePath, $codeFilename)
}
catch {
    # No Scripts folder
    Write-Host "Error adding file: " + $_
    exit
}

#$toolsPath