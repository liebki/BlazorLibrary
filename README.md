# BlazorLibrary
A library for video games on the computer, created using .NET Core 7.0 with MAUI and Blazor!

## Technologies

### Created using
- Blazor-Server based on .NET Core 7.0
    - Bootstrap 4.3.1
    - JQuery 3.3.1
    - PopperJs 1.14.7

### Nuget(s)
Microsoft.Windows.SDK.BuildTools             10.0.22621.1
Microsoft.AspNetCore.Components.WebView.Maui 7.0.49      
Blazored.Modal                               7.1.0       
Microsoft.Extensions.DependencyInjection     7.0.0       
HtmlAgilityPack                              1.11.46     
System.Data.SQLite                           1.0.116     
Microsoft.Extensions.Logging                 7.0.0       
CsvHelper                                    30.0.0      
Microsoft.Extensions.Logging.Abstractions    7.0.0       
Newtonsoft.Json                              13.0.1      
Microsoft.WindowsAppSDK                      1.1.5       
Microsoft.Maui.Graphics.Win2D.WinUI.Desktop  7.0.49      
Microsoft.Maui.Graphics                      7.0.49      
Microsoft.Extensions.Logging.Debug           7.0.0       
Microsoft.Extensions.Configuration           7.0.0    

### External Sources (API's)
- MMOGA 
    - Pricing (If game is found)
- Rawg.io 
    - Pictures and metacritic-value (If game is found)

## Features

### General
- Keep a library of your games, with images, genre etc.

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
    - Print the game (Not very beautiful, but it works)
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
- Start the BlazorApp.csproj file
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

![Library Example Picture](https://kmliebl.de/blazorlibraryscreenshots/library-ow.png)
(Old, before MAUI port, new one will follow if everything works as planned)

## Roadmap

1.
- Wishlist
- Change API-Key, etc. basically edit config while app is running in an interface!

2.
- Remove or upgrade print function (hell on earth in maui, I can tell you)
- Reduce or optimize code (in general) (with maui?)
- Change rating menu
- Integrate mmoga and rawg api (now that I created those two)
- Language files, to change language
- Integrate platforms like steam and such