namespace BlazorApp.Modelle.Csv
{
    public class CsvGenre
    {
        public CsvGenre(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}