namespace BlazorLibrary.Modelle
{
    public class Bewertung
    {
        public Bewertung(int sterneanzahl, string sternebewertung)
        {
            this.Sterneanzahl = sterneanzahl;
            this.Sternebewertung = sternebewertung;
        }

        public int Sterneanzahl { get; set; }
        public string Sternebewertung { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Sterneanzahl)}={Sterneanzahl}, {nameof(Sternebewertung)}={Sternebewertung}}}";
        }
    }
}