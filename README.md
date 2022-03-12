# BlazorLibrary
A library for video games on the computer, created using C#, Bootstrap and Electron.
## Technologies

### Created using
- Blazor-Server based on .NET Core 6.0
    - Bootstrap 4.3.1
    - JQuery 3.3.1
    - PopperJs 1.14.7

### Nuget(s)
- Blazored.Modal
- CsvHelper
- ElectronNet.API
- HtmlAgilityPack
- System.Data.SQlite

### External Sources (API's)
- MMOGA - Used for prices (Wrote some price scraper, not beautiful and not very accurate atm.)
- Rawg.io - Used for pictures and metacritic-value of games.
## Features

### General
- Scroll through all added games in your library
- Save created games and game genre in a sqlite file

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


## Roadmap

- Remove or upgrade print function
- Change icons of electron-shell
- Reduce or optimize code (in general)
- Change rating menu
- Enhance and integrate better mmoga scraper (for all languages)
- Language files, to change language
- Integrate platforms like steam and such
- More to come..
