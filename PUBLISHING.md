# Publishing NiceIO

Do this:

* Edit `NiceIO.nuspec` and bump the version
* Check that everything is shiny
  * `NiceIO.PackageTest/test.ps1`
* Confirm API changes if any
  * `dotnet tool restore`
  * `dotnet tool run generate-public-api --target-frameworks net8.0 --assembly (resolve-path bin\Release\netstandard2.0\NiceIO.dll) | out-file api.txt`
  * Check diff to be sure doing correct semantic version bump
* Send it to GitHub
  * `git commit/reset` and get to a clean state
  * `git tag release-$version` where `$version` is what was set in the .nuspec above
  * `git push --tags`
  * If there are no errors, publishing the new version to the Nuget Gallery should happen in about 5 minutes.
