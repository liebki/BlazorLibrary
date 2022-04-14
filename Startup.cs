using BlazorApp.Data;
using Blazored.Modal;
using ElectronNET.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredModal();
            services.AddSingleton<FileManager>();
            services.AddSingleton<SQLiteManager>();
            services.AddSingleton<RawgAccessManager>();
            services.AddSingleton<CsvManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            if (HybridSupport.IsElectronActive)
            {
                CreateWindow();
            }
        }

        private static async Task CreateWindow()
        {
            BrowserWindow ElectronBrowserWindow = await Electron.WindowManager.CreateWindowAsync();
            ElectronBrowserWindow.SetMenuBarVisibility(false);
            ElectronBrowserWindow.OnClosed += () =>
            {
                Electron.App.Quit();
            };
        }
    }
}