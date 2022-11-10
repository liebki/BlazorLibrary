using System.Reflection;

using Blazored.Modal;

using BlazorLibrary.Data;
using BlazorLibrary.Management;
using BlazorLibrary.Modelle.Application;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.Extensions.Logging;

namespace BlazorLibrary {

    public static class MauiProgram
    {
        public static ApplicationSettings Einstellungen { get; set; } = new();
        public static MauiApp CreateMauiApp()
	    {
            var builder = MauiApp.CreateBuilder();
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
            builder.Services.AddBlazoredModal();

            builder.Services.AddSingleton<SQLiteManager>();
            builder.Services.AddSingleton<RawgAccessManager>();
            
            builder.Services.AddSingleton<CsvManager>();
            SetupLibrary();

            return builder.Build();
	    }

        private static void SetupLibrary()
        {
            Einstellungen = Manager.ReadJsonSettingsFile(File.ReadAllText(Path.Combine(Manager.MauiProgramActiveDirectory(), "ApplicationSettingsFile.json")));
            bool InternetAvailable = Manager.InternetAvailable();

            if (Einstellungen is not null)
            {
                if (!InternetAvailable)
                {
                    Einstellungen.Usepricescraper = false;
                    Einstellungen.Userawg = false;
                }
                SQLiteManager.SetupDatabase();
            }
        }
    }
}