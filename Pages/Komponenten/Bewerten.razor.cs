using System.Threading.Tasks;

using Blazored.Modal;
using Blazored.Modal.Services;

using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class Bewerten
    {
        private string bewertungtooltip { get; set; } = "";
        private Bewertung sterneBewertung { get; set; } = new(0, "");
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