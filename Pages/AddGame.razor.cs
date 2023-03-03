using BlazorLibrary.Data;
using BlazorLibrary.Models;
using NavigationManagerUtils;

using BlazorLibrary.Management;
using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class AddGame
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        [Inject]
        public NavManUtils NavMan { get; set; }

        public IEnumerable<LibraryGenre> GenreForGame { get; set; } = new List<LibraryGenre>();
        public LibraryGenre[] AvailableGenres { get; set; }
        private string Name { get; set; }
        private string Description { get; set; } = string.Empty;
        private string ImageUrl { get; set; }
        private string ExecutablePath { get; set; } = string.Empty;

        public async Task ExecutableDialog()
        {
            (string path, bool successfull) = await Manager.OpenSelectExecutableDialog();
            if (successfull)
            {
                ExecutablePath = path;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await CollectData();
        }

        public async Task SaveGame()
        {
            if (!string.IsNullOrEmpty(Name) && Name.Length > 1)
            {
                int GameId = await DatabaseMan.InsertNewGame(Name, Manager.ActiveLibraryUser, Description, ImageUrl, ExecutablePath);
                await DatabaseMan.ClearGenresOfGame(GameId);

                if (GenreForGame.Any())
                {
                    await DatabaseMan.SaveGenresOfGame(GameId, GenreForGame);
                }
                await Manager.SimpleDialogMessage("Information", $"You created the game {Name}");
                NavMan.Navigate("/games");
            }
            else
            {
                await Manager.SimpleDialogMessage("Information", $"An error occured, the name is NOT optional!");
            }
        }

        private Func<LibraryGenre, string> GenreIdToGenreName = g => g?.Name;

        private async Task CollectData()
        {
            LibraryGenre[] Genrelist = await DatabaseMan.GetUserrelatedGenreEntries(Manager.ActiveLibraryUser);
            if (Genrelist != null)
            {
                AvailableGenres = Genrelist;
                StateHasChanged();
            }
        }
    }
}