using RawgNET;
using RawgNET.Models;

namespace BlazorLibrary.Data
{
    public class RawgNetManager
    {
        protected RawgNetManager()
        { }

        public static async Task<Game> GetGameByName(string gamename, string apikey, bool getAchievements = false, bool getScreenshots = false)
        {
            RawgClient client = new(new ClientOptions(apikey));
            if (await client.IsGameExisting(gamename))
            {
                Game game = await client.GetGame(gamename, getAchievements, getScreenshots);
                if (game != null)
                {
                    return game;
                }
            }
            return null;
        }
    }
}