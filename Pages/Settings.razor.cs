
using MudBlazor;

using System.Text;
using NavigationManagerUtils;

using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;
using BlazorLibrary.Models.Application;

namespace BlazorLibrary.Pages
{
    partial class Settings
    {
        [Inject]
        private NavManUtils NavMan { get; set; }

        private MudBlazor.Color ShowHideButtonColor { get; set; } = MudBlazor.Color.Warning;
        private InputType ShowHideState { get; set; } = InputType.Password;
        private string ShowHideButtonText { get; set; } = "Show Api-Key";
        private ApplicationSettings EinstellungenBackup { get; set; }

        protected override async Task OnInitializedAsync()
        {
            EinstellungenBackup = new()
            {
                PriceScraperUseragent = MauiProgram.Settings.PriceScraperUseragent,
                RawgApikey = MauiProgram.Settings.RawgApikey,
                Databasename = MauiProgram.Settings.Databasename,
                UseRawg = MauiProgram.Settings.UseRawg,
                UsePriceScraper = MauiProgram.Settings.UsePriceScraper,
                UseColoredNavMenu = MauiProgram.Settings.UseColoredNavMenu,
                UseCustomNavMenuColor = MauiProgram.Settings.UseCustomNavMenuColor,
                CustomColorCode = MauiProgram.Settings.CustomColorCode
            };
        }

        private void ShowHideApiKey()
        {
            if (ShowHideState == InputType.Password)
            {
                ShowHideButtonText = "Hide Api-Key";
                ShowHideState = InputType.Text;
                ShowHideButtonColor = MudBlazor.Color.Success;
            }
            else
            {
                ShowHideButtonText = "Show Api-Key";
                ShowHideState = InputType.Password;
                ShowHideButtonColor = MudBlazor.Color.Warning;
            }
        }

        private StringBuilder VerifyFields()
        {
            StringBuilder FieldErrors = new();
            if (string.IsNullOrEmpty(EinstellungenBackup.PriceScraperUseragent) || EinstellungenBackup.PriceScraperUseragent.Length < 10)
            {
                FieldErrors.AppendLine("Useragent was empty or too short, changed to default one");
                EinstellungenBackup.PriceScraperUseragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0";
            }

            if (string.IsNullOrEmpty(EinstellungenBackup.Databasename) || !IsDatabasenameOnlyLetters() || EinstellungenBackup.Databasename.Length < 3)
            {
                FieldErrors.AppendLine("The name for the sqlite database was corrupt or too short, changed to default one");
                EinstellungenBackup.Databasename = "blazorgamelibrary";
            }

            if (EinstellungenBackup.UseRawg && (string.IsNullOrEmpty(EinstellungenBackup.RawgApikey) || EinstellungenBackup.RawgApikey.Length < 10 && !EinstellungenBackup.RawgApikey.ToUpper().Contains("API-KEY FROM RAWG.IO")))
            {
                FieldErrors.AppendLine("The Api-Key was too short or empty, changed to default value and disabled the userawg option");
                EinstellungenBackup.PriceScraperUseragent = "API-KEY FROM RAWG.IO";
                EinstellungenBackup.UseRawg = false;
            }

            if (EinstellungenBackup.UseColoredNavMenu && EinstellungenBackup.UseCustomNavMenuColor)
            {
                FieldErrors.AppendLine("You cant use the colored NavMenu, when using the the custom NavMenu color option");
                EinstellungenBackup.UseCustomNavMenuColor = false;
            }

            if (EinstellungenBackup.UseCustomNavMenuColor && string.IsNullOrEmpty(EinstellungenBackup.CustomColorCode) && !EinstellungenBackup.CustomColorCode.Contains('#'))
            {
                FieldErrors.AppendLine("You need to fill the custom color field, to use the custom colored NavMenu option");
                EinstellungenBackup.UseColoredNavMenu = true;
                EinstellungenBackup.UseCustomNavMenuColor = false;
            }

            return FieldErrors;
        }

        private bool IsDatabasenameOnlyLetters()
        {
            return EinstellungenBackup.Databasename.All(Char.IsLetter);
        }

        private async Task SaveSettingsToFile()
        {
            StringBuilder FieldErrorResult = VerifyFields();
            bool CloseLibraryAfterRename = false;

            if (FieldErrorResult.Length > 1)
            {
                await Manager.SimpleDialogMessage("Information", $"The settings are not gonna be saved as you wish, check this:{Environment.NewLine}Errors: {FieldErrorResult}");
            }

            if (!EinstellungenBackup.Databasename.Equals(MauiProgram.Settings.Databasename, StringComparison.OrdinalIgnoreCase))
            {
                bool DoRenameDatabaseForUser = await Application.Current.MainPage.DisplayAlert("Important decision", "The database name changed, would you like to have your database file renamed?", "Yes", "No");

                if (DoRenameDatabaseForUser)
                {
                    string OldDatabasePath = Path.Combine(Manager.GetExecutionDirectory(), $"{MauiProgram.Settings.Databasename}.sqlite");
                    string NewDatabasePath = Path.Combine(Manager.GetExecutionDirectory(), $"{EinstellungenBackup.Databasename}.sqlite");

                    File.Copy(OldDatabasePath, NewDatabasePath, true);

                    await Manager.SimpleDialogMessage("Information", "The database should have been renamed, ", "OK");
                }
                else
                {
                    await Manager.SimpleDialogMessage("Information", "The settings were saved, BlazorLibrary will be closed!\nAttention: The database name changed, so a new one will be generated!");
                    CloseLibraryAfterRename = true;
                }
            }

            if (EinstellungenBackup.Save(true))
            {
                MauiProgram.SetupLibrary();
            }

            if (CloseLibraryAfterRename)
            {
                Environment.Exit(0);
            }

            NavMan.Reload();
        }
    }
}