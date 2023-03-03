using BlazorLibrary.Data;
using BlazorLibrary.Models;

using NavigationManagerUtils;
using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Components
{
    partial class TrashCard
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        [Inject]
        public NavManUtils NavMan { get; set; }

        [Parameter]
        public LibraryGame GameOfCard { get; set; }

        private async Task RecoverGame()
        {
            await DatabaseMan.ChangeDeletionStateOfGame(GameOfCard, false);
            NavMan.Navigate("/games");
        }

        private async Task DeleteGameFull()
        {
            await DatabaseMan.ClearGenresOfGame(GameOfCard.Id);
            await DatabaseMan.DeleteGame(GameOfCard);

            await DatabaseMan.DeleteComment(GameOfCard.Id, GameOfCard.Comment.Id);
            NavMan.Navigate("/games");
        }
    }
}