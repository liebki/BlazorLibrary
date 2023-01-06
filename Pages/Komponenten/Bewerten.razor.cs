using Blazored.Modal;
using Blazored.Modal.Services;

using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class Bewerten
    {
        private string berwertungGrund { get; set; } = string.Empty;
        private Bewertung sterneBewertung { get; set; } = new(0, string.Empty);
        [CascadingParameter] private BlazoredModalInstance BlazoredModal { get; set; }

        private async Task FormBeenden()
        {
            sterneBewertung = new(sterneBewertung.Sterneanzahl, berwertungGrund);
            await BlazoredModal.CloseAsync(ModalResult.Ok(sterneBewertung));
        }
    }
}