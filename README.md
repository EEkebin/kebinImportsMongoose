# kebinImportsMongoose
***This is just an automated installer for Among Us, through Steam. Town of Us by polusgg, BetterCrewLink by OhMyGuus.***
> **Yes, you must own the game through Steam for this to work.**

## Installation

> ***LITERALLY JUST RUN IT AND FOLLOW INSTRUCTIONS.***

## Build

* **kebinImportsMongoose can only be built with .NET6.0.**  
   * This is due to the fact that .NET5.0 gives a seemingly unsolvable class error for WebClient.  
   * If someone can help with this, please do.  
> The following command can be used to build kebinImportsMongoose into a single Executable File.  
```dosbatch
dotnet publish -r win-x64 -c Release -o publish -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
```

## Credits
> **[EEkebin](https://github.com/EEkebin)**


## Licenses

> **[kebinImportsMongoose is licened under the GNU General Public License v3.0.](https://github.com/EEkebin/kebinImportsMongoose/blob/main/LICENSE)  
> [Town Of Us is licensed under the GNU General Public License v3.0.](https://github.com/polusgg/Town-Of-Us/blob/master/LICENSE)  
> [BetterCrewLink is licensed under the GNU General Public License v3.0.](https://github.com/OhMyGuus/BetterCrewLink/blob/nightly/LICENSE)  
> [SimpleJSON is licensed under the MIT License.](https://github.com/Bunny83/SimpleJSON/blob/master/LICENSE)**


## Disclaimers

> kebinImportsMongoose is not sponsored by or affiliated with Innersloth LLC, PlayEveryWare, or their affiliates. "Among Us" is a trademark or registered trademark of Innersloth LLC, PlayEveryWare, and/or its affiliates in the U.S. and elsewhere.  

> kebinImportsMongoose is not sponsored by or affiliated with Valve or its affiliates. "Steam" is a trademark or registered trademark of Valve and/or its affiliates in the U.S. and elsewhere.

> kebinImportsMongoose and its credited persons are not responsible for any of the actions committed by its dependencies.  

> kebinImportsMongoose and its credited persons do not intend any misuse of itself nor its dependencies.  

> kebinImportsMongoose will not assume the role of fixing its dependencies issues and reserves the right to terminate functionality through its credited persons authority.