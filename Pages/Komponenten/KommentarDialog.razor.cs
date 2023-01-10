
using MudBlazor;

using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
    partial class KommentarDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string CommentText { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void SaveComment()
        {   
            if(string.IsNullOrEmpty(CommentText))
            {
                CommentText = "";
            }
            MudDialog.Close(DialogResult.Ok((CommentText, false)));
        }
        private void DeleteComment()
        {
            MudDialog.Close(DialogResult.Ok(("", true)));
        }

    }
}