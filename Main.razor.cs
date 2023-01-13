using Microsoft.AspNetCore.Components;

namespace BlazorLibrary
{
    partial class Main
    {
        [Inject]
        private NavigationManager NavigationMan { get; set; }

        protected void BackToStart()
        {
            NavigationMan.NavigateTo("/", true);
        }
    }
}