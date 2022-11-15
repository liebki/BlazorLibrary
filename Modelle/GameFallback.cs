using Newtonsoft.Json;

namespace BlazorLibrary.Modelle
{
	public partial class GameFallback
	{
		[JsonProperty("redirect")]
		public bool Redirect { get; set; }

		[JsonProperty("slug")]
		public string Slug { get; set; }
	}
}