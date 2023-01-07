
using MudBlazor;

using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class GameCard
    {
        [Parameter]
        public Spiel SpielKarte { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

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
            IDialogReference dialog = await DialogService.ShowAsync<Bewerten>("Review the game");
            DialogResult result = await dialog.Result;

            if (!result.Canceled)
            {
                Bewertung spieleBewertung = (Bewertung)result.Data;
                SpielKarte.Sterne = spieleBewertung.Sterneanzahl;

                SpielKarte.SterneTooltip = spieleBewertung.Grund;
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