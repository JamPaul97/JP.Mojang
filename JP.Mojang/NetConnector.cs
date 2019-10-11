using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JP.Mojang
{
    public static class NetConnector
    {
		private static HttpClient _client = new HttpClient();
		/// <summary>
		/// Function to help make the Request to Mojang's Servers
		/// </summary>
		/// <param name="obj">The Post object.</param>
		/// <param name="url">The user to Post.</param>
		/// <returns></returns>
		internal static async Task<string> MakeRequest(dynamic obj, string url)
		{
			var content = new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(url, content);
			var responseString = await response.Content.ReadAsStringAsync();
			return responseString;
		}
	}
}
