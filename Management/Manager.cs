
using RawgNET.Models;
using Newtonsoft.Json;
using System.Reflection;

using System.Diagnostics;
using BlazorLibrary.Data;
using BlazorLibrary.Models;

using BlazorLibrary.Models.User;

using System.Net.NetworkInformation;

namespace BlazorLibrary.Management
{
    public static class Manager
    {
        public static LibraryUser ActiveLibraryUser { get; set; } = null;
        private static readonly string[] GameImageExtensionsFilter = { ".png", ".jpg", ".gif" };
        private const string ImageNotFoundUrl = "https://sinatax.de/wp-content/themes/consultix/images/no-image-found-360x260.png";

        public static async Task<(string, bool)> OpenSelectExecutableDialog()
        {
            string ExecutablePath = string.Empty;
            FileResult file = await Manager.PickExecutablePath();

            if (file != null)
            {
                ExecutablePath = file.FullPath;
                return (ExecutablePath, true);
            }

            return (ExecutablePath, false);
        }
        public static bool DoesStringContainCharacters(string Input)
        {
            foreach (char c in Input)
            {
                if (!char.IsDigit(c))
                    return true;
            }
            return false;
        }

        public static void ClearActiveLibraryUser()
        {
            ActiveLibraryUser = null;
        }

        public static bool IsInternetAvailable()
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

        public static void SaveImage(Stream DataStream, string Path)
        {
            using FileStream fileStream = new(Path, FileMode.Create, FileAccess.Write);
            DataStream.CopyTo(fileStream);
        }

        public static string GetExecutionDirectory()
        {
            string WholeAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string AssemblyDirectoryPath = Path.GetDirectoryName(WholeAssemblyPath);

            return AssemblyDirectoryPath;
        }

        public static async Task<(string, string, string)> AddExternalGamecardSources(string name, string imageurl = "")
        {
            string ImageUrl = imageurl, Metacritic = "∅", RoughPrice = "∅";

            if (MauiProgram.Settings.UsePriceScraper)
            {
                string Price = PriceScraperManager.GetRoughPriceOfGame(name);
                if (!string.IsNullOrEmpty(Price) && !string.IsNullOrWhiteSpace(Price))
                {
                    RoughPrice = Price;
                }
            }

            if (MauiProgram.Settings.UseRawg)
            {
                Game GameRawgQuery = await RawgNetManager.GetGameByName(name, MauiProgram.Settings.RawgApikey, true, true);
                if (GameRawgQuery != null && GameRawgQuery.Id != null)
                {
                    Metacritic = Convert.ToString(GameRawgQuery.Metacritic);
                    if (string.IsNullOrEmpty(ImageUrl) || string.IsNullOrWhiteSpace(ImageUrl) || !GameImageExtensionsFilter.Any(ImageUrl.Contains))
                    {
                        ImageUrl = GameRawgQuery.BackgroundImage.ToString();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(ImageUrl) || string.IsNullOrWhiteSpace(ImageUrl) || !GameImageExtensionsFilter.Any(ImageUrl.Contains))
                    {
                        ImageUrl = ImageNotFoundUrl;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ImageUrl) || string.IsNullOrWhiteSpace(ImageUrl) || !GameImageExtensionsFilter.Any(ImageUrl.Contains))
                {
                    ImageUrl = ImageNotFoundUrl;
                }
            }
            return (ImageUrl, Metacritic, RoughPrice);
        }

        public static (string, string) ToBeSureRemoveNullValues(string Description, string ExecutablePath)
        {
            if (string.IsNullOrEmpty(Description))
            {
                Description = string.Empty;
            }
            if (string.IsNullOrEmpty(ExecutablePath))
            {
                ExecutablePath = string.Empty;
            }
            return (Description, ExecutablePath);
        }

        public static int RandomNumber()
        {
            int x = new Random().Next(1, 999);
            int y = new Random(new Random().Next(1, 999)).Next(1, 999);

            int z = x + (y / 2);
            return z;
        }

        /// <summary>
        /// Has to be WAY more simple and maybe in Library.razor.cs
        /// </summary>
        public static void GamecardFilter(ref int FilterMode, ref int FilterDirection, ref List<LibraryGame> ShownGamelist, ref List<Models.LibraryGame> OriginalGamelist)
        {
            if (FilterMode == 0)
            {
                ShownGamelist = OriginalGamelist;
            }
            else if (FilterMode == 1)
            {
                if (FilterDirection == 0)
                {
                    ShownGamelist = ShownGamelist.Where(x => Manager.GenreArrayToString(x.ListOfGenres).Length >= 1).OrderBy(x => Manager.GenreArrayToString(x.ListOfGenres)[0]).ToList();
                }
                else
                {
                    ShownGamelist = ShownGamelist.Where(x => Manager.GenreArrayToString(x.ListOfGenres).Length >= 1).OrderByDescending(x => Manager.GenreArrayToString(x.ListOfGenres)[0]).ToList();
                }
            }
            else if (FilterMode == 2)
            {
                if (FilterDirection == 0)
                {
                    ShownGamelist = ShownGamelist.OrderBy(x => x.ListOfGenres.Length).ToList();
                }
                else
                {
                    ShownGamelist = ShownGamelist.OrderByDescending(x => x.ListOfGenres.Length).ToList();
                }
            }
            else if (FilterMode == 3)
            {
                if (FilterDirection == 0)
                {
                    ShownGamelist = OriginalGamelist.OrderBy(x => x.Name).ToList();
                }
                else
                {
                    ShownGamelist = OriginalGamelist.OrderByDescending(x => x.Name).ToList();
                }
            }
            else if (FilterMode == 4)
            {
                if (FilterDirection == 0)
                {
                    ShownGamelist = OriginalGamelist.OrderBy(x => x.IsFavourite).ToList();
                }
                else
                {
                    ShownGamelist = OriginalGamelist.OrderByDescending(x => x.IsFavourite).ToList();
                }
            }
            else if (FilterMode == 5)
            {
                if (FilterDirection == 0)
                {
                    ShownGamelist = OriginalGamelist.OrderBy(x => x.ReviewStars).ToList();
                }
                else
                {
                    ShownGamelist = OriginalGamelist.OrderByDescending(x => x.ReviewStars).ToList();
                }
            }
        }

        public static string[] GenreArrayToString(LibraryGenre[] Genrelist)
        {
            string[] GenreStringlist = Array.Empty<string>();
            if (Genrelist != null && Genrelist.Length >= 1)
            {
                GenreStringlist = Genrelist.Select(a => a.Name).ToArray();
            }
            return GenreStringlist;
        }

        public static async Task SimpleDialogMessage(string Title = "Window", string Message = "Message", string ButtonText = "OK")
        {
            await Application.Current.MainPage.DisplayAlert(Title, Message, ButtonText);
        }

        public static async Task<FileResult> PickExecutablePath(string PickerTitle = "Choose the executable")
        {
            PickOptions options = new()
            {
                PickerTitle = PickerTitle
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
            }

            return null;
        }

        public static async Task<StreamReader> GetStreamOfFile(string PickerTitle = "Choose a file")
        {
            StreamReader ret = null;
            PickOptions options = new()
            {
                PickerTitle = PickerTitle
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
            }

            return ret;
        }

        public static T JsonToObject<T>(string JsonText)
        {
            return JsonConvert.DeserializeObject<T>(JsonText);
        }

        public static string ObjectToJson(object ObjectInput)
        {
            return JsonConvert.SerializeObject(ObjectInput, Formatting.Indented);
        }

        public static void TryToCallExecutable(string exepfad)
        {
            ProcessStartInfo ProcessValues = new()
            {
                CreateNoWindow = true,
                WorkingDirectory = $"C:\\users\\{Environment.UserName}\\Desktop\\",

                UseShellExecute = true,
                FileName = exepfad,

                WindowStyle = ProcessWindowStyle.Hidden
            };

            using Process process = Process.Start(ProcessValues);
            process.Start();
        }
    }
}