using Blazored.Modal;
using BlazorLibrary.Modelle;

using Blazored.Modal.Services;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class Bewerten
    {
        private string bewertungtooltip { get; set; } = string.Empty;
        private Bewertung sterneBewertung { get; set; } = new(0, string.Empty);
        [CascadingParameter] private BlazoredModalInstance BlazoredModal { get; set; }
        private int auswahl { get; set; } = 0;

        protected override void OnInitialized() => BlazoredModal.SetTitle("Rate the game");

        public void setzeSterne(int sterne)
        {
            auswahl = sterne;
        }

        private async Task FormBeenden()
        {
            sterneBewertung = new(auswahl, bewertungtooltip);
            await BlazoredModal.CloseAsync(ModalResult.Ok(sterneBewertung));
        }
    }
}