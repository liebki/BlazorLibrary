using BlazorApp.Data;
using BlazorApp.Modelle.Application;
using ElectronNET.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace BlazorApp
{
    public class Program
    {
        public static ApplicationSettings Einstellungen { get; set; } = new();

        public static void Main()
        {
            Einstellungen = Manager.ReadJsonSettingsFile(File.ReadAllText("ApplicationSettingsFile.json"));
            bool InternetAvailable = Manager.InternetAvailable();
            if (!object.Equals(Einstellungen, null))
            {
                if (!InternetAvailable)
                {
                    Einstellungen.Usepricescraper = false;
                    Einstellungen.Userawg = false;
                }
                SQLiteManager.SetupDatabase();
                CreateHostBuilder(null).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseElectron(args);
                    webBuilder.UseEnvironment("Development");
                    webBuilder.UseStartup<Startup>();
                });
    }
}