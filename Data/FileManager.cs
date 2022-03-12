using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Threading.Tasks;

namespace BlazorApp.Data
{
    public class FileManager
    {
        public async Task<string> DateiDialogOeffnen()
        {
            string pfad = null;
            BrowserWindow window = null;
            foreach (BrowserWindow win in Electron.WindowManager.BrowserWindows)
            {
                if (win.Id == 1)
                {
                    window = win;
                }
            }

            if (!object.Equals(window, null))
            {
                OpenDialogOptions options = new OpenDialogOptions
                {
                    Title = "Wähle dein Spiel aus.",
                    Filters = new FileFilter[]
                {
                new FileFilter { Name = "EXE", Extensions = new string[] {"exe" } }
                }
                };

                string[] files = await Electron.Dialog.ShowOpenDialogAsync(window, options);
                if (files.Length > 0)
                {
                    pfad = files[0];
                }
            }
            return pfad;
        }

        public async Task<string> CsvDialogOeffnen()
        {
            string pfad = "";
            BrowserWindow window = null;
            foreach (BrowserWindow win in Electron.WindowManager.BrowserWindows)
            {
                if (win.Id == 1)
                {
                    window = win;
                }
            }

            if (!object.Equals(window, null))
            {
                OpenDialogOptions options = new OpenDialogOptions
                {
                    Title = "Wähle eine CSV-Datei aus.",
                    Filters = new FileFilter[]
                {
                new FileFilter { Name = "CSV", Extensions = new string[] {"csv" } }
                }
                };

                string[] files = await Electron.Dialog.ShowOpenDialogAsync(window, options);
                pfad = files[0];
            }
            return pfad;
        }
    }
}