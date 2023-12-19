# Publishing NiceIO

Do this:

* Edit `NiceIO.nuspec` and bump the version
* Check that everything is shiny
  * `dotnet test --filter "TestCategory!=CrashesNUnitOnSystemNetFramework"`
  * `NiceIO.PackageTest/test.ps1`
* Send it to GitHub
  * `git commit/reset` and get to a clean state
  * `git tag release-$version` where `$version` is what was set in the .nuspec above
  * `git push --tags`

  * If there are no errors, publishing the new version to the Nuget Gallery should happen in about 5 minutes.
