# BlazorLibrary
A library for games, created using .NET Core 7.0 with MAUI (Blazor)!

## Technologies

### Created using
- MAUI (Blazor) on .NET Core 7.0

### Nuget(s)
- CsvHelper
- HtmlAgilityPack
- Microsoft.AspNetCore.Components.WebView.Maui
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.Extensions.Logging.Debug
- Microsoft.Maui.Graphics
- Microsoft.Maui.Graphics.Win2D.WinUI.Desktop
- Microsoft.Windows.SDK.BuildTools
- Microsoft.WindowsAppSDK
- [MmogaNet is coming](https://github.com/liebki/MMOGANet)
    - Pricing (If game is found)
- [MudBlazor](https://github.com/MudBlazor/MudBlazor)!
    - For the new design, simply everything from buttons to select's
- Newtonsoft.Json
- [RawgNet](https://github.com/liebki/RawgNET)
    - Where the game data comes from
- System.Data.SQLite
- [Html2Canvas JS](https://html2canvas.hertzen.com) (removed until I decide how to implement the print thing)
    - To use the print feature, nothing else is working besides some old JavaScript

## Features

### What is this?
- A external library of your games, with images, genre etc.

### General

#### Login
- Login or register a account (everything locally in sqlite!) to comment and have different games/genre visible per user

#### Games
- Add games with (name, description, picture (GIF's work), path to executeable and one or more genre)
- Edit and delete games
- Add games using .csv file import
- Recover games that you deleted from the trashcan or delete them completely

##### Gamescards
- Start games
- Favorize them
- Give them one or more genres
- Rate them using stars (1-5) and write a text for the rating
- Estimated prices, images and metacritic-values are visible (if game was found)
    - The prices are not accurate and may be wrong, should be more accurate with [MmogaNet](https://github.com/liebki/MMOGANet)
- The metacritic-values and pictures are directly from rawg

#### Genre
- Add, edit and delete genre
- Add genre using .csv file import

## Installation

1 Download the project files
2 Open the BlazorLibrary.sln file
3 Download the nuget packages
4 Start or build the project

## Installation for usage

You have to download the whole project, MAUI and visual studio are not able to produce a good exportable build right now yet

### Or download the project, build it using following command (still too big tho):
- dotnet publish -f net7.0-windows10.0.19041.0 -c Release -p:WindowsPackageType=None

## FAQ

#### Does this work on every OS?

I created this on windows 10 and tested it on other windows 10 machines, I cant guarantee anything for other operating systems or versions.

#### Where do I get an API-Key for rawg.io?

You can apply (get a key) here: https://rawg.io/apidocs

#### Where do I put my rawg.io API-Key and change settings?

- For now, look in the "ApplicationSettingsFile.json" file, under "rawgapikey", just paste it in the brackets, and it should work.
- Working on a userfriendly way

## License

**Software:** BlazorLibrary

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Screenshots

![Library Example Picture](https://kmliebl.de/blazorlibraryscreenshots/blazorlibrary-maui.PNG)

## Roadmap

- Internal cleaning and code reduction
- Import data from various sources, like databases etc.
- Better price querying [MmogaNet](https://github.com/liebki/MMOGANet)
- Reintroduce the print feature but better
- Maybe get the To-Do's directly of GitHub?
- Export the games, genres or everything
- Language files, to change language
- Integrate platforms like steam and such