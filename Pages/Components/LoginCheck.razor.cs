using NavigationManagerUtils;
using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Components
{
    partial class LoginCheck
    {
        [Inject]
        public NavManUtils NavMan { get; set; }
    }
}