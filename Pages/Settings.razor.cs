using System;
using MudBlazor;

using System.Text;
using BlazorLibrary.Management;

using Microsoft.AspNetCore.Components;

using BlazorLibrary.Modelle.Application;

namespace BlazorLibrary.Pages
{
    partial class Settings
    {
        [Inject]
        private NavigationManager NavMan { get; set; }
        private MudBlazor.Color ShowHideButtonColor { get; set; } = MudBlazor.Color.Warning;
        private InputType ShowHideState { get; set; } = InputType.Password;
        private string ShowHideButtonText { get; set; } = "Show Api-Key";
        private ApplicationSettings EinstellungenBackup { get; set; }


        protected override async Task OnInitializedAsync()
        {
            EinstellungenBackup = new()
            {
                Pricescraperuseragent = MauiProgram.Einstellungen.Pricescraperuseragent,
                Rawgapikey = MauiProgram.Einstellungen.Rawgapikey,
                Sqlitedatabasename = MauiProgram.Einstellungen.Sqlitedatabasename,
                Userawg = MauiProgram.Einstellungen.Userawg,
                Usepricescraper = MauiProgram.Einstellungen.Usepricescraper,
                Displaydeveloper = true,
                Pricescrapermode = "mmoga"
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
            if (string.IsNullOrEmpty(EinstellungenBackup.Pricescraperuseragent) || EinstellungenBackup.Pricescraperuseragent.Length < 10)
            {
                FieldErrors.AppendLine("Useragent was empty or too short, changed to default one");
                EinstellungenBackup.Pricescraperuseragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0";
            }

            if (string.IsNullOrEmpty(EinstellungenBackup.Pricescrapermode) || !EinstellungenBackup.Pricescrapermode.Contains("mmoga"))
            {
                FieldErrors.AppendLine("The pricescraper mode cant be changed yet, changed to default one");
                EinstellungenBackup.Pricescrapermode = "mmoga";
            }

            if (string.IsNullOrEmpty(EinstellungenBackup.Sqlitedatabasename) || !SqliteDatabasenameIsOnlyLetters() || EinstellungenBackup.Sqlitedatabasename.Length < 3)
            {
                FieldErrors.AppendLine("The name for the sqlite database was corrupt or too short, changed to default one");
                EinstellungenBackup.Sqlitedatabasename = "blazorgamelibrary";
            }

            if (EinstellungenBackup.Userawg && (string.IsNullOrEmpty(EinstellungenBackup.Rawgapikey) || EinstellungenBackup.Rawgapikey.Length < 10 && !EinstellungenBackup.Rawgapikey.ToUpper().Contains("API-KEY FROM RAWG.IO")))
            {
                FieldErrors.AppendLine("The Api-Key was too short or empty, changed to default value and disabled the userawg option");
                EinstellungenBackup.Pricescraperuseragent = "API-KEY FROM RAWG.IO";
                EinstellungenBackup.Userawg = false;
            }

            return FieldErrors;
        }

        private bool SqliteDatabasenameIsOnlyLetters()
        {
            return EinstellungenBackup.Sqlitedatabasename.All(Char.IsLetter);
        }

        private async Task SaveSettingsToFile()
        {
            StringBuilder FieldErrorResult = VerifyFields();
            bool CloseLibraryAfterRename = false;

            if (FieldErrorResult.Length > 1)
            {
                await Manager.MauiDialog("Information", $"The settings are not gonna be saved as you wish, check this:{Environment.NewLine}Errors: {FieldErrorResult}");
            }

            if (!EinstellungenBackup.Sqlitedatabasename.Equals(MauiProgram.Einstellungen.Sqlitedatabasename, StringComparison.OrdinalIgnoreCase))
            {
                bool RenameDatabaseForUser = await Application.Current.MainPage.DisplayAlert("Important decision", "The database name changed, would you like to have your database file renamed?", "Yes", "No");

                if (RenameDatabaseForUser)
                {
                    string OldDatabasePath = Path.Combine(Manager.MauiProgramActiveDirectory(), $"{MauiProgram.Einstellungen.Sqlitedatabasename}.sqlite");
                    string NewDatabasePath = Path.Combine(Manager.MauiProgramActiveDirectory(), $"{EinstellungenBackup.Sqlitedatabasename}.sqlite");

                    File.Copy(OldDatabasePath, NewDatabasePath, true);

                    await Manager.MauiDialog("Information", "The database should have been renamed, ", "OK");
                }
                else
                {
                    await Manager.MauiDialog("Information", "The settings were saved, BlazorLibrary will be closed!\nAttention: The database name changed, so a new one will be generated!");
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

            NavMan.NavigateTo("/settings", true);
        }
    }
}