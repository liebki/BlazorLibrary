using Blazored.Modal.Services;

using BlazorLibrary.Management;
using BlazorLibrary.Modelle.Csv;
using BlazorLibrary.Pages.Komponenten;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class ImportMenu
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string Nachricht { get; set; } = string.Empty;

        [CascadingParameter] public IModalService Modal { get; set; }

        private void test()
        {
            navMan.NavigateTo("/uhawd", true);
        }

        private void ShowModal() => Modal.Show<Confirm>(Nachricht);

        public StreamReader spielcsv { get; set; } = null;
        public string spielcsv_state { get; set; } = "Nothing";
        public StreamReader genrecsv { get; set; } = null;
        public string genrecsv_state { get; set; } = "Nothing";

        protected override async Task OnInitializedAsync()
        {
            if (!String.IsNullOrEmpty(Nachricht))
            {
                ShowModal();
            }
        }

        public async Task VerarbeiteDaten()
        {
            bool importiert = false;
            if (spielcsv?.BaseStream.Length > 0)
            {
                List<object> CsvImportSpiel = csvMan.CsvStreamReaderToObjects<CsvSpiel>(spielcsv);
                List<CsvSpiel> CsvSpieleListe = CsvImportSpiel.Cast<CsvSpiel>().ToList();

                if (CsvSpieleListe.Count > 0)
                {
                    await _db.CsvInsertSpieleInDatabase(CsvSpieleListe, Manager.ActiveUser);
                }
                importiert = true;
            }

            if (genrecsv?.BaseStream.Length > 0)
            {
                List<object> CsvImportGenre = csvMan.CsvStreamReaderToObjects<CsvGenre>(genrecsv);
                List<CsvGenre> CsvGenreListe = CsvImportGenre.Cast<CsvGenre>().ToList();

                if (CsvGenreListe.Count > 0)
                {
                    await _db.CsvInsertGenreInDatabase(CsvGenreListe, Manager.ActiveUser);
                }
                importiert = true;
            }

            if (importiert)
            {
                navMan.NavigateTo("/import?Nachricht=You imported data!", true);
            }
            else
            {
                navMan.NavigateTo("/import?Nachricht=You didn't import data!", true);
            }
        }

        public async Task SelectImportCsv(ImportType sel)
        {
            if (sel == ImportType.Spiel)
            {
                StreamReader input = await Manager.ReadStreamFromFile();
                if (input?.BaseStream.Length > 0)
                {
                    spielcsv = input;
                    spielcsv_state = "File selected, read and waiting for import.";
                    StateHasChanged();
                }
            }
            else
            {
                StreamReader input = await Manager.ReadStreamFromFile();
                if (input?.BaseStream.Length > 0)
                {
                    genrecsv = input;
                    genrecsv_state = "File selected, read and waiting for import.";
                    StateHasChanged();
                }
            }
        }
    }

    public enum ImportType
    {
        Spiel = 0,
        Genre = 1
    }
}