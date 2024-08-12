```
dotnet pack --configuration Release
dotnet tool install --global --add-source ./bin/Release CustomTool --version 1.0.0
customtool new command -c DeleteTeam -p Project -o ../Application/Features/Team/
```
