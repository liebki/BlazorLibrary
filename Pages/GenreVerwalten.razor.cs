
using BlazorLibrary.Modelle;

using Blazored.Modal.Services;
using BlazorLibrary.Pages.Komponenten;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
	partial class GenreVerwalten
	{
		[Parameter]
		[SupplyParameterFromQuery]
		public string Nachricht { get; set; } = string.Empty;

		[CascadingParameter] public IModalService Modal { get; set; }

		private void ShowModal() => Modal.Show<Confirm>(Nachricht);

		public string NeuesGenre { get; set; }
		public int[] auswahlgenre { get; set; } = { };
		public Genre editgenre { get; set; }
		public Genre[] GenreListe { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await HoleDaten();
			if (!String.IsNullOrEmpty(Nachricht))
			{
				ShowModal();
			}
		}

		public int[] AuswahlGenre
		{
			get
			{
				return auswahlgenre;
			}
			set
			{
				auswahlgenre = value;
				if (AuswahlGenre.Length > 0)
				{
					editgenre = _db.BestimmtesGenreErhalten(auswahlgenre[0]);
				}
			}
		}

		public async Task AendereGenre()
		{
			await _db.RenameGenre(editgenre);
			navMan.NavigateTo($"/genreverwalten?Nachricht=You renamed a genre to {editgenre.Name}", true);

			await HoleDaten();
		}

		public async Task ErstelleGenre()
		{
			if (!(NeuesGenre.Length > 0 && string.IsNullOrWhiteSpace(NeuesGenre)))
			{
				await _db.CreateGenreInDatabase(NeuesGenre);
				navMan.NavigateTo($"/genreverwalten?Nachricht=You created the genre {NeuesGenre}", true);

				NeuesGenre = string.Empty;
				await HoleDaten();
			}
		}

		public async Task LoescheGenre()
		{
			int GenreAnzahlWahl = AuswahlGenre.Count();
			if (GenreAnzahlWahl >= 0)
			{
				bool konflikt = await _db.SpielGenreCheck(AuswahlGenre);
				if (!konflikt)
				{
					string msg = "You deleted the genre ";
					if (GenreAnzahlWahl > 1)
					{
						msg = "You deleted the genre ";
					}

					Genre[] liste = await _db.AlleGenreErhalten();
					string GenreNamen = string.Empty;

					foreach (int i in AuswahlGenre)
					{
						foreach (Genre genre in liste)
						{
							if (genre.Id.Equals(i))
							{
								GenreNamen += $"{genre.Name} ";
							}
						}
					}
					await _db.DeleteGenreInDatabase(AuswahlGenre);
					navMan.NavigateTo($"/genreverwalten?Nachricht={msg}{GenreNamen}", true);

					await HoleDaten();
				}
				else
				{
					navMan.NavigateTo("/genreverwalten?Nachricht=Some game(s) still use one or more genre!", true);
				}
			}
		}

		private async Task HoleDaten()
		{
			Genre[] genreList = await _db.AlleGenreErhalten();
			if (!object.Equals(null, genreList))
			{
				GenreListe = genreList;
				StateHasChanged();
			}
		}
	}
}