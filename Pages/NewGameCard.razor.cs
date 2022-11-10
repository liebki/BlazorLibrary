using Blazored.Modal;
using BlazorLibrary.Modelle;

using Blazored.Modal.Services;
using BlazorLibrary.Pages.Komponenten;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class NewGameCard
    {
        [Parameter]
        public Spiel spiel { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }
        private Bewertung Bewertung;

        private async Task SpielLoeschen()
        {
            await _db.SetSpielDeleteStatus(spiel, true);
            navMan.NavigateTo("/anzeige", true);
        }

        private async Task FavoritSetzen(bool wert)
        {
            await _db.SpielFavoritSetzen(spiel.Id, wert);
            if (wert)
            {
                spiel.Fav = 1;
            }
            else
            {
                spiel.Fav = 0;
            }
            StateHasChanged();
        }

        private async Task ShowModal()
        {
            IModalReference messageForm = Modal.Show<Bewerten>();
            ModalResult result = await messageForm.Result;

            if (!result.Cancelled)
            {
                Bewertung = (Bewertung)result.Data;
                spiel.Sterne = Bewertung.Sterneanzahl;

                spiel.SterneTooltip = Bewertung.Sternebewertung;
                await _db.UpdateSterneForGame(spiel);

                StateHasChanged();
            }
        }
    }
}