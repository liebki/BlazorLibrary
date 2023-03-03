using NavigationManagerUtils;
using Microsoft.AspNetCore.Components;

namespace BlazorLibrary
{
    partial class Main
    {
        [Inject]
        private NavManUtils NavMan { get; set; }

        protected void BackToStart()
        {
            NavMan.Navigate("/");
        }
    }
}