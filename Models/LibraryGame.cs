namespace BlazorLibrary.Models
{
    public class LibraryGame
    {
        public LibraryGame(int id, int owner = 0, string name = "", string beschreibung = "", string bildlink = "", string exepfad = "", string sternetooltip = "", int fav = 0, int sterne = 0, int papierkorb = 0, string metacritics = "", string estimatedprice = "", (string Text, bool Commented, int Id) kommentar = new(), LibraryGenre[] genrelist = null)
        {
            this.Id = id;
            this.LibraryUserId = owner;
            this.Name = name;
            this.Description = beschreibung;
            this.ImageUrl = bildlink;
            this.ExecutablePath = exepfad;
            this.ReviewText = sternetooltip;
            this.IsFavourite = fav;
            this.ReviewStars = sterne;
            this.IsInTrash = papierkorb;
            this.Metacritics = metacritics;
            this.RoughPrice = estimatedprice;
            this.Comment = kommentar;
            this.ListOfGenres = genrelist;
        }

        public int Id { get; set; }
        public int LibraryUserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ExecutablePath { get; set; }
        public string ReviewText { get; set; }
        public int IsFavourite { get; set; }
        public int ReviewStars { get; set; }
        public int IsInTrash { get; set; }
        public string Metacritics { get; set; }
        public string RoughPrice { get; set; }
        public (string Text, bool Commented, int Id) Comment { get; set; }
        public LibraryGenre[] ListOfGenres { get; set; }
    }
}