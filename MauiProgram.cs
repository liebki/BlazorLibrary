using BlazorLibrary.Data;
using MudBlazor.Services;
using NavigationManagerUtils;

using BlazorLibrary.Management;

using Microsoft.Extensions.Logging;
using BlazorLibrary.Models.Application;

namespace BlazorLibrary
{
    public static class MauiProgram
    {
        public static ApplicationSettings Settings { get; set; } = new();

        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

            AddCustomManagers(builder);
            SetupLibrary();

            return builder.Build();
        }

        private static void AddCustomManagers(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<SqliteDatabaseManager>();
            builder.Services.AddSingleton<CsvManager>();

            builder.Services.AddTransient<NavManUtils>();
            builder.Services.AddSingleton<RawgNetManager>();
        }

        public static void SetupLibrary()
        {
            string SettingsAsJson = File.ReadAllText(Path.Combine(Manager.GetExecutionDirectory(), "ApplicationSettingsFile.json"));
            Settings = Manager.JsonToObject<ApplicationSettings>(SettingsAsJson);

            if (Settings is not null)
            {
                if (!Manager.IsInternetAvailable() || Settings.RawgApikey.Length < 10)
                {
                    Settings.UsePriceScraper = false;
                    Settings.UseRawg = false;
                }
                SqliteDatabaseManager.SetupDatabase();
            }
        }
    }
}