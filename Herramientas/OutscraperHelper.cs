using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herramientas
{
    public class OutscraperHelper
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;

        public OutscraperHelper(string apiKey)
        {
            this.apiKey = apiKey;
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        }

        public async Task<string> GetAmazonReviews(string url, int limit, bool async)
        {
            string encodedUrl = Uri.EscapeDataString(url);
            string apiUrl = $"https://api.app.outscraper.com/amazon/reviews?query={encodedUrl}&limit={limit}&async={async}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}
