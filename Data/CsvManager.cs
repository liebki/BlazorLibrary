
using CsvHelper;

using System.Globalization;

using CsvHelper.Configuration;
using BlazorLibrary.Models.Csv;

namespace BlazorLibrary.Data
{
    public class CsvManager
    {
        public List<object> CsvStreamReaderToObjects<T>(StreamReader CsvDateiInhalt)
        {
            List<object> ItemList = new();

            using (CsvReader csv = new(CsvDateiInhalt, GetConfiguration()))
            {
                csv.Read();
                csv.ReadHeader();

                if (typeof(CsvGame).IsAssignableFrom(typeof(T)))
                {
                    ProcessCsvGameEntries(ItemList, csv);
                }

                if (typeof(CsvGenre).IsAssignableFrom(typeof(T)))
                {
                    ProcessCsvGenreEntries(ItemList, csv);
                }
            }
            return ItemList;
        }

        private static void ProcessCsvGenreEntries(List<object> ItemList, CsvReader csv)
        {
            while (csv.Read())
            {
                string Name = csv.GetField("Name");
                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name))
                {
                    CsvGenre genre = new(Name);
                    ItemList.Add(genre);
                }
            }
        }

        private static void ProcessCsvGameEntries(List<object> ItemList, CsvReader csv)
        {
            while (csv.Read())
            {
                string Name = csv.GetField("Name");
                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name))
                {
                    string Description = csv.GetField("Beschreibung");
                    string ImageUrl = csv.GetField("Bildlink");

                    string ExecutablePath = csv.GetField("Exepfad");
                    CsvGame game = new(Name, Description, ImageUrl, ExecutablePath);

                    ItemList.Add(game);
                }
            }
        }

        private static CsvConfiguration GetConfiguration()
        {
            CsvConfiguration configuration = new(CultureInfo.CurrentCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                ShouldSkipRecord = record => record.Row.Parser.Record.All(string.IsNullOrWhiteSpace)
            };
            return configuration;
        }
    }
}