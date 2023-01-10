
using MudBlazor;

using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;
using System.ComponentModel.Design;

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

        private async Task ShowKommentarModal()
        {
            DialogParameters kommentarTextPara = new()
            {
                { "CommentText", SpielKarte.Kommentar.Text }
            };

            IDialogReference dialog = await DialogService.ShowAsync<KommentarDialog>("Comment the game", kommentarTextPara);
            DialogResult result = await dialog.Result;

            if (!result.Canceled)
            {
                (string Text, bool ToDelete) Comment = ((string, bool))result.Data;
                if(!SpielKarte.Kommentar.Commented)
                {
                    int CommentId = await _db.SaveSpielComment(SpielKarte.Id, Comment.Text);
                    SpielKarte.Kommentar = (Comment.Text, true, CommentId);
                } 
                else
                {
                    if(Comment.ToDelete)
                    {
                        await _db.DeleteComment(SpielKarte.Id, SpielKarte.Kommentar.Id);
                        SpielKarte.Kommentar = ("", false, 0);
                    } else
                    {
                        (string Text, bool Gesetzt, int Id) NewComment = (Comment.Text, SpielKarte.Kommentar.Commented, SpielKarte.Kommentar.Id);

                        await _db.UpdateComment(NewComment);
                        SpielKarte.Kommentar = NewComment;
                    }

                }
                StateHasChanged();
            }
        }

        private async Task ShowBewertenModal()
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