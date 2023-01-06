# BlazorLibrary
A library for games, created using .NET Core 7.0 with MAUI (Blazor)!

## Technologies

### Created using
- MAUI (Blazor) on .NET Core 7.0

### Nuget(s)
- Microsoft.Windows.SDK.BuildTools
- Microsoft.AspNetCore.Components.WebView.Maui
- Blazored.Modal
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
- [MudBlazor](https://github.com/MudBlazor/MudBlazor)!
- [RawgNet](https://github.com/liebki/RawgNET)


### External Sources (API's)
- MMOGA [MmogaNet is coming](https://github.com/liebki/MMOGANet)
    - Pricing (If game is found)
- Rawg.io ([RawgNet](https://github.com/liebki/RawgNET))
    - Pictures and metacritic-value (If game is found)

## Features

### General
- A library of your games, with images, genre etc.

### Login
- Login or register a account (everything locally in sqlite!) to comment and have different games/genre visible per user

### Games
- Add games with (name, description, picture (GIF's work), path to executeable and one or more genre)
- Edit and delete games
- Add games using .csv file import
- Recover games that you deleted from the trashcan or delete them completely

#####
- Gamescards (Detailed)
    - Start games using the executeable
    - Favorize them
    - Rate them using stars (1-5) and write a text for the rating
#####
- Games will be displayed with prices, image and metacritic-value (if game is found)
    - The prices are not very accurate and may be wrong, I'm still working on a new and better scraper.
    - The metacritic-values and pictures are directly from the rawg.io api.

### Genre
- Add genre
- Edit and delete genre
- Add genre using .csv file import

## Installation

- Download the project files
- Open the BlazorLibrary.sln file
- Download the nuget packages
- Start or build the project

## FAQ

#### Does this work on every OS?

I created this on windows 10 and tested it on other windows 10 machines, I cant guarantee anything for other operating systems or versions.

#### Where do I get an API-Key for rawg.io?

You can apply (get a key) here: https://rawg.io/apidocs

#### Where do I put my rawg.io API-Key?

Look in the "ApplicationSettingsFile.json" file, under "rawgapikey", just paste it in the brackets, and it should work.

## License

“Commons Clause” License Condition v1.0

The Software is provided to you by the Licensor under the License, as defined below, subject to the following condition.

Without limiting other conditions in the License, the grant of rights under the License will not include, and the License does not grant to you, the right to Sell the Software.

For purposes of the foregoing, “Sell” means practicing any or all of the rights granted to you under the License to provide to third parties, for a fee or other consideration (including without limitation fees for hosting or consulting/ support services related to the Software), a product or service whose value derives, entirely or substantially, from the functionality of the Software. Any license notice or attribution required by the License must also include this Commons Clause License Condition notice.

**Software:** BlazorLibrary

**License:** Apache License 2.0

**Licensor:** Kim Mario Liebl

[commonsclause](https://commonsclause.com/)
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