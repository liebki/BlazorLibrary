using Newtonsoft.Json;

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
	}
}