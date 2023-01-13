using BlazorLibrary.Data;
using BlazorLibrary.Models;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class Trashcan
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        public List<LibraryGame> Gamelist { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await CollectData();
        }

        public async Task CollectData()
        {
            if (Gamelist.Count > 0)
            {
                Gamelist.Clear();
                StateHasChanged();
            }

            Gamelist = await DatabaseMan.GetLibraryUsersDeletedGamelist(Manager.ActiveLibraryUser);

            if (Gamelist.Count > 0)
            {
                StateHasChanged();
            }
        }
    }
}