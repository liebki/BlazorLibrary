namespace BlazorLibrary.Modelle.Csv
{
	public class CsvSpiel
	{
		public CsvSpiel(string name, string beschreibung, string bildlink, string exepfad)
		{
			this.Name = name;
			this.Beschreibung = beschreibung;
			this.Bildlink = bildlink;
			this.Exepfad = exepfad;
		}

		public string Name { get; set; }
		public string Beschreibung { get; set; }
		public string Bildlink { get; set; }
		public string Exepfad { get; set; }
	}
}