namespace BlazorLibrary.Modelle.Nutzer
{
    public class LibraryUser
    {
        public LibraryUser(int id, string name, int passcode)
        {
            Id = id;
            Name = name;
            Passcode = passcode;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Passcode { get; set; }
    }
}