# BlazorLibrary
A library for games, created using .NET Core 7.0 with MAUI (Blazor)!

## Technologies

### Created using
- MAUI (Blazor) on .NET Core 7.0

### Nuget(s)
- Microsoft.Windows.SDK.BuildTools
- Microsoft.AspNetCore.Components.WebView.Maui
- Microsoft.Extensions.DependencyInjection   
- HtmlAgilityPack   
- System.Data.SQLite 
- Microsoft.Extensions.Logging
- CsvHelper   
- Microsoft.Extensions.Logging.Abstractions     
- Newtonsoft.Json 
- Microsoft.WindowsAppSDK     
- Microsoft.Maui.Graphics.Win2D.WinUI.Desktop  
- Microsoft.Maui.Graphics   
- Microsoft.Extensions.Logging.Debug 
- Microsoft.Extensions.Configuration
- https://html2canvas.hertzen.com
    - To use the print feature, nothing else is working besides some old JavaScript
- [MudBlazor](https://github.com/MudBlazor/MudBlazor)!
    - For the new design, simply everything from buttons to select's
- [RawgNet](https://github.com/liebki/RawgNET)
    - Where the game data comes from
- [MmogaNet is coming](https://github.com/liebki/MMOGANet)
    - Pricing (If game is found)

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
- Settings, to change various things (ApplicationSettingsFile.json)
- Being able to comment, not the review like a bigger text to comment a game
- Import data from various sources, like databases etc.
- Better price querying [MmogaNet](https://github.com/liebki/MMOGANet)
- Reintroduce the print feature but better
- Maybe get the To-Do's directly of GitHub?
- Export the games, genres or everything
- Language files, to change language
- Integrate platforms like steam and such