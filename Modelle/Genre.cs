namespace BlazorLibrary.Modelle
{
    public class Genre
    {
        public Genre(int id, int owner, string name)
        {
            this.Id = id;
            this.Owner = owner;
            this.Name = name;
        }

        public int Id { get; set; }
        public int Owner { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Owner)}={Owner.ToString()}, {nameof(Name)}={Name}}}";
        }
    }
}