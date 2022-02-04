using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTests
{
    public class ResponseHelper
    {
        private HttpClient client = new HttpClient();

        public async Task<string> GetResponseContentAsync(string url, HttpMethod httpMethod)
        {
            var responseForNow = await client.SendAsync(new HttpRequestMessage(httpMethod, url));

            return await responseForNow.Content.ReadAsStringAsync();
        }

        public JsonElement.ArrayEnumerator GetArrayEnumerator(string requestContent)
        {
            var jsonDocForNow = JsonDocument.Parse(requestContent);

            return jsonDocForNow.RootElement.EnumerateArray();
        }
    }
}
