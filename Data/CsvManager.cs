using BlazorApp.Modelle.Csv;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BlazorApp.Data
{
    public class CsvManager
    {
        public List<CsvSpiel> CsvZuSpielListe(string csvPath)
        {
            List<CsvSpiel> SpieleListe = new();
            using (StreamReader reader = new(csvPath))
            using (CsvReader csv = new(reader, GetConfiguration()))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    string Name = csv.GetField("Name");
                    if (!object.Equals(Name, null) && !string.IsNullOrWhiteSpace(Name))
                    {
                        string Beschreibung = csv.GetField("Beschreibung");
                        string Bildlink = csv.GetField("Bildlink");
                        string Exepfad = csv.GetField("Exepfad");
                        CsvSpiel spiel = new(Name, Beschreibung, Bildlink, Exepfad);
                        SpieleListe.Add(spiel);
                    }
                }
            }
            return SpieleListe;
        }

        public List<CsvGenre> CsvZuGenreListe(string csvPath)
        {
            List<CsvGenre> GenreListe = new();
            using (StreamReader reader = new(csvPath))
            using (CsvReader csv = new(reader, GetConfiguration()))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    string Name = csv.GetField("Name");
                    if (!object.Equals(Name, null) && !string.IsNullOrWhiteSpace(Name))
                    {
                        CsvGenre genre = new(Name);
                        GenreListe.Add(genre);
                    }
                }
            }
            return GenreListe;
        }

        private static CsvConfiguration GetConfiguration()
        {
            CsvConfiguration configuration = new(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                ShouldSkipRecord = record => record.Record.All(string.IsNullOrWhiteSpace)
            };
            return configuration;
        }
    }
}