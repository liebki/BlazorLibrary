
using MudBlazor;

using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class Bewerten
    {
        private string berwertungGrund { get; set; } = string.Empty;
        private Bewertung sterneBewertung { get; set; } = new(0, string.Empty);

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void GiveReview()
        {
            sterneBewertung = new(sterneBewertung.Sterneanzahl, berwertungGrund);
            MudDialog.Close(DialogResult.Ok(sterneBewertung));
        }

    }
}