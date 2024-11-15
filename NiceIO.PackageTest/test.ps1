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

    # restore build and run to test that the embedding and extenions work    
    $result = (dotnet run)
    if (!$result.contains('Path is a/b/c/file.txt')) {
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
