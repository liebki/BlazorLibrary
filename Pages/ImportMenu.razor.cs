using BlazorLibrary.Data;
using BlazorLibrary.Management;
using BlazorLibrary.Models.Csv;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class ImportMenu
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        [Inject]
        public CsvManager CsvMan { get; set; }

        public StreamReader GameCsvStream { get; set; } = null;
        public string GameCsvPlaceholder { get; set; } = "Nothing loaded yet";
        public StreamReader GenreCsvStream { get; set; } = null;
        public string GenreCsvPlaceholder { get; set; } = "Nothing loaded yet";

        public async Task VerarbeiteDaten()
        {
            bool ImportedData = false;
            if (GameCsvStream?.BaseStream.Length > 0)
            {
                List<object> CsvImportGamelist = CsvMan.CsvStreamReaderToObjects<CsvGame>(GameCsvStream);
                List<CsvGame> CsvGamelist = CsvImportGamelist.Cast<CsvGame>().ToList();

                if (CsvGamelist.Count > 0)
                {
                    await DatabaseMan.InsertCsvEntries((dynamic)CsvGamelist, Manager.ActiveLibraryUser);
                }
                ImportedData = true;
            }

            if (GenreCsvStream?.BaseStream.Length > 0)
            {
                List<object> CsvImportGenrelist = CsvMan.CsvStreamReaderToObjects<CsvGenre>(GenreCsvStream);
                List<CsvGenre> CsvGenreListe = CsvImportGenrelist.Cast<CsvGenre>().ToList();

                if (CsvGenreListe.Count > 0)
                {
                    await DatabaseMan.InsertCsvEntries((dynamic)CsvGenreListe, Manager.ActiveLibraryUser);
                }
                ImportedData = true;
            }

            if (ImportedData)
            {
                await Manager.SimpleDialogMessage("Information", "You imported data!");
            }
            else
            {
                await Manager.SimpleDialogMessage("Information", "You didn't import data!");
            }
        }

        public async Task SelectImportCsv(ImportType sel)
        {
            if (sel == ImportType.Game)
            {
                StreamReader input = await Manager.GetStreamOfFile();
                if (input?.BaseStream.Length > 0)
                {
                    GameCsvStream = input;
                    GameCsvPlaceholder = "File selected, read and waiting for import.";

                    StateHasChanged();
                }
            }
            else
            {
                StreamReader input = await Manager.GetStreamOfFile();
                if (input?.BaseStream.Length > 0)
                {
                    GenreCsvStream = input;
                    GenreCsvPlaceholder = "File selected, read and waiting for import.";

                    StateHasChanged();
                }
            }
        }
    }

    public enum ImportType
    {
        Game = 0,
        Genre = 1
    }
}