namespace BlazorLibrary.Modelle
{
    public class Bewertung
    {
        public Bewertung(int sterneanzahl, string kommentar)
        {
            this.Sterneanzahl = sterneanzahl;
            this.Grund = kommentar;
        }

        public int Sterneanzahl { get; set; }
        public string Grund { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Sterneanzahl)}={Sterneanzahl}, {nameof(Grund)}={Grund}}}";
        }
    }
}