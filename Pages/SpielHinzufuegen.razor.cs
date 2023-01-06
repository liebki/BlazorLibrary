using BlazorLibrary.Management;
using BlazorLibrary.Modelle;

namespace BlazorLibrary.Pages
{
    partial class SpielHinzufuegen
    {
        public int[] AuswahlGenre { get; set; } = { };
        public IEnumerable<Genre> GenreAuswahl { get; set; } = new List<Genre>();
        public Genre[] GenreListe { get; set; }
        private string Name { get; set; }
        private string Beschreibung { get; set; } = string.Empty;
        private string Bildlink { get; set; }
        private string Exepfad { get; set; } = string.Empty;

        public async Task SelectExeFile()
        {
            FileResult file = await Manager.GetExecuteablePath();
            if (file != null)
            {
                Exepfad = file.FullPath;
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await HoleDaten();
        }

        public async Task speichereDaten()
        {
            if (!string.IsNullOrEmpty(Name) && Name.Length > 1)
            {
                int SpielId = await _db.CreateGameInDatabase(Name, Manager.ActiveUser, Beschreibung, Bildlink, Exepfad);
                await _db.RemoveAllGenreOfGame(SpielId);

                if (GenreAuswahl.Any())
                {
                    await _db.SaveGenreOfGame(SpielId, GenreAuswahl);
                }
                await Manager.MauiDialog("Information", $"You created the game {Name}");
                navMan.NavigateTo("/games", true);
            }
            else
            {
                await Manager.MauiDialog("Information", $"An error occured, the name is NOT optional!");
            }
        }

        private Func<Genre, string> GenreIdToGenreName = g => g?.Name;

        private async Task HoleDaten()
        {
            Genre[] genreList = await _db.AlleGenreErhalten(Manager.ActiveUser);
            if (genreList != null)
            {
                GenreListe = genreList;
                StateHasChanged();
            }
        }
    }
}