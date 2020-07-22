# VS Code Template for Space Engineers & Scripts

> :warning: **Note**: This template use a dirty solution that wraps the *Program* code in a c# .net class library, but it is a quick one to start coding in VS Code since it provides autocompletion of Space Engineers classes. There are more robusts solutions, but I just wanted a bare minimum (Making it work in eclipse on linux wasn't fun).

## Why, How, What?
* Why : Because the In-Game editor is just not enough. Also, my windows partition only have Games in it. Installing a full IDE with services and libraries was out of the question.
* How : Using the dotnet cli tool you can create a "Solutions" File and a "MSLIB" project where you can reference SpaceEngineers DLL's and use autocomplete, yay!
* What : A basic program template that lets you reference the SE DLL's in a C# workspace.
  
## Using the template
* Install Space Engineers (or else!)
* Install Visual Studio Code (2015 or later)
  * Install NuGet Packet Manager and C# Extension (from OmniSharp)
* Install .net core sdk (VS Code may alert you of this after installing the C# extension)
* Install .net framework 4.6.1 developer pack
* clone this repository
* copy and paste the SpaceEngineers\Luisau\Template folder to a new one, i.e.: SpaceEngineers\Luisau\MyCoolScript and change the name of the program-template.cs file in there to mycoolscript.cs
* Open VS Code, and Using *File > Open Folder* open the cloned repository root folder.
* Once VS Code loads, open the mycoolscript.cs file and edit the Namespace and place your code.
* If you have installed SE in a different location (other than the default one, you should open the SpaceEngineers.csproj file and fix the paths)