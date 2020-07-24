# VS Code Template for Space Engineers & Scripts

> :warning: **Note**: This template use a dirty solution that wraps the *Program* code in a c# .net class library, but it is a quick one to start coding in VS Code since it provides autocompletion of Space Engineers classes. There are more robusts solutions, but I just wanted a bare minimum (Making it work in eclipse on linux wasn't fun).

## Using the template
* Install Space Engineers (or else!)
* Install Visual Studio Code (2015 or later)
  * Install NuGet Packet Manager and C# Extension (from OmniSharp)
* Install .net core sdk (VS Code may alert you of this after installing the C# extension)
* Install .net framework 4.6.1 developer pack
* Clone or download this repository
* copy and paste the SpaceEngineers\Luisau\Template folder to a new one, i.e.: SpaceEngineers\Luisau\MyCoolScript and change the name of the program-template.cs file in there to mycoolscript.cs
* Open VS Code, and Using *File > Open Folder* open the cloned repository root folder.
* Once VS Code loads, open the mycoolscript.cs file and edit the Namespace and place your code in the designated space inside the template.
* If you have installed SE in a different location (other than `C:\Program Files (x86)\Steam\steamapps\common\SpaceEngineers`), you should open the SpaceEngineers.csproj file and fix the paths.

I used ideas from [gregretkowski](https://github.com/gregretkowski/VSC-SE) and [mrdaemon](https://github.com/mrdaemon), cheers!.