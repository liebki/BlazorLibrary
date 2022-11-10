using System.Collections.Generic;
using System.Threading.Tasks;

using BlazorLibrary.Management;
using BlazorLibrary.Modelle;

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
            get { return filterung; }
            set
            {
                filterung = value;
                SortiereSpiele(filterung);
            }
        }

        public int Filterungfolge
        {
            get { return filterungfolge; }
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
            SpieleListe.Clear();
            SpieleListe = await _db.SpieleListeErhalten();

            StateHasChanged();
        }
    }
}