namespace BlazorLibrary.Models.Csv
{
    public class CsvGame
    {
        public CsvGame(string name, string beschreibung, string bildlink, string exepfad)
        {
            this.Name = name;
            this.Description = beschreibung;
            this.ImageUrl = bildlink;
            this.ExecutablePath = exepfad;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ExecutablePath { get; set; }
    }
}