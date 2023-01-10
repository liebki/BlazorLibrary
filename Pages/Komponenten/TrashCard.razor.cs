using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class TrashCard
    {
        [Parameter]
        public Spiel SpielKarte { get; set; }

        private async Task SpielWiederherstellen()
        {
            await _db.SetSpielDeleteStatus(SpielKarte, false);
            navMan.NavigateTo("/games", true);
        }

        private async Task SpielEndgueltigLoeschen()
        {
            await _db.RemoveAllGenreOfGame(SpielKarte.Id);
            await _db.DeleteSpielCompletely(SpielKarte);

            await _db.DeleteComment(SpielKarte.Id, SpielKarte.Kommentar.Id);
            navMan.NavigateTo("/games", true);
        }
    }
}