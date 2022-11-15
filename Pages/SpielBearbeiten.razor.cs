using BlazorLibrary.Modelle;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
	partial class SpielBearbeiten
	{
		[Parameter]
		[SupplyParameterFromQuery]
		public string SpielId { get; set; }

		public int[] AuswahlGenre { get; set; } = { };
		public Genre[] GenreListe { get; set; }
		public Spiel Spiel { get; set; }

		public async Task SelectExeFile()
		{
			FileResult file = await Manager.GetExecuteablePath();
			if (file != null)
			{
				Spiel.Exepfad = file.FullPath;
				StateHasChanged();
			}
		}

		protected override async Task OnInitializedAsync()
		{
			await HoleDaten();
		}

		public async Task speichereDaten()
		{
			await _db.UpdateSpielInDatabase(Spiel);
			await _db.RemoveAllGenreOfGame(Spiel.Id);

			await _db.SaveGenreOfGame(Spiel.Id, AuswahlGenre);
			navMan.NavigateTo($"/?Nachricht=You edited the game {Spiel.Name}", true);
		}

		private async Task HoleDaten()
		{
			Spiel spiel = await _db.SpielErhalten(Int32.Parse(SpielId));
			Genre[] genreList = await _db.AlleGenreErhalten();

			if (!object.Equals(null, spiel) && !object.Equals(null, genreList))
			{
				Spiel = spiel;
				GenreListe = genreList;

				StateHasChanged();
			}
		}
	}
}