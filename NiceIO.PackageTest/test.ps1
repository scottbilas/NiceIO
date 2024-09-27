Push-Location $PSScriptRoot
Copy-Item NiceIO.PackageTest.csproj NiceIO.PackageTest.csproj.orig 

try {
    Remove-Item -recur -ea:silent bin, obj, packages, ../bin, ../obj

    dotnet restore
    if ($LASTEXITCODE) { throw "fail to restore, error is $LASTEXITCODE" }

    dotnet pack .. -p:NuspecFile=NiceIO.nuspec
    if ($LASTEXITCODE) { throw "fail to dotnet pack, error is $LASTEXITCODE" }

    dotnet add package OkTools.NiceIO --source ..\bin\release --package-directory packages
    if ($LASTEXITCODE) { throw "fail to dotnet add package, error is $LASTEXITCODE" }
    
    $result = (dotnet run)
    if ($result -ne 'Path is a/b/c/file.txt') {
        dotnet build -bl
        throw 'it broke - also see msbuild.binlog'
    }

    ''
    'Everything is shiny'
}
finally {
    Pop-Location
    Move-Item -force NiceIO.PackageTest.csproj.orig NiceIO.PackageTest.csproj
}
