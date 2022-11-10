using Blazored.Modal.Services;

using BlazorLibrary.Pages.Komponenten;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class Index
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string Nachricht { get; set; } = string.Empty;

        [CascadingParameter] public IModalService Modal { get; set; }

        private void ShowModal() => Modal.Show<Confirm>(Nachricht);

        protected override async Task OnInitializedAsync()
        {
            if (!String.IsNullOrEmpty(Nachricht))
            {
                ShowModal();
            }
        }
    }
}