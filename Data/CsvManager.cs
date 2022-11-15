
using CsvHelper;

using System.Globalization;

using CsvHelper.Configuration;
using BlazorLibrary.Modelle.Csv;

namespace BlazorLibrary.Data
{
	public class CsvManager
	{
		public List<object> CsvStreamReaderToObjects<T>(StreamReader CsvDateiInhalt)
		{
			List<object> RetList = new();

			using (CsvReader csv = new(CsvDateiInhalt, GetConfiguration()))
			{
				csv.Read();
				csv.ReadHeader();

				if (typeof(CsvSpiel).IsAssignableFrom(typeof(T)))
				{
					ProcessCsvSpielEntries(RetList, csv);
				}

				if (typeof(CsvGenre).IsAssignableFrom(typeof(T)))
				{
					ProcessCsvGenreEntries(RetList, csv);
				}
			}
			return RetList;
		}

		private static void ProcessCsvGenreEntries(List<object> RetList, CsvReader csv)
		{
			while (csv.Read())
			{
				string Name = csv.GetField("Name");
				if (!object.Equals(Name, null) && !string.IsNullOrWhiteSpace(Name))
				{
					CsvGenre genre = new(Name);
					RetList.Add(genre);
				}
			}
		}

		private static void ProcessCsvSpielEntries(List<object> RetList, CsvReader csv)
		{
			while (csv.Read())
			{
				string Name = csv.GetField("Name");
				if (!object.Equals(Name, null) && !string.IsNullOrWhiteSpace(Name))
				{
					string Beschreibung = csv.GetField("Beschreibung");
					string Bildlink = csv.GetField("Bildlink");

					string Exepfad = csv.GetField("Exepfad");
					CsvSpiel spiel = new(Name, Beschreibung, Bildlink, Exepfad);

					RetList.Add(spiel);
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