
using Newtonsoft.Json;

using BlazorLibrary.Modelle;

namespace BlazorLibrary.Data
{
	public class RawgAccessManager
	{
		private const string gameApiBaseLink = "https://rawg.io/api/games/";
		private static readonly string rawgApiKey = MauiProgram.Einstellungen.Rawgapikey;

		public Game RawgGameInformations(string name)
		{
			Game game = null;
			Task<Game> g = Task.Run(() => GameQuery(name));

			g.Wait();
			if (object.Equals(g.Result.BackgroundImage, null))
			{
				Task<GameFallback> FallBackInformations = Task.Run(() => FallBackQuery(name));
				FallBackInformations.Wait();

				if (FallBackInformations.Result.Redirect)
				{
					Task<Game> SecondTry = Task.Run(() => GameQuery(FallBackInformations.Result.Slug));
					g.Wait();

					g = SecondTry;
				}
			}
			if (!object.Equals(g.Result.BackgroundImage, null))
			{
				game = g.Result;
				if (object.Equals(game.Metacritic, null))
				{
					game.Metacritic = 0;
				}
			}
			return game;
		}

		public async Task<GameFallback> FallBackQuery(string gamename)
		{
			string GameName = gamename;
			GameName = GameName.ToLower();

			if (gamename.Contains(' '))
			{
				GameName = GameName.Replace(" ", "-");
			}
			string reqUrl = $"{gameApiBaseLink}{GameName}?search_precise=false&search_exact=false&key={rawgApiKey}";
			string json = string.Empty;

			using (HttpClient client = new())
			{
				Task<HttpResponseMessage> response = client.GetAsync(reqUrl);
				response.Wait();

				json = await response.Result.Content.ReadAsStringAsync();
			}
			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}
			return DeserializeGameFallbackJson(json);
		}

		public async Task<Game> GameQuery(string gamename)
		{
			string GameName = gamename;
			GameName = GameName.ToLower();

			if (gamename.Contains(' '))
			{
				GameName = GameName.Replace(" ", "-");
			}

			string reqUrl = $"{gameApiBaseLink}{GameName}?search_precise=false&search_exact=false&key={rawgApiKey}";
			string json = string.Empty;

			using (HttpClient client = new())
			{
				Task<HttpResponseMessage> response = client.GetAsync(reqUrl);
				response.Wait();

				json = await response.Result.Content.ReadAsStringAsync();
			}
			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}
			return DeserializeGameJson(json);
		}

		public GameFallback DeserializeGameFallbackJson(string json)
		{
			return JsonConvert.DeserializeObject<GameFallback>(json);
		}

		public Game DeserializeGameJson(string json)
		{
			return JsonConvert.DeserializeObject<Game>(json);
		}
	}
}