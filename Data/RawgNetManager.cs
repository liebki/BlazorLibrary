using System;
using RawgNET;

using System.Linq;

namespace BlazorLibrary.Data
{
	public class RawgNetManager
	{
		public Game GetGameByName(string gamename, bool getAchievements, bool getScreenshots, string apikey)
		{
			/*
			 * 
			 * Besser über 'apikey' Positionierung nachdenken..
			 * 
			 */
			using (RawgClient client = new(new RawgClientOptions(apikey)))
			{
				if (client.IsGameExisting(gamename))
				{
					Game game = client.GetGameData(gamename, getAchievements, getScreenshots);
					if (!object.Equals(game, null))
					{
						return game;
					}
				}
			}
			return null;
		}

	}
}
