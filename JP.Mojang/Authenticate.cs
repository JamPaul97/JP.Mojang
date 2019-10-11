using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JP.Mojang
{
	public static class Authenticate
	{
		private static string URL = "https://authserver.mojang.com/authenticate";
		private static APIResponse lastResponse;
		/// <summary>
		/// The Last Response the server sended.
		/// </summary>
		public static APIResponse LastResponse
		{
			get { return lastResponse; }
		}
		/// <summary>
		/// A Function to authenticate a player to Mojang's API. Username and Password must be valid. If the process was succesfull then you can access LastResponse to get any infromation available.
		/// </summary>
		/// <param name="username">The userame or email to Authenticate</param>
		/// <param name="password">The user's password for the current account</param>
		/// <returns>Returns true if the Authentication proccess was succefull.</returns>
		public static async Task<bool> Try(string username, string password)
		{
			lastResponse = null;

			var jsonRequest = new AuthenticateJSON
			{
				username = username,
				password = password,
				requestUser = true,
				agent = new Agent
				{
					name = "Minecraft",
					version = 1

				}
			};
			string responseString;
			try
			{
				responseString = await NetConnector.MakeRequest(jsonRequest, "https://authserver.mojang.com/authenticate");
			}
			catch
			{
				return false;
			}

			lastResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
			return true;
		}
		/// <summary>
		/// A Function to authenticate a player to Mojang's API. Username and Password must be valid. If the process was succesfull then you can access LastResponse to get any infromation available.
		/// </summary>
		/// <param name="username">The userame or email to Authenticate</param>
		/// <param name="password">The user's password for the current account</param>
		/// <param name="guid">A guid for the current machine</param>
		/// <returns>Returns true if the Authentication proccess was succefull.</returns>
		public static async Task<bool> Try(string username, string password, string guid)
		{


			var jsonRequest = new AuthenticateJSON
			{
				username = username,
				password = password,
				requestUser = true,
				clientToken = guid,
				agent = new Agent
				{
					name = "Minecraft",
					version = 1

				}
			};
			string responseString;
			try
			{
				responseString = await NetConnector.MakeRequest(jsonRequest, "https://authserver.mojang.com/authenticate");
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
		private class AuthenticateJSON
		{
			public string username;
			public string password;
			public string clientToken;
			public bool requestUser;
			public Agent agent;
		}
		private class Agent
		{
			public string name;
			public int version;
		}
		private class ValidateTemplate
		{
			public string accessToken;
			public string clientToken;
		}
	}
}
