using System.Text;

using BlazorLibrary.Data;
using BlazorLibrary.Models;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class Genres
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        [Inject]
        public NavigationManager NavigationMan { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public string NewGenreName { get; set; }

        public LibraryGenre GenreToUpdate { get; set; }
        public LibraryGenre[] AvailableGenres { get; set; }
        private readonly Func<LibraryGenre, string> GenreIdToGenreName = g => g?.Name;
        private IEnumerable<LibraryGenre> selectedgenre { get; set; } = new HashSet<LibraryGenre>();

        private IEnumerable<LibraryGenre> SelectedGenres
        {
            get
            {
                return selectedgenre;
            }
            set
            {
                selectedgenre = value;
                if (SelectedGenres.Count() == 1)
                {
                    GenreToUpdate = SelectedGenres.First();
                }
            }
        }

        public async Task UpdateGenre()
        {
            await DatabaseMan.RenameGenreEntry(GenreToUpdate);
            await Manager.SimpleDialogMessage("Information", $"You renamed the genre to {GenreToUpdate.Name}");

            NavigationMan.NavigateTo("/genres", true);
        }

        public async Task CreateGenre()
        {
            if (!(NewGenreName.Length > 0 && string.IsNullOrWhiteSpace(NewGenreName)))
            {
                await DatabaseMan.InsertGenreEntry(NewGenreName, Manager.ActiveLibraryUser);
                await Manager.SimpleDialogMessage("Information", $"You created the genre {NewGenreName}");

                NewGenreName = string.Empty;
                await CollectData();
            }
        }

        public async Task DeleteGenres()
        {
            int SelectedGenresCount = SelectedGenres.Count();
            if (SelectedGenresCount >= 0)
            {
                bool IsGenreInUse = await DatabaseMan.AreGenresUsedByGames(SelectedGenres, Manager.ActiveLibraryUser);
                if (!IsGenreInUse)
                {
                    string msg = "You deleted the genre";
                    if (SelectedGenresCount > 1)
                    {
                        msg += "s";
                    }

                    LibraryGenre[] Genrelist = await DatabaseMan.GetUserrelatedGenreEntries(Manager.ActiveLibraryUser);
                    StringBuilder GenreNamen = new();

                    foreach (LibraryGenre g in SelectedGenres)
                    {
                        foreach (LibraryGenre genre in Genrelist)
                        {
                            if (genre.Id.Equals(g))
                            {
                                GenreNamen.Append(g.Name);
                            }
                        }
                    }
                    await DatabaseMan.DeleteGenreInDatabase(SelectedGenres);
                    await Manager.SimpleDialogMessage("Information", $"{msg} {GenreNamen}");

                    NavigationMan.NavigateTo("/genres", true);
                    await CollectData();
                }
                else
                {
                    await Manager.SimpleDialogMessage("Information", "Some game(s) still use one or more genre(s), could not delete!");
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await CollectData();
        }

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