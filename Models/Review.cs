namespace BlazorLibrary.Models
{
    public class Review
    {
        public Review(int sterneanzahl, string kommentar)
        {
            this.Stars = sterneanzahl;
            this.Reason = kommentar;
        }

        public int Stars { get; set; }
        public string Reason { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Stars)}={Stars}, {nameof(Reason)}={Reason}}}";
        }
    }
}