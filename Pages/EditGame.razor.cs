using BlazorLibrary.Data;
using BlazorLibrary.Models;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class EditGame
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        [Inject]
        public NavigationManager NavigationMan { get; set; }

        [Parameter]
        public string GameId { get; set; }

        public IEnumerable<LibraryGenre> GenresToAdd { get; set; } = new HashSet<LibraryGenre>();
        public LibraryGenre[] AvailableGenres { get; set; } = Array.Empty<LibraryGenre>();
        public LibraryGame GameToUpdate { get; set; }
        private Func<LibraryGenre, string> GenreIdToGenreName = g => g?.Name;

        public async Task ExecutableDialog()
        {
            (string path, bool successfull) = await Manager.OpenSelectExecutableDialog();
            if (successfull)
            {
                GameToUpdate.ExecutablePath = path;
            }
        }

        public async Task SaveData()
        {
            await DatabaseMan.UpdateGame(GameToUpdate);
            await DatabaseMan.ClearGenresOfGame(GameToUpdate.Id);

            await DatabaseMan.SaveGenresOfGame(GameToUpdate.Id, GenresToAdd);
            NavigationMan.NavigateTo("/games", true);

            await Manager.SimpleDialogMessage("Information", $"You edited the game {GameToUpdate.Name}");
        }

        protected override async Task OnInitializedAsync()
        {
            await CollectData();
        }

        private async Task CollectData()
        {
            LibraryGame GameToEdit = await DatabaseMan.GetGameById(int.Parse(GameId));
            LibraryGenre[] Genrelist = await DatabaseMan.GetUserrelatedGenreEntries(Manager.ActiveLibraryUser);

            if (GameToEdit != null)
            {
                GameToUpdate = GameToEdit;
            }

            if (Genrelist.Length > 0)
            {
                AvailableGenres = Genrelist;
            }

            StateHasChanged();
        }
    }
}