using BlazorLibrary.Data;
using BlazorLibrary.Models;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Components
{
    partial class TrashCard
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        [Inject]
        public NavigationManager NavigationMan { get; set; }

        [Parameter]
        public LibraryGame GameOfCard { get; set; }

        private async Task RecoverGame()
        {
            await DatabaseMan.ChangeDeletionStateOfGame(GameOfCard, false);
            NavigationMan.NavigateTo("/games", true);
        }

        private async Task DeleteGameFull()
        {
            await DatabaseMan.ClearGenresOfGame(GameOfCard.Id);
            await DatabaseMan.DeleteGame(GameOfCard);

            await DatabaseMan.DeleteComment(GameOfCard.Id, GameOfCard.Comment.Id);
            NavigationMan.NavigateTo("/games", true);
        }
    }
}