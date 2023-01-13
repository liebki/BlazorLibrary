
using MudBlazor;

using BlazorLibrary.Models;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Components
{
    partial class ReviewDialog
    {
        private string ReviewReasonText { get; set; } = string.Empty;
        private Review ReviewToUse { get; set; } = new(0, string.Empty);

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void GiveReview()
        {
            ReviewToUse = new(ReviewToUse.Stars, ReviewReasonText);
            MudDialog.Close(DialogResult.Ok(ReviewToUse));
        }
    }
}