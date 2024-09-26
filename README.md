# NiceIO 
[![build and 
test](https://github.com/scottbilas/NiceIO/actions/workflows/validate.ymll/badge.svg)](https://github.com/scottbilas/NiceIO/actions/workflows/validate.ymll)

_This is a fork of Lucas Meijer's [NiceIO](https://github.com/lucasmeijer/NiceIO). 99.9% credit goes to Lucas for this
library. I mostly just added nuget injection packaging that maintains his "just copy it into your  project" design. -scott_

## About

For when you've had to use `System.IO` one time too many. 

I need to make c# juggle files & directories around a lot. It has to work on osx, linux and windows. It always hurts, and I've never enjoyed it. NiceIO is an attempt to fix that.

NiceIO is MIT Licensed.

## Installation

While you can just copy [NiceIO.cs](https://raw.githubusercontent.com/scottbilas/NiceIO/refs/heads/dev/NiceIO.cs)
directly into your project and hack at it, NiceIO is **best used in its package form**: add a project reference to
[OkTools.NiceIO](https://www.nuget.org/packages/OkTools.NiceIO).

But this is different from a typical Nuget package!! `OkTools.NiceIO` injects `NiceIO.cs` _directly into your project_.
There will be no `NiceIO.dll`. Also, because `NPath` is `partial`, you can add whatever you want to it without being
limited to extension methods (though those of course are also fine).

You get all the benefits of a version-controlled, upstream-maintained class while still being able to directly extend it.

To get `NPath`, do the following:

1. Add a reference to `OkTools.NiceIO` in your project.

2. (Optional) Decide if you want `NPath` to be public API. By default, it is `internal`, but if you want your library
to publish it or use in other public API, you can set `NICEIO_PUBLIC` as a `DefineConstant` in a project `PropertyGroup`.

3. (Optional) Decide if you want `NPath` to be in the default `NiceIO` namespace. You can change it to what you want by
setting a `PreprocessorValue` for `NICEIO_NAMESPACE` in a project `ItemGroup`.

4. (Optional) Add members directly to `NPath` by creating a `partial class NPath` in your project and adding them there.

Here is an example snippet of a `.csproj` that exercises the above features:

```xml
<PropertyGroup>
  <DefineConstants>$(DefineConstants);NICEIO_PUBLIC</DefineConstants>
</PropertyGroup>
<ItemGroup>
  <PackageReference Include="OkTools.NiceIO" />
  <PreprocessorValue Include="NICEIO_NAMESPACE" Value="My.Lovely.Namespace" Visible="false" />
</ItemGroup>
```

After a `dotnet restore`, this project will contain a new public class `My.Lovely.Namespace.NPath`.

Here's a silly example of extending `NPath` in that same project:

```c#
// MyProject/NiceIO_MoreStuff.cs
namespace My.Lovely.Namespace;
partial class NPath
{
    bool _someField;
    public void DoSomething() => _someField = _path.Length > 10;
}
``` 

Real world usage of the above features is demonstrated at another OkTools project at
[Core.csproj](https://github.com/scottbilas/OkTools/blob/dev/src/Core/Core.csproj)
and [NiceIO_Ext.cs](https://github.com/scottbilas/OkTools/blob/dev/src/Core/NiceIO_Ext.cs).     

## Basic Usage

```c#
//paths are immutable
NPath path1 = new NPath(@"/var/folders/something");
// /var/folders/something

//use back,forward,or trailing slashes,  doesnt matter
NPath path2 = new NPath(@"/var\folders/something///");
// /var/folders/something

//semantically the same
path1 == path2;
// true

// ..'s that are not at the beginning of the path get collapsed
new NPath("/mydir/../myfile.exe");
// /myfile.exe

//build paths
path1.Combine("dir1/dir2");
// /var/folders/something/dir1/dir2

//handy accessors
NPath.HomeDirectory;
// /Users/lucas

//all operations return their destination, so they fluently daisychain
NPath myfile = NPath.HomeDirectory.CreateDirectory("mysubdir").CreateFile("myfile.txt");
// /Users/lucas/mysubdir/myfile.txt

//common operations you know and expect
myfile.Exists();
// true

//you will never again have to look up if .Extension includes the dot or not
myfile.ExtensionWithDot;
// ".txt"

//getting parent directory
NPath dir = myfile.Parent;
// /User/lucas/mysubdir

//copying files,
myfile.Copy("myfile2");
// /Users/lucas/mysubdir/myfile2

//into not-yet-existing directories
myfile.Copy("hello/myfile3");
// /Users/lucas/mysubdir/hello/myfile3

//listing files
dir.Files(recurse:true);
// { /Users/lucas/mysubdir/myfile.txt, 
//   /Users/lucas/mysubdir/myfile2, 
//   /Users/lucas/mysubdir/hello/myfile3 }

//or directories
dir.Directories();
// { /Users/lucas/mysubdir/hello }

//or both
dir.Contents(recurse:true);
// { /Users/lucas/mysubdir/myfile.txt, 
//   /Users/lucas/mysubdir/myfile2, 
//   /Users/lucas/mysubdir/hello/myfile3, 
//   /Users/lucas/mysubdir/hello }

//copy entire directory, and listing everything in the copy
myfile.Parent.Copy("anotherdir").Files(recurse:true);
// { /Users/lucas/anotherdir/myfile, 
//   /Users/lucas/anotherdir/myfile.txt, 
//   /Users/lucas/anotherdir/myfile2, 
//   /Users/lucas/anotherdir/hello/myfile3 }

//easy accesors for common operations:
string text = myfile.ReadAllText();
string[] lines = myfile.ReadAllLines();
myFile.WriteAllText("hello");
myFile.WriteAllLines(new[] { "one", "two"});
```
