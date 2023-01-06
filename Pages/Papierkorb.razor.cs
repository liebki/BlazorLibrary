using BlazorLibrary.Management;
using BlazorLibrary.Modelle;

namespace BlazorLibrary.Pages
{
    partial class Papierkorb
    {
        public List<Spiel> SpieleListe = new();

        protected override async Task OnInitializedAsync()
        {
            await HoleSpiele();
        }

        public async Task HoleSpiele()
        {
            if (SpieleListe.Count > 0)
            {
                SpieleListe.Clear();
                StateHasChanged();
            }

            SpieleListe = await _db.GeloeschteSpieleListeErhalten(Manager.ActiveUser);

            if (SpieleListe.Count > 0)
            {
                StateHasChanged();
            }
        }
    }
}