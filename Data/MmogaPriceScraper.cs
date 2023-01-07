using HtmlAgilityPack;

namespace BlazorLibrary.Data
{
    public static class MmogaPriceScraper
    {
        private const string MmogaSearchUrl = "https://www.mmoga.de/advanced_search.php?keywords=";

        public static string GetPreisVonSpiel(string gamename)
        {
            string GameName = FilteNameInput(gamename);
            string ProductEndPrice = null;

            List<string> ProductPriceList = new();
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(MauiProgram.Einstellungen.Pricescraperuseragent);
                string WebContentResult = string.Empty;

                using (HttpResponseMessage response = client.GetAsync($"{MmogaSearchUrl}{GameName}").Result)
                {
                    WebContentResult = GetUrlResponseString(response);
                }
                if (!string.IsNullOrEmpty(WebContentResult) && SearchResultChecker(WebContentResult))
                {
                    HtmlDocument doc = new();
                    doc.LoadHtml(WebContentResult);

                    int ProductCount = 0;
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='searchCont']"))
                    {
                        ProductCount++;
                        foreach (HtmlNode node2 in doc.DocumentNode.SelectNodes($"/html/body/div[2]/div/div[2]/div[2]/div[{ProductCount}]/ul/li[3]"))
                        {
                            CheckPriceOfProduct(ProductPriceList, node2);
                        }
                    }
                    if (ProductPriceList.Count != ProductCount)
                    {
                        RemoveOtherListings(ProductPriceList, ProductCount);
                    }
                }
                if (ProductPriceList.Count > 0)
                {
                    ProductEndPrice = ProductPriceList[0];
                }
            }
            return ProductEndPrice;
        }

        private static string FilteNameInput(string GameName)
        {
            if (GameName.Contains(' '))
            {
                GameName = GameName.Replace(" ", "+");
            }
            if (GameName.Contains('-'))
            {
                GameName = GameName.Replace("-", " ");
            }

            return GameName;
        }

        private static void CheckPriceOfProduct(List<string> ProductPriceList, HtmlNode node2)
        {
            string Price = node2.InnerText;
            Price = Price.Replace("&nbsp;&euro;", string.Empty);

            if (Price.Contains(' '))
            {
                string[] PriceWithOfferPrice = Price.Split(' ');
                Price = PriceWithOfferPrice[1];
            }
            Price = String.Concat(Price.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            if (!string.IsNullOrWhiteSpace(Price))
            {
                ProductPriceList.Add(Price);
            }
        }

        private static string GetUrlResponseString(HttpResponseMessage response)
        {
            string WebContentResult;
            using (HttpContent content = response.Content)
            {
                WebContentResult = content.ReadAsStringAsync().Result;
            }

            return WebContentResult;
        }

        private static void RemoveOtherListings(List<string> ProductPriceList, int ProductCount)
        {
            int ProductCountDif = (ProductPriceList.Count - ProductCount);
            for (int i = 0; i < ProductCountDif; i++)
            {
                ProductPriceList.RemoveAt(i);
            }
        }

        private static bool SearchResultChecker(string webcontent)
        {
            if (webcontent.Contains("Ihre Suche nach") && webcontent.Contains("lieferte keine Ergebnisse"))
            {
                return false;
            }
            return true;
        }
    }
}