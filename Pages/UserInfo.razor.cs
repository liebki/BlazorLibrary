using BlazorLibrary.Data;
using BlazorLibrary.Management;
using BlazorLibrary.Models.User;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class UserInfo
    {
        [Inject]
        private NavigationManager NavigationMan { get; set; }

        [Inject]
        private SqliteDatabaseManager DatabaseMan { get; set; }

        private string UserNick { get; set; }
        private string UserPas { get; set; }
        private string DeletionConfirmField { get; set; } = string.Empty;
        private bool DeletionButtonState { get; set; } = true;

        private async Task TryUserDeletion()
        {
            if (DeletionConfirmField.Equals("delete"))
            {
                await DatabaseMan.DeleteLibraryUser(Manager.ActiveLibraryUser);
                await Manager.SimpleDialogMessage("Alert", "Account was deleted!", "OK");

                TryUserLogout();
            }
            else
            {
                await Manager.SimpleDialogMessage("Alert", "Confirmation was not given!", "OK");
            }
            DeletionConfirmField = string.Empty;
        }

        private bool DeletionTextFieldCheck(string Fieldtext)
        {
            if (Fieldtext.Contains("delete"))
            {
                DeletionButtonState = false;
                return true;
            }
            DeletionButtonState = true;
            return false;
        }

        private async Task TryUserLogin()
        {
            bool IsCredentialsInvalid = false;
            if (string.IsNullOrEmpty(UserNick) || string.IsNullOrWhiteSpace(UserNick) || UserNick.Contains(' '))
            {
                UserNick = string.Empty;
                IsCredentialsInvalid = true;
            }

            if (Manager.DoesStringContainCharacters(UserPas) || string.IsNullOrWhiteSpace(UserPas) || UserPas.Contains(' ') || UserPas.Length > 6)
            {
                UserPas = string.Empty;
                IsCredentialsInvalid = true;
            }

            if (IsCredentialsInvalid)
            {
                await Manager.SimpleDialogMessage("Alert", "Nickname or Passcode dont follow the rules!\nNicknames ONLY contain numbers or characters\nPasscodes ONLY contain digits, nothing else!", "Retry");
            }
            else
            {
                Tuple<LibraryUser, LibraryUserAccountState> LibraryUserTry = DatabaseMan.GetLibraryUserEntry(UserNick, Int32.Parse(UserPas));

                if (LibraryUserTry != null)
                {
                    if (LibraryUserTry.Item2 == LibraryUserAccountState.UserAlreadyExists)
                    {
                        await Manager.SimpleDialogMessage("Alert", "You have been logged in!", "OK");
                        Manager.ActiveLibraryUser = LibraryUserTry.Item1;
                    }
                    else if (LibraryUserTry.Item2 == LibraryUserAccountState.UserCanBeCreated)
                    {
                        bool CanCreateAccount = await Application.Current.MainPage.DisplayAlert("Question?", "Would you like to register an account?", "Yes", "No");
                        if (CanCreateAccount)
                        {
                            await DatabaseMan.CreateLibraryUser(UserNick, Int32.Parse(UserPas));
                            await Manager.SimpleDialogMessage("Alert", "The account has been created, you may log in now!", "OK");
                        }
                        else
                        {
                            await Manager.SimpleDialogMessage("Alert", "See you soon!", "OK");
                        }
                    }
                    else
                    {
                        await Manager.SimpleDialogMessage("Alert", "An error occured, please try another combination!", "OK");
                    }
                }
            }

            UserNick = string.Empty;
            UserPas = string.Empty;

            NavigationMan.NavigateTo("/", true);
        }

        private void TryUserLogout()
        {
            Manager.ClearActiveLibraryUser();
            NavigationMan.NavigateTo("/", true);
        }
    }
}