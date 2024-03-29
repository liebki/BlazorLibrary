﻿using System.Diagnostics;

using BlazorLibrary.Models;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages
{
    partial class PrintGame
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string SpielId { get; set; }

        public LibraryGame spiel { get; set; } = null;
        private string ImageBase64Data { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            spiel = await _db.GetGameById(Int32.Parse(SpielId));
        }

        public void AbbrechenAction()
        {
            navMan.NavigateTo("/anzeige", true);
        }

        public async Task DruckeSpiel()
        {
            if (ImageBase64Data?.Length > 0 && ImageBase64Data != string.Empty)
            {
                try
                {
                    ImageBase64Data = ImageBase64Data.Replace("data:image/png;base64,", string.Empty);
                    DirectoryInfo tempDir = Directory.CreateDirectory(Path.Combine($"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Temp", "BlazorGameLibraryImageDump"));
                    Guid randomImageName = Guid.NewGuid();

                    string imagePath = Path.Combine(tempDir.FullName, $"{randomImageName}.png");
                    byte[] imgByteArray = Convert.FromBase64String(ImageBase64Data);

                    File.WriteAllBytes(imagePath, imgByteArray);
                    Process printMspaintCmd = new();

                    printMspaintCmd.StartInfo.FileName = "mspaint";
                    printMspaintCmd.StartInfo.Arguments = $"/pt {imagePath}";

                    printMspaintCmd.Start();
                    File.Delete(imagePath);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}