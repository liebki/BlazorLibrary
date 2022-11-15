using MMOGAScraper;

namespace BlazorLibrary.Data
{
	public class MmogaNetManager
	{
		public string GetAveragePriceOfGame(string gamename)
		{
			MmogaScraper scraper = new(ScraperRegion.DE);
			List<LightProduct> LighProductList = scraper.QuickSearch(gamename);

			if (LighProductList.Count > 0)
			{
				IEnumerable<decimal> ProductPrices = LighProductList.Select(x => x.Price).ToList();
				decimal averagePrice = ProductPrices.AsQueryable().Average();

				return Convert.ToString(averagePrice);
			}

			return "0";
		}
	}
}