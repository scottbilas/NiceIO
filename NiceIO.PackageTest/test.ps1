pushd $PSScriptRoot
try {
    del -recur -ea:silent bin, obj, packages, ../bin, ../obj

    dotnet restore
    if ($LASTEXITCODE) { throw "fail to restore, error is $LASTEXITCODE" }

    dotnet pack .. -p:NuspecFile=NiceIO.nuspec
    if ($LASTEXITCODE) { throw "fail to dotnet pack, error is $LASTEXITCODE" }

    dotnet add package OkTools.NiceIO --source ..\bin\release --package-directory packages
    if ($LASTEXITCODE) { throw "fail to dotnet add package, error is $LASTEXITCODE" }
    
    foreach ($target in 'Test-Namespace', 'Test-Default') {
        "Testing $target"
        $result = (dotnet run -c $target)
        if ($result -ne 'Path is a/b/c/file.txt') {
            dotnet build -c $target -bl 
            throw 'it broke - also see msbuild.binlog'
        }
    }
    ''
    'Everything is shiny'
}
finally {
    popd
}
