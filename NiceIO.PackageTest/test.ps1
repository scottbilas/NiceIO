del -recur -ea:silent bin, obj, packages, ../bin, ../obj
dotnet pack .. -p:NuspecFile=NiceIO.nuspec
dotnet add package OkTools.NiceIO --source ..\bin\release --package-directory packages
if ((dotnet run) -ne 'Path is a/b/c/file.txt') { throw 'it broke' }
''
'Everything is shiny'
