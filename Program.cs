using System.Security.AccessControl;
using SpotifyPlaylist.Utilities;
using SpotifyPlaylist.Services;

class Program
{

  static async Task Main(string[] args)
  {

    // var secrets = SecretsManager.LoadSecrets();
    // string clientId = secrets["SpotifyClientId"];
    // string clientSecret = secrets["SpotifyClientSecret"];
    // string redirectUri = "http://localhost:5000/callback";

    // var authService = new SpotifyAuthService(clientId, clientSecret, redirectUri);
    // var newAccessToken = await authService.RefreshAccessToken("AQCDBX5tsCO_xRtzcsLVCJVCWzspxUlh8AxFVdnf0Xdu62fdEJlYYazQpkIpdobBr6RJ8UNx-8lIhHtSTEyp9PSAaK2_leBfrhiRYQ_porq1J-inZEmEOzVmZ_JIu0wSVSY");
    // Console.WriteLine($"New Access Token: {newAccessToken}");

    var accessToken = "BQAY_5-3wyqJBTkldhDEHijT55abYIX8CQdlTDILAYhc_DRWY-YoXwGXuj54oTP0RUe52r8LmopBTTn2hIqEIDVS3jyoLJNIYAY3eQ1YWzr63bh0U-EyUI9Aj7902QXTiRHggvyqlbYpnhZDXyPooT0q-KJjNHwWJ8BaCUjHnYVo8JVkTc9Y-hGepOIAMLZpV6OfImg9RBEWcJZ-x7gxZ8L-7Fgtv_CDCGoFpHN4UBh_yggmTCAH0saIvonRuJIiygb46EEiA6MjEKhuA12wu56urfWF0jZfrfLx_8HOYNumPMY";
    var apiService = new SpotifyApiService();
    string playlistId = "1tX8T4STbwy9xnyVhU8LHL";

    Console.WriteLine($"Fetching tracks from playlist ID: {playlistId}...");

    try
    {
      var trackIds = await apiService.FetchPlaylistTracks(accessToken, playlistId);

      Console.WriteLine("\nTrack IDs:");
      foreach (var trackId in trackIds)
      {
        Console.WriteLine(trackId); // Print each track ID
      }

      Console.WriteLine($"\nTotal Tracks Fetched: {trackIds.Count}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred: {ex.Message}");
    }
  }




  //   Console.WriteLine("Fetching your 'Liked Songs'...");
  //   var likedSongIds = await apiService.FetchLikedSongs(accessToken);

  //   Console.WriteLine("\nTrack IDs:");
  //   foreach (var trackId in likedSongIds)
  //   {
  //     Console.WriteLine(trackId); // Should print only the track IDs
  //   }

  //   Console.WriteLine($"\nTotal Tracks Fetched: {likedSongIds.Count}");
  // }

  // var playlists = await apiService.FetchUserPlaylists(accessToken);

  // Console.WriteLine("Playlists:");
  //   foreach (var playlist in playlists)
  //   {
  //     Console.WriteLine($"{playlist.Name} (ID: {playlist.Id})");
  //   }





  // // Getting access token logic
  // var secrets = SecretsManager.LoadSecrets();
  // string clientId = secrets["SpotifyClientId"];
  // string clientSecret = secrets["SpotifyClientSecret"];
  // string redirectUri = "http://localhost:5000/callback";

  // var authService = new SpotifyAuthService(clientId, clientSecret, redirectUri);

  // string[] scopes = { "playlist-read-private", "playlist-modify-public", "playlist-modify-private" };

  // string loginUrl = authService.GenerateLoginUrl(scopes);

  // Console.WriteLine("Spotify Login URL:");
  // Console.WriteLine(loginUrl);

  // Console.WriteLine("\nCopy the above URL into your browser, log in, and authorize the app.");
  // Console.WriteLine("After authorization, Spotify will redirect you to the redirect URI with a 'code' parameter.");
  // Console.WriteLine("Paste the 'code' value here:");
  // string code = Console.ReadLine();

  // try
  // {
  //   string accessToken = await authService.ExchangeCodeForToken(code);
  //   Console.WriteLine("\nAccess Token:");
  //   Console.WriteLine(accessToken);
  // }
  // catch (Exception ex)
  // {
  //   Console.WriteLine($"Error during token exchange: {ex.Message}");
  // }


}


