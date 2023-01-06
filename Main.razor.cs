using Microsoft.AspNetCore.Components;

namespace BlazorLibrary
{
    partial class Main
    {
        [Inject]
        private NavigationManager NavMan { get; set; }

        protected void BackToStart()
        {
            NavMan.NavigateTo("/", true);
        }
    }
}