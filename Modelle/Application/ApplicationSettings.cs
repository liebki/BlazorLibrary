
using Newtonsoft.Json;

using BlazorLibrary.Management;

namespace BlazorLibrary.Modelle.Application
{
    public class ApplicationSettings
    {
        [JsonProperty("rawgapikey")]
        public string Rawgapikey { get; set; }

        [JsonProperty("sqlitedatabasename")]
        public string Sqlitedatabasename { get; set; }

        [JsonProperty("userawg")]
        public bool Userawg { get; set; }

        [JsonProperty("pricescrapermode")]
        public string Pricescrapermode { get; set; }

        [JsonProperty("pricescraperuseragent")]
        public string Pricescraperuseragent { get; set; }

        [JsonProperty("usepricescraper")]
        public bool Usepricescraper { get; set; }

        [JsonProperty("displaydeveloper")]
        public bool Displaydeveloper { get; set; }

        public bool Save(bool TransferToActiveSettings = false)
        {
            string SettingsJson = Manager.GetSettingsAsJson(this);

            if (TransferToActiveSettings)
            {
                MauiProgram.Einstellungen = this;
            }

            if (!string.IsNullOrEmpty(SettingsJson))
            {
                File.WriteAllText(Path.Combine(Manager.MauiProgramActiveDirectory(), "ApplicationSettingsFile.json"), SettingsJson);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{{{nameof(Rawgapikey)}={Rawgapikey}, {nameof(Sqlitedatabasename)}={Sqlitedatabasename}, {nameof(Userawg)}={Userawg.ToString()}, {nameof(Pricescrapermode)}={Pricescrapermode}, {nameof(Pricescraperuseragent)}={Pricescraperuseragent}, {nameof(Usepricescraper)}={Usepricescraper.ToString()}, {nameof(Displaydeveloper)}={Displaydeveloper.ToString()}}}";
        }
    }
}