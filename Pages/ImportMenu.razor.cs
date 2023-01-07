using BlazorLibrary.Management;
using BlazorLibrary.Modelle.Csv;

namespace BlazorLibrary.Pages
{
    partial class ImportMenu
    {
        public StreamReader spielcsv { get; set; } = null;
        public string spielcsv_state { get; set; } = "Nothing";
        public StreamReader genrecsv { get; set; } = null;
        public string genrecsv_state { get; set; } = "Nothing";

        protected override async Task OnInitializedAsync()
        {
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
                await Manager.MauiDialog("Information", "You imported data!");
            }
            else
            {
                await Manager.MauiDialog("Information", "You didn't import data!");
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