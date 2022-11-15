using BlazorLibrary.Modelle;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
	partial class DeletedGameCard
	{
		[Parameter]
		public Spiel SelectedGame { get; set; }

		private async Task SpielWiederherstellen()
		{
			await _db.SetSpielDeleteStatus(SelectedGame, false);
			navMan.NavigateTo("/anzeige", true);
		}

		private async Task SpielEndgueltigLoeschen()
		{
			await _db.RemoveAllGenreOfGame(SelectedGame.Id);
			await _db.DeleteSpielCompletely(SelectedGame);

			navMan.NavigateTo("/anzeige", true);
		}
	}
}