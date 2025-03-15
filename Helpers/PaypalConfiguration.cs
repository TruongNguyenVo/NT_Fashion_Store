namespace NT_Fashion_Store.Helpers;
using PayPal.Api;
using System.Collections.Generic;

public static class PaypalConfiguration
{
	public static Dictionary<string, string> GetConfig(string mode)
	{
		return new Dictionary<string, string>()
		{
			{"mode", mode}
		};
	}

	private static string GetAccessToken(string clientId, string clientSecret, string mode)
	{
		return new OAuthTokenCredential(clientId, clientSecret, GetConfig(mode)).GetAccessToken();
	}

	public static APIContext GetAPIContext(string clientId, string clientSecret, string mode)
	{
		var apiContext = new APIContext(GetAccessToken(clientId, clientSecret, mode));
		apiContext.Config = GetConfig(mode);
		return apiContext;
	}
}

