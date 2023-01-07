using System.Text;

using BlazorLibrary.Modelle;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class GenreVerwalten
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string NeuesGenre { get; set; }
        public Genre editgenre { get; set; }
        public Genre[] GenreListe { get; set; }
        private IEnumerable<Genre> auswahlGenre { get; set; } = new HashSet<Genre>();

        private IEnumerable<Genre> AuswahlGenre
        {
            get
            {
                return auswahlGenre;
            }
            set
            {
                auswahlGenre = value;
                if (AuswahlGenre.Count() == 1)
                {
                    editgenre = AuswahlGenre.First();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await HoleDaten();
        }

        private Func<Genre, string> GenreIdToGenreName = g => g?.Name;

        public async Task AendereGenre()
        {
            await _db.RenameGenre(editgenre);
            await Manager.MauiDialog("Information", $"You renamed the genre to {editgenre.Name}");

            await HoleDaten();
        }

        public async Task ErstelleGenre()
        {
            if (!(NeuesGenre.Length > 0 && string.IsNullOrWhiteSpace(NeuesGenre)))
            {
                await _db.CreateGenreInDatabase(NeuesGenre, Manager.ActiveUser);
                await Manager.MauiDialog("Information", $"You created the genre {NeuesGenre}");

                NeuesGenre = string.Empty;
                await HoleDaten();
            }
        }

        public async Task LoescheGenre()
        {
            int GenreAnzahlWahl = AuswahlGenre.Count();
            if (GenreAnzahlWahl >= 0)
            {
                bool konflikt = await _db.SpielGenreCheck(AuswahlGenre, Manager.ActiveUser);
                if (!konflikt)
                {
                    string msg = "You deleted the genre";
                    if (GenreAnzahlWahl > 1)
                    {
                        msg += "s";
                    }

                    Genre[] liste = await _db.AlleGenreErhalten(Manager.ActiveUser);
                    StringBuilder GenreNamen = new();

                    foreach (Genre g in AuswahlGenre)
                    {
                        foreach (Genre genre in liste)
                        {
                            if (genre.Id.Equals(g))
                            {
                                GenreNamen.Append(g.Name);
                            }
                        }
                    }
                    await _db.DeleteGenreInDatabase(AuswahlGenre);
                    await Manager.MauiDialog("Information", $"{msg} {GenreNamen}");

                    navMan.NavigateTo("/genres", true);
                    await HoleDaten();
                }
                else
                {
                    await Manager.MauiDialog("Information", "Some game(s) still use one or more genre(s), could not delete!");
                }
            }
        }

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