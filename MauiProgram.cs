using BlazorLibrary.Data;
using MudBlazor.Services;
using BlazorLibrary.Management;

using Microsoft.Extensions.Logging;

using BlazorLibrary.Modelle.Application;

namespace BlazorLibrary
{
    public static class MauiProgram
    {
        public static ApplicationSettings Einstellungen { get; set; } = new();

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

            builder.Services.AddSingleton<SQLiteManager>();

            builder.Services.AddSingleton<CsvManager>();
            builder.Services.AddSingleton<RawgNetManager>();

            SetupLibrary();
            return builder.Build();
        }

        public static void SetupLibrary()
        {
            string EinstellungenJson = File.ReadAllText(Path.Combine(Manager.MauiProgramActiveDirectory(), "ApplicationSettingsFile.json"));
            Einstellungen = Manager.ReadJsonSettingsFile(EinstellungenJson);

            if (Einstellungen is not null)
            {
                if (!Manager.InternetAvailable() || Einstellungen.Rawgapikey.Length < 10)
                {
                    Einstellungen.Usepricescraper = false;
                    Einstellungen.Userawg = false;
                }
                SQLiteManager.SetupDatabase();
            }
        }
    }
}