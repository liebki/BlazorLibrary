using BlazorLibrary.Data;
using BlazorLibrary.Models;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class Library
    {
        [Inject]
        public SqliteDatabaseManager DatabaseMan { get; set; }

        private int filtermode;
        private int filterdirection;
        public List<LibraryGame> OriginalGamelist = new();
        public List<LibraryGame> ActiveGamelist = new();

        public int FilterMode
        {
            get
            {
                return filtermode;
            }
            set
            {
                filtermode = value;
                ResortGamecards(filtermode);
            }
        }

        public int FilterDirection
        {
            get
            {
                return filterdirection;
            }
            set
            {
                filterdirection = value;
                ResortGamecards(filtermode);
            }
        }

        public void ResortGamecards(int modus)
        {
            Manager.GamecardFilter(ref modus, ref filterdirection, ref ActiveGamelist, ref OriginalGamelist);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await CollectData();
            OriginalGamelist = ActiveGamelist;
        }

        public async Task CollectData()
        {
            if (ActiveGamelist.Count > 0)
            {
                ActiveGamelist.Clear();
                StateHasChanged();
            }

            ActiveGamelist = await DatabaseMan.GetLibraryUsersGamelist(Manager.ActiveLibraryUser);

            if (ActiveGamelist.Count > 0)
            {
                StateHasChanged();
            }
        }
    }
}