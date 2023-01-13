namespace BlazorLibrary.Models
{
    public class LibraryGenre
    {
        public LibraryGenre(int id, int owner, string name)
        {
            this.Id = id;
            this.LibraryUserId = owner;
            this.Name = name;
        }

        public int Id { get; set; }
        public int LibraryUserId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(LibraryUserId)}={LibraryUserId.ToString()}, {nameof(Name)}={Name}}}";
        }
    }
}