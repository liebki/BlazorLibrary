using BlazorLibrary.Data;
using BlazorLibrary.Management;
using BlazorLibrary.Modelle.Nutzer;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class Anmeldung
    {
        [Inject]
        private NavigationManager NavMan { get; set; }

        [Inject]
        private SQLiteManager SqlMan { get; set; }

        private string Nickname { get; set; }
        private string Passcode { get; set; }

        private string DeletionConfirmField { get; set; } = string.Empty;
        private bool DeletionButtonState { get; set; } = true;

        private async Task TryUserDeletion()
        {
            if (DeletionConfirmField.Equals("delete"))
            {
                await SqlMan.DeleteLibraryUser(Manager.ActiveUser);
                await Application.Current.MainPage.DisplayAlert("Alert", "Account was deleted!", "OK");

                await TryUserLogout();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Confirmation was not given!", "OK");
            }
            DeletionConfirmField = string.Empty;
        }

        private bool DeletionTextFieldCheck(string feld)
        {
            if (feld.Contains("delete"))
            {
                DeletionButtonState = false;
                return true;
            }
            DeletionButtonState = true;
            return false;
        }

        private async Task TryUserLogin()
        {
            bool IsCredentialsUnvalid = false;
            if (string.IsNullOrEmpty(Nickname) || string.IsNullOrWhiteSpace(Nickname) || Nickname.Contains(' '))
            {
                Nickname = string.Empty;
                IsCredentialsUnvalid = true;
            }

            if (Manager.DoesStringContainCharacters(Passcode) || string.IsNullOrWhiteSpace(Passcode) || Passcode.Contains(' ') || Passcode.Length > 6)
            {
                Passcode = string.Empty;
                IsCredentialsUnvalid = true;
            }

            if (IsCredentialsUnvalid)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Nickname or Passcode dont follow the rules!\nNicknames ONLY contain numbers or characters\nPasscodes ONLY contain digits, nothing else!", "Retry");
            }
            else
            {
                Tuple<LibraryUser, UserCase> LoginVersuch = SqlMan.ErhalteNutzer(Nickname, Int32.Parse(Passcode));

                if (LoginVersuch != null)
                {
                    if (LoginVersuch.Item2 == UserCase.Exists)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "You have been logged in!", "OK");
                        Manager.ActiveUser = LoginVersuch.Item1;
                    }
                    else if (LoginVersuch.Item2 == UserCase.Createable)
                    {
                        bool CreateAccount = await Application.Current.MainPage.DisplayAlert("Question?", "Would you like to register an account?", "Yes", "No");
                        if (CreateAccount)
                        {
                            await SqlMan.ErstelleNutzer(Nickname, Int32.Parse(Passcode));
                            await Application.Current.MainPage.DisplayAlert("Alert", "The account has been created, you may log in now!", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Alert", "See you soon!", "OK");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "An error occured, please try another combination!", "OK");
                    }
                }
            }
            Nickname = string.Empty;
            Passcode = string.Empty;
            NavMan.NavigateTo("/", true);
        }

        private async Task TryUserLogout()
        {
            Manager.ClearActiveUser();
            NavMan.NavigateTo("/", true);
        }

        protected override async Task OnInitializedAsync()
        {
        }
    }
}