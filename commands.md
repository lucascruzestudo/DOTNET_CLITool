```
dotnet pack --configuration Release
dotnet tool install --global --add-source ./bin/Release CustomTool --version 1.0.0
customtool new command -p projectname  -e entityname -o outputpath
```
