using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JP.Mojang
{
	public static class Refresh
	{
		private const string URL = "https://authserver.mojang.com/authenticate";
		private static APIResponse lastResponse;
		/// <summary>
		/// The Last Response the server sended.
		/// </summary>
		public static APIResponse LastResponse
		{
			get { return lastResponse; }
		}
		/// <summary>
		/// Refreshes a valid accessToken. It can user to keep the user logged in.
		/// </summary>
		/// <param name="accessToken">The accessToken to be refreshed</param>
		/// <param name="requestUser">If true the user object will be returned</param>
		/// <returns>True if the Refreshed proccess was succefull.</returns>
		public static async Task<bool> Try(string accessToken, bool requestUser)
		{
			lastResponse = null;

			var jsonRequest = new Payload
			{
				accessToken = accessToken,
				requestUser = requestUser,
			};
			string responseString;
			try
			{
				responseString = await NetConnector.MakeRequest(jsonRequest, URL);
			}
			catch
			{
				return false;
			}

			lastResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
			return true;

		}
		/// <summary>
		/// Refreshes a valid accessToken. It can user to keep the user logged in.
		/// </summary>
		/// <param name="accessToken">The accessToken to be refreshed</param>
		/// <param name="requestUser">If true the user object will be returned</param>
		/// <param name="clientToken">This needs to be identical to the one used to obtain the accessToken in the first place. Leave empty if unknown</param>
		/// <returns>True if the Refreshed proccess was succefull.</returns>
		public static async Task<bool> Try(string accessToken, string clientToken, bool requestUser)
		{


			var jsonRequest = new Payload
			{
				accessToken = accessToken,
				clientToken = clientToken,
				requestUser = requestUser,
			};
			string responseString;
			try
			{
				responseString = await NetConnector.MakeRequest(jsonRequest, URL);
			}
			catch
			{
				return false;
			}

			lastResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
			return true;

		}
		public class APIResponse
		{
			public string accessToken;
			public string clientToken;
			public List<Profile> availableProfiles;
			public Profile selectedProfile;
			public User user;


			public class User
			{
				public string id;
				public List<Property> properties;

				public class Property
				{
					public string name;
					public string value;
				}

			}

			public class Profile
			{
				public string id;
				public string name;
				public bool legacy;
			}
		}
		private class Payload
		{
			public string accessToken;
			public string clientToken;
			public bool requestUser;
		}
	}
}
