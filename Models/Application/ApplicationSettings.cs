
using Newtonsoft.Json;

using BlazorLibrary.Management;

namespace BlazorLibrary.Models.Application
{
    public class ApplicationSettings
    {
        [JsonProperty("rawgkey")]
        public string RawgApikey { get; set; }

        [JsonProperty("databasename")]
        public string Databasename { get; set; }

        [JsonProperty("userawg")]
        public bool UseRawg { get; set; }

        [JsonProperty("pricescraperuseragent")]
        public string PriceScraperUseragent { get; set; }

        [JsonProperty("usepricescraper")]
        public bool UsePriceScraper { get; set; }

        [JsonProperty("colorednav")]
        public bool UseColoredNavMenu { get; set; }

        [JsonProperty("customcolor")]
        public bool UseCustomNavMenuColor { get; set; }

        [JsonProperty("customcolorcode")]
        public string CustomColorCode { get; set; }

        public bool Save(bool ChangeRunningSettings = false)
        {
            string SettingsJson = Manager.ObjectToJson(this);

            if (ChangeRunningSettings)
            {
                MauiProgram.Settings = this;
            }

            if (!string.IsNullOrEmpty(SettingsJson))
            {
                File.WriteAllText(Path.Combine(Manager.GetExecutionDirectory(), "ApplicationSettingsFile.json"), SettingsJson);
                return true;
            }
            return false;
        }

    }
}