namespace BlazorApp
{
    public class Spiel
    {
        public Spiel(int id, string name = "", string beschreibung = "", string bildlink = "", string exepfad = "", string sternetooltip = "", int fav = 0, int sterne = 0, int papierkorb = 0, string metacritics = "", string estimatedprice = "", Genre[] genrelist = null)
        {
            this.Id = id;
            this.Name = name;
            this.Beschreibung = beschreibung;
            this.Bildlink = bildlink;
            this.Exepfad = exepfad;
            this.SterneTooltip = sternetooltip;
            this.Fav = fav;
            this.Sterne = sterne;
            this.Papierkorb = papierkorb;
            this.Metacritics = metacritics;
            this.Estimatedprice = estimatedprice;
            this.Genrelist = genrelist;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public string Bildlink { get; set; }
        public string Exepfad { get; set; }
        public string SterneTooltip { get; set; }
        public int Fav { get; set; }
        public int Sterne { get; set; }
        public int Papierkorb { get; set; }
        public string Metacritics { get; set; }
        public string Estimatedprice { get; set; }
        public Genre[] Genrelist { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id}, {nameof(Name)}={Name}, {nameof(Beschreibung)}={Beschreibung}, {nameof(Bildlink)}={Bildlink}, {nameof(Exepfad)}={Exepfad}, {nameof(SterneTooltip)}={SterneTooltip}, {nameof(Fav)}={Fav}, {nameof(Sterne)}={Sterne}, {nameof(Papierkorb)}={Papierkorb}, {nameof(Metacritics)}={Metacritics}, {nameof(Estimatedprice)}={Estimatedprice}, {nameof(Genrelist)}={Genrelist}}}";
        }
    }
}