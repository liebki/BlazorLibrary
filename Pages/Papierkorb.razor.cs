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
            SpieleListe.Clear();
            SpieleListe = await _db.GeloeschteSpieleListeErhalten();

            StateHasChanged();
        }
    }
}