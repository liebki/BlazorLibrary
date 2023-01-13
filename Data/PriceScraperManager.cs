using HtmlAgilityPack;

namespace BlazorLibrary.Data
{
    public static class PriceScraperManager
    {
        private const string MmogaUrl = "https://www.mmoga.de/advanced_search.php?keywords=";

        public static string GetRoughPriceOfGame(string Name)
        {
            string GameNameInput = FilteNameInput(Name);
            string PriceOfGame = null;

            List<string> GamePriceList = new();
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(MauiProgram.Settings.PriceScraperUseragent);
                string WebContentResult = string.Empty;

                using (HttpResponseMessage response = client.GetAsync($"{MmogaUrl}{GameNameInput}").Result)
                {
                    WebContentResult = GetUrlResponseString(response);
                }
                if (!string.IsNullOrEmpty(WebContentResult) && IsProductsearchSuccessful(WebContentResult))
                {
                    HtmlDocument doc = new();
                    doc.LoadHtml(WebContentResult);

                    int ProductCount = 0;
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='searchCont']"))
                    {
                        ProductCount++;
                        foreach (HtmlNode node2 in doc.DocumentNode.SelectNodes($"/html/body/div[2]/div/div[2]/div[2]/div[{ProductCount}]/ul/li[3]"))
                        {
                            CheckPriceOfProduct(GamePriceList, node2);
                        }
                    }
                    if (GamePriceList.Count != ProductCount)
                    {
                        RemoveOtherListings(GamePriceList, ProductCount);
                    }
                }
                if (GamePriceList.Count > 0)
                {
                    PriceOfGame = GamePriceList[0];
                }
            }
            return PriceOfGame;
        }

        private static string FilteNameInput(string Name)
        {
            if (Name.Contains(' '))
            {
                Name = Name.Replace(" ", "+");
            }
            if (Name.Contains('-'))
            {
                Name = Name.Replace("-", " ");
            }

            return Name;
        }

        private static void CheckPriceOfProduct(List<string> GamePriceList, HtmlNode node2)
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
                GamePriceList.Add(Price);
            }
        }

        private static string GetUrlResponseString(HttpResponseMessage Response)
        {
            string WebContentResult = string.Empty;
            using (HttpContent content = Response.Content)
            {
                WebContentResult = content.ReadAsStringAsync().Result;
            }

            return WebContentResult;
        }

        private static void RemoveOtherListings(List<string> GamePriceList, int ProductCount)
        {
            int ProductCountDif = (GamePriceList.Count - ProductCount);
            for (int i = 0; i < ProductCountDif; i++)
            {
                GamePriceList.RemoveAt(i);
            }
        }

        private static bool IsProductsearchSuccessful(string Webcontent)
        {
            if (Webcontent.Contains("Ihre Suche nach") && Webcontent.Contains("lieferte keine Ergebnisse"))
            {
                return false;
            }
            return true;
        }
    }
}