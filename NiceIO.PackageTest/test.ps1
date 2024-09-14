pushd $PSScriptRoot
try {
    del -recur -ea:silent bin, obj, packages, ../bin, ../obj

    dotnet restore
    if ($LASTEXITCODE) { throw "fail to restore, error is $LASTEXITCODE" }

    dotnet pack .. -p:NuspecFile=NiceIO.nuspec
    if ($LASTEXITCODE) { throw "fail to dotnet pack, error is $LASTEXITCODE" }

    dotnet add package OkTools.NiceIO --source ..\bin\release --package-directory packages
    if ($LASTEXITCODE) { throw "fail to dotnet add package, error is $LASTEXITCODE" }

    if ((dotnet run) -ne 'Path is a/b/c/file.txt') { throw 'it broke' }
    ''
    'Everything is shiny'
}
finally {
    popd
}
