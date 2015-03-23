 param($installPath, $toolsPath, $package, $project)

# Get the project item for the scripts folder
try {
    $scriptsFolderProjectItem = $project.ProjectItems.Item("App_Start")
    $projectScriptsFolderPath = $scriptsFolderProjectItem.FileNames(1)

    Write-Host "AppStart: " + $projectScriptsFolderPath
}
catch {
    # No Scripts folder
    Write-Host "No scripts folder found"
}