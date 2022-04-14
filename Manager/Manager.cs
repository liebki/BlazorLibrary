using BlazorApp.Data;
using BlazorApp.Modelle.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace BlazorApp
{
    public static class Manager
    {
        private static readonly string[] BildFilterBegriffe = { ".png", ".jpg", ".gif" };

        public static bool InternetAvailable()
        {
            try
            {
                using (Ping ping = new())
                {
                    PingReply result = ping.Send("8.8.8.8", 500, new byte[32], new PingOptions { DontFragment = true, Ttl = 32 });
                    return result.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void AddExternalSource(string name, ref string bild, ref string metacritic, ref string estimatedprice)
        {
            RawgAccessManager rawgMan = new();
            if (Program.Einstellungen.Usepricescraper)
            {
                string price = MmogaPriceScraper.GetPreisVonSpiel(name);
                if (object.Equals(price, null))
                {
                    estimatedprice = "!";
                }
                else
                {
                    estimatedprice = price;
                }
            }
            if (Program.Einstellungen.Userawg)
            {
                Game RawgGame = rawgMan.RawgGameInformations(name);
                if (object.Equals(RawgGame, null))
                {
                    metacritic = "!";
                }
                else
                {
                    metacritic = Convert.ToString(RawgGame.Metacritic);

                    if (string.IsNullOrEmpty(bild) || !BildFilterBegriffe.Any(bild.Contains))
                    {
                        bild = RawgGame.BackgroundImage.ToString();
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(bild) || !BildFilterBegriffe.Any(bild.Contains))
                {
                    bild = "https://sinatax.de/wp-content/themes/consultix/images/no-image-found-360x260.png";
                }
            }
        }

        public static void RemoveNullValues(ref string beschreibung, ref string exepfad)
        {
            if (object.Equals(beschreibung, null))
            {
                beschreibung = "";
            }
            if (object.Equals(exepfad, null))
            {
                exepfad = "";
            }
        }

        public static int RandomGenreNummer()
        {
            int randomNum = RandomSpielNummer();
            int randomNum2 = RandomSpielNummer();
            int randomNum3 = RandomSpielNummer();
            int nummer = randomNum + ((randomNum2 / 4) + (randomNum3 / 2));
            return nummer;
        }

        public static int RandomSpielNummer()
        {
            int number = new Random().Next(1, 999);
            int number2 = new Random(number).Next(1, 999);
            int random = number + (number2 / 2);
            return random;
        }

        public static void KartenFilterungAnzeige(ref int modus, ref int filterungsfolge, ref List<Spiel> SpieleListe, ref List<Spiel> OriginaleListe)
        {
            if (modus == 0)
            {
                SpieleListe = OriginaleListe;
            }
            else if (modus == 1)
            {
                if (filterungsfolge == 0)
                {
                    SpieleListe = SpieleListe.Where(x => Manager.GenreArrayToStringArray(x.Genrelist).Length >= 1).OrderBy(x => Manager.GenreArrayToStringArray(x.Genrelist)[0]).ToList();
                }
                else
                {
                    SpieleListe = SpieleListe.Where(x => Manager.GenreArrayToStringArray(x.Genrelist).Length >= 1).OrderByDescending(x => Manager.GenreArrayToStringArray(x.Genrelist)[0]).ToList();
                }
            }
            else if (modus == 2)
            {
                if (filterungsfolge == 0)
                {
                    SpieleListe = SpieleListe.OrderBy(x => x.Genrelist.Length).ToList();
                }
                else
                {
                    SpieleListe = SpieleListe.OrderByDescending(x => x.Genrelist.Length).ToList();
                }
            }
            else if (modus == 3)
            {
                if (filterungsfolge == 0)
                {
                    SpieleListe = OriginaleListe.OrderBy(x => x.Name).ToList();
                }
                else
                {
                    SpieleListe = OriginaleListe.OrderByDescending(x => x.Name).ToList();
                }
            }
            else if (modus == 4)
            {
                if (filterungsfolge == 0)
                {
                    SpieleListe = OriginaleListe.OrderBy(x => x.Fav).ToList();
                }
                else
                {
                    SpieleListe = OriginaleListe.OrderByDescending(x => x.Fav).ToList();
                }
            }
            else if (modus == 5)
            {
                if (filterungsfolge == 0)
                {
                    SpieleListe = OriginaleListe.OrderBy(x => x.Sterne).ToList();
                }
                else
                {
                    SpieleListe = OriginaleListe.OrderByDescending(x => x.Sterne).ToList();
                }
            }
        }

        public static string[] GenreArrayToStringArray(Genre[] genrelist)
        {
            string[] result = Array.Empty<string>();
            if (!object.Equals(genrelist, null) && genrelist.Length >= 1)
            {
                result = genrelist.Select(a => a.Name).ToArray();
            }
            return result;
        }

        public static ApplicationSettings ReadJsonSettingsFile(string json)
        {
            return JsonConvert.DeserializeObject<ApplicationSettings>(json);
        }

        public static string CreateModalId(string inp)
        {
            string value = inp;
            value = value.ToLower();
            value = String.Concat(value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static void StartExe(string exepfad)
        {
            ProcessStartInfo startInfo = new();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = exepfad;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            using (Process process = Process.Start(startInfo))
            {
                process.Start();
            }
        }
    }
}