using System.Text;
using System.Web;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace SpotifyPlaylist.Services
{

  // Defining the class
  public class SpotifyAuthService
  {
    // Properties in the class
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly string redirectUri;

    // Constructor to establish properties
    public SpotifyAuthService(string clientId, string clientSecret, string redirectUri)
    {
      this.clientId = clientId;
      this.clientSecret = clientSecret;
      this.redirectUri = redirectUri;
    }

    // Method that generates LoginUri
    public string GenerateLoginUrl(string[] scopes)
    {
      string scopeString = string.Join(" ", scopes);

      var uriBuilder = new StringBuilder("https://accounts.spotify.com/authorize?");
      uriBuilder.Append($"client_id={clientId}");
      uriBuilder.Append("&response_type=code");
      uriBuilder.Append($"&redirect_uri={HttpUtility.UrlEncode(redirectUri)}");
      uriBuilder.Append($"&scope={HttpUtility.UrlEncode(scopeString)}");

      return uriBuilder.ToString();

    }

    // Retrieves access token after logging in
    public async Task<string> ExchangeCodeForToken(string code)
    {
      var client = new RestClient("https://accounts.spotify.com/api/token");
      var request = new RestRequest("https://accounts.spotify.com/api/token", Method.Post);
      request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
      request.AddParameter("grant_type", "authorization_code");
      request.AddParameter("code", code);
      request.AddParameter("redirect_uri", redirectUri);
      request.AddParameter("client_id", clientId);
      request.AddParameter("client_secret", clientSecret);

      var response = await client.ExecuteAsync(request);
      if (response.IsSuccessful)
      {
        var json = JObject.Parse(response.Content);
        string accessToken = json["access_token"]?.ToString();
        string refreshToken = json["refresh_token"]?.ToString();

        Console.WriteLine($"Access Token: {accessToken}");
        Console.WriteLine($"Refresh Token: {refreshToken}");

        return accessToken;
      }
      else
      {
        throw new Exception($"Failed to exchange code for token: {response.Content}");
      }
    }

    public async Task<string> RefreshAccessToken(string refreshToken)
    {
      var client = new RestClient("https://accounts.spotify.com");
      var request = new RestRequest("https://accounts.spotify.com/api/token", Method.Post);

      request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
      request.AddParameter("grant_type", "refresh_token");
      request.AddParameter("refresh_token", refreshToken);
      request.AddParameter("client_id", clientId); // Replace with your client ID
      request.AddParameter("client_secret", clientSecret); // Replace with your client secret

      var response = await client.ExecuteAsync(request);

      if (response.IsSuccessful)
      {
        var json = JObject.Parse(response.Content);
        string newAccessToken = json["access_token"]?.ToString();
        Console.WriteLine($"New Access Token: {newAccessToken}");
        return newAccessToken;
      }
      else
      {
        Console.WriteLine("Error refreshing token:");
        Console.WriteLine(response.Content);
        throw new Exception("Failed to refresh access token.");
      }
    }


  }
}
