
using MudBlazor;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Components
{
    partial class CommentDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public string CommentText { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void SaveComment()
        {
            if (string.IsNullOrEmpty(CommentText))
            {
                CommentText = string.Empty;
            }
            MudDialog.Close(DialogResult.Ok((CommentText, false)));
        }

        private void DeleteComment()
        {
            MudDialog.Close(DialogResult.Ok((string.Empty, true)));
        }
    }
}