using BlazorLibrary.Modelle;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class SpielBearbeiten
    {
        [Parameter]
        public string SpielId { get; set; }

        public IEnumerable<int> G { get; set; } = new HashSet<int>();
        public IEnumerable<Genre> AuswahlGenre { get; set; } = new HashSet<Genre>();
        public Genre[] GenreListe { get; set; } = Array.Empty<Genre>();
        public Spiel Spiel { get; set; }

        public async Task SelectExeFile()
        {
            FileResult file = await Manager.GetExecuteablePath();
            if (file != null)
            {
                Spiel.Exepfad = file.FullPath;
                StateHasChanged();
            }
        }

        private Func<Genre, string> GenreIdToGenreName = g => g?.Name;

        protected override async Task OnInitializedAsync()
        {
            await HoleDaten();
        }

        public async Task speichereDaten()
        {
            await _db.UpdateSpielInDatabase(Spiel);
            await _db.RemoveAllGenreOfGame(Spiel.Id);

            await _db.SaveGenreOfGame(Spiel.Id, AuswahlGenre);
            navMan.NavigateTo("/games", true);

            await Manager.MauiDialog("Information", $"You edited the game {Spiel.Name}");
        }

        private async Task HoleDaten()
        {
            Spiel spiel = await _db.SpielErhalten(int.Parse(SpielId));
            Genre[] genreList = await _db.AlleGenreErhalten(Manager.ActiveUser);

            if (spiel != null)
            {
                Spiel = spiel;
            }

            if (genreList.Length > 0)
            {
                GenreListe = genreList;
            }

            StateHasChanged();
        }
    }
}