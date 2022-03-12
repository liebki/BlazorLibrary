using BlazorApp.Data;
using Blazored.Modal;
using ElectronNET.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorApp
{
    public class Startup
    {
        private BrowserWindow electronBrowserWindow = null;

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

        private async void CreateWindow()
        {
            electronBrowserWindow = await Electron.WindowManager.CreateWindowAsync();
            electronBrowserWindow.SetMenuBarVisibility(false);
            electronBrowserWindow.OnClosed += () =>
            {
                Electron.App.Quit();
            };
        }
    }
}