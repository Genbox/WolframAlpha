$tempFolder = .\Temp

# Delete any old releases
Remove-Item $tempFolder\* -ErrorAction SilentlyContinue

# Pack the project into Temp
dotnet pack src\((Get-Item .).Parent).sln -c release -o $tempFolder

$nugetKey = Read-Host "Enter Nuget API key"

# Push to nuget
dotnet nuget push -k $nugetKey -s https://api.nuget.org/v3/index.json $tempFolder\*