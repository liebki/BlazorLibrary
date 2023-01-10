using BlazorLibrary.Modelle;
using BlazorLibrary.Management;

namespace BlazorLibrary.Pages
{
    partial class Anzeige
    {
        private int filterung;
        private int filterungfolge;
        public List<Spiel> OriginaleListe = new();
        public List<Spiel> SpieleListe = new();

        public int Filterung
        {
            get
            {
                return filterung;
            }
            set
            {
                filterung = value;
                SortiereSpiele(filterung);
            }
        }

        public int Filterungfolge
        {
            get
            {
                return filterungfolge;
            }
            set
            {
                filterungfolge = value;
                SortiereSpiele(filterung);
            }
        }

        public void SortiereSpiele(int modus)
        {
            Manager.KartenFilterungAnzeige(ref modus, ref filterungfolge, ref SpieleListe, ref OriginaleListe);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await HoleSpiele();
            OriginaleListe = SpieleListe;
        }

        public async Task HoleSpiele()
        {
            if (SpieleListe.Count > 0)
            {
                SpieleListe.Clear();
                StateHasChanged();
            }

            SpieleListe = await _db.SpieleListeErhalten(Manager.ActiveUser);

            if(SpieleListe.Count > 0)
            {
                StateHasChanged();
            }

        }
    }
}