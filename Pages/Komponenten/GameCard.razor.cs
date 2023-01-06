using Blazored.Modal;
using Blazored.Modal.Services;

using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class GameCard
    {
        [Parameter]
        public Spiel SpielKarte { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }
        private Bewertung bewertung;

        private async Task SpielLoeschen()
        {
            await _db.SetSpielDeleteStatus(SpielKarte, true);
            navMan.NavigateTo("/games", true);
        }

        private async Task FavoritSetzen(bool wert)
        {
            await _db.SpielFavoritSetzen(SpielKarte.Id, wert);
            if (wert)
            {
                SpielKarte.Fav = 1;
            }
            else
            {
                SpielKarte.Fav = 0;
            }
            StateHasChanged();
        }

        private async Task ShowModal()
        {
            IModalReference messageForm = Modal.Show<Bewerten>("Give a review");
            ModalResult result = await messageForm.Result;

            if (!result.Cancelled)
            {
                bewertung = (Bewertung)result.Data;
                SpielKarte.Sterne = bewertung.Sterneanzahl;

                SpielKarte.SterneTooltip = bewertung.Grund;
                await _db.UpdateSterneForGame(SpielKarte);

                StateHasChanged();
            }
        }

        private string HrefGameEditUrl()
        {
            return $"/spielbearbeiten/{SpielKarte.Id}";
        }
    }
}