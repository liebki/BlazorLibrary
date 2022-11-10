﻿using System.Threading.Tasks;

using BlazorLibrary.Management;
using BlazorLibrary.Modelle;

namespace BlazorLibrary.Pages
{
    partial class SpielHinzufuegen
    {
        public int[] AuswahlGenre { get; set; } = { };
        public Genre[] GenreListe { get; set; }
        private string Name { get; set; }
        private string Beschreibung { get; set; } = "";
        private string Bildlink { get; set; }
        private string Exepfad { get; set; } = "";

        public async Task SelectExeFile()
        {
            FileResult file = await Manager.GetExecuteablePath();
            if (file != null)
            {
                Exepfad = file.FullPath;
                StateHasChanged();
            }
            //string dateiPfad = await fileMan.DateiDialogOeffnen();
            //if (!string.IsNullOrEmpty(dateiPfad))
            //{
            //    Exepfad = dateiPfad;
            //    StateHasChanged();
            //}
        }

        protected override async Task OnInitializedAsync()
        {
            await HoleDaten();
        }

        public async Task speichereDaten()
        {
            if (!object.Equals(Name, null) && Name.Length > 1)
            {
                int SpielId = await _db.CreateGameInDatabase(Name, Beschreibung, Bildlink, Exepfad);
                await _db.RemoveAllGenreOfGame(SpielId);

                if (AuswahlGenre.Length > 0)
                {
                    await _db.SaveGenreOfGame(SpielId, AuswahlGenre);
                }

                navMan.NavigateTo("/?Nachricht=You created the game " + Name, true);
            }
            else
            {
                navMan.NavigateTo("/?Nachricht=An error occured!", true);
            }
        }

        private async Task HoleDaten()
        {
            Genre[] genreList = await _db.AlleGenreErhalten();
            if (!object.Equals(null, genreList))
            {
                GenreListe = genreList;
                StateHasChanged();
            }
        }
    }
}