using MudBlazor;

using BlazorLibrary.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorLibrary.Data;

namespace BlazorLibrary.Pages.Components
{
    partial class GameCard
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        [Inject]
        public NavigationManager NavigationMan { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }
        [Parameter]

        public LibraryGame GameOfCard { get; set; }

        private async Task MoveGameToTrashcan()
        {
            await DatabaseMan.ChangeDeletionStateOfGame(GameOfCard, true);
            NavigationMan.NavigateTo("/games", true);
        }

        private async Task ChangeFavouriteState(bool wert)
        {
            await DatabaseMan.ChangeFavouriteStateOfGame(GameOfCard.Id, wert);
            if (wert)
            {
                GameOfCard.IsFavourite = 1;
            }
            else
            {
                GameOfCard.IsFavourite = 0;
            }
            StateHasChanged();
        }

        private async Task ShowCommentModal()
        {
            DialogParameters CommentModalParameter = new()
            {
                { "CommentText", GameOfCard.Comment.Text }
            };

            IDialogReference CommentModal = await DialogService.ShowAsync<CommentDialog>("Comment the game", CommentModalParameter);
            DialogResult CommentModalResult = await CommentModal.Result;

            if (!CommentModalResult.Canceled)
            {
                (string Text, bool ToDelete) Comment = ((string, bool))CommentModalResult.Data;
                if (!GameOfCard.Comment.Commented)
                {
                    int CommentId = await DatabaseMan.SaveComment(GameOfCard.Id, Comment.Text);
                    GameOfCard.Comment = (Comment.Text, true, CommentId);
                }
                else
                {
                    if (Comment.ToDelete)
                    {
                        await DatabaseMan.DeleteComment(GameOfCard.Id, GameOfCard.Comment.Id);
                        GameOfCard.Comment = (string.Empty, false, 0);
                    }
                    else
                    {
                        (string Text, bool Gesetzt, int Id) NewComment = (Comment.Text, GameOfCard.Comment.Commented, GameOfCard.Comment.Id);

                        await DatabaseMan.UpdateCommentEntry(NewComment);
                        GameOfCard.Comment = NewComment;
                    }
                }
                StateHasChanged();
            }
        }

        private async Task ShowReviewModal()
        {
            IDialogReference ReviewModal = await DialogService.ShowAsync<ReviewDialog>("Review the game");
            DialogResult ReviewModalResult = await ReviewModal.Result;

            if (!ReviewModalResult.Canceled)
            {
                Review ReviewToUse = (Review)ReviewModalResult.Data;
                GameOfCard.ReviewStars = ReviewToUse.Stars;

                GameOfCard.ReviewText = ReviewToUse.Reason;
                await DatabaseMan.UpdateGameReview(GameOfCard);

                StateHasChanged();
            }
        }

        private string HrefGameEditUrl()
        {
            return $"/spielbearbeiten/{GameOfCard.Id}";
        }
    }
}