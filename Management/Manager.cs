﻿using System.Text;
using RawgNET.Models;
using Newtonsoft.Json;
using System.Reflection;

using System.Diagnostics;
using BlazorLibrary.Data;
using BlazorLibrary.Modelle;
using BlazorLibrary.Modelle.Nutzer;

using System.Net.NetworkInformation;

using BlazorLibrary.Modelle.Application;

namespace BlazorLibrary.Management
{
    public static class Manager
    {
        public static LibraryUser ActiveUser { get; set; } = null;
        private static readonly string[] BildFilterBegriffe = { ".png", ".jpg", ".gif" };

        public static bool DoesStringContainCharacters(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return true;
            }
            return false;
        }

        public static void ClearActiveUser()
        {
            ActiveUser = null;
        }

        public static bool InternetAvailable()
        {
            try
            {
                using Ping ping = new();
                PingReply result = ping.Send("8.8.8.8", 500, new byte[32], new PingOptions
                {
                    DontFragment = true,
                    Ttl = 32
                });

                return result.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        public static void SaveImage(Stream stream, string path)
        {
            using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
        }

        public static string MauiProgramActiveDirectory()
        {
            string strExeFilePath = Assembly.GetExecutingAssembly().Location;
            string strWorkPath = Path.GetDirectoryName(strExeFilePath);

            return strWorkPath;
        }

        public static async Task<(string, string, string)> AddExternalSource(string name, string bildurl = "")
        {
            string bild = bildurl, metacritic = "∅", estimatedprice = "∅";

            if (MauiProgram.Einstellungen.Usepricescraper)
            {
                string price = MmogaPriceScraper.GetPreisVonSpiel(name);
                if (!string.IsNullOrEmpty(price) && !string.IsNullOrWhiteSpace(price))
                {
                    estimatedprice = price;
                }
            }

            if (MauiProgram.Einstellungen.Userawg)
            {
                Game RawgGame = await RawgNetManager.GetGameByName(name, MauiProgram.Einstellungen.Rawgapikey, true, true);
                if (RawgGame != null && RawgGame.Id != null)
                {
                    metacritic = Convert.ToString(RawgGame.Metacritic);
                    if (string.IsNullOrEmpty(bild) || string.IsNullOrWhiteSpace(bild) || !BildFilterBegriffe.Any(bild.Contains))
                    {
                        bild = RawgGame.BackgroundImage.ToString();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(bild) || string.IsNullOrWhiteSpace(bild) || !BildFilterBegriffe.Any(bild.Contains))
                    {
                        bild = "https://sinatax.de/wp-content/themes/consultix/images/no-image-found-360x260.png";
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(bild) || string.IsNullOrWhiteSpace(bild) || !BildFilterBegriffe.Any(bild.Contains))
                {
                    bild = "https://sinatax.de/wp-content/themes/consultix/images/no-image-found-360x260.png";
                }
            }
            return (bild, metacritic, estimatedprice);
        }

        public static (string, string) RemoveNullValues(string beschreibung, string exepfad)
        {
            if (string.IsNullOrEmpty(beschreibung))
            {
                beschreibung = string.Empty;
            }
            if (string.IsNullOrEmpty(exepfad))
            {
                exepfad = string.Empty;
            }
            return (beschreibung, exepfad);
        }

        public static int RandomIdNumber()
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
            if (genrelist != null && genrelist.Length >= 1)
            {
                result = genrelist.Select(a => a.Name).ToArray();
            }
            return result;
        }

        public static async Task MauiDialog(string title = "Window", string message = "Message", string buttontext = "OK")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttontext);
        }

        public static async Task<string> ReadStringFromFile(string pickername = "Datei wählen")
        {
            StringBuilder ret = new();

            PickOptions options = new()
            {
                PickerTitle = pickername
            };

            try
            {
                FileResult result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    Stream stream = await result.OpenReadAsync();
                    byte[] b = new byte[1024];

                    UTF8Encoding temp = new(true);

                    while (stream.Read(b, 0, b.Length) > 0)
                    {
                        ret.Append(temp.GetString(b));
                    }

                    stream.Flush();
                    stream.Position = 0;
                }
                return ret.ToString();
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return ret.ToString();
        }

        public static async Task<FileResult> GetExecuteablePath(string pickername = "Spieldatei wählen")
        {
            PickOptions options = new()
            {
                PickerTitle = pickername
            };

            try
            {
                FileResult result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }

        public static async Task<StreamReader> ReadStreamFromFile(string pickername = "Datei wählen")
        {
            StreamReader ret = null;

            PickOptions options = new()
            {
                PickerTitle = pickername
            };

            try
            {
                FileResult result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    Stream stream = await result.OpenReadAsync();
                    stream.Flush();

                    ret = new(stream);
                    stream.Position = 0;
                }
                return ret;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return ret;
        }

        public static ApplicationSettings ReadJsonSettingsFile(string json)
        {
            return JsonConvert.DeserializeObject<ApplicationSettings>(json);
        }

        public static string GetSettingsAsJson(ApplicationSettings settings)
        {
            return JsonConvert.SerializeObject(settings, Formatting.Indented);
        }

        public static void StartExe(string exepfad)
        {
            ProcessStartInfo startInfo = new()
            {
                CreateNoWindow = true,

                WorkingDirectory = $"C:\\users\\{Environment.UserName}\\Desktop\\",
                UseShellExecute = true,

                FileName = exepfad,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using Process process = Process.Start(startInfo);
            process.Start();
        }
    }
}