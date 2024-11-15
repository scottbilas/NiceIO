Push-Location $PSScriptRoot
Copy-Item NiceIO.PackageTest.csproj NiceIO.PackageTest.csproj.orig

try {
    # delete everything including packages
    Remove-Item -recur -ea:silent bin, obj, packages, ../bin, ../obj

    # build and test
    dotnet test .. --filter "TestCategory!=CrashesNUnitOnSystemNetFramework"
    dotnet build .. -c Release

    # produce the niceio package
    dotnet pack .. -p:NuspecFile=NiceIO.nuspec
    if ($LASTEXITCODE) { throw "fail to dotnet pack, error is $LASTEXITCODE" }

    # add it to our project from local store
    dotnet add package OkTools.NiceIO
    if ($LASTEXITCODE) { throw "fail to dotnet add package, error is $LASTEXITCODE" }

    # restore and build
    dotnet build
    if ($LASTEXITCODE) { throw "fail to dotnet build, error is $LASTEXITCODE" }

    # including escape chars to clear the status
    $result = ./bin/Debug/net8.0/NiceIO.PackageTest.exe
    if ($result -ne 'Path is a/b/c/file.txt') {
        dotnet build -bl
        throw "it broke with result '$result' - also see msbuild.binlog"
    }

    ''
    'Everything is shiny'
}
finally {
    Move-Item -force NiceIO.PackageTest.csproj.orig NiceIO.PackageTest.csproj
    Pop-Location
}
