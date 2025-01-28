using RestSharp;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SpotifyPlaylist.Services
{
    public class SpotifyApiService
    {
        private readonly string baseUrl = "https://api.spotify.com";
        public async Task FetchUserProfile(string accessToken)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest("v1/me", Method.Get);

            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine("User Profile:");
                Console.WriteLine(response.Content);
            }
            else
            {
                Console.WriteLine("Error fetching user profile:");
                Console.WriteLine(response.Content);
            }
        }

        // Method to fetch all user playlists
        public async Task<List<(string Id, string Name)>> FetchUserPlaylists(string accessToken)
        {
            var playlists = new List<(string Id, string Name)>();
            string nextUrl = "https://api.spotify.com/v1/me/playlists";
            var client = new RestClient();

            do
            {
                var request = new RestRequest(nextUrl, Method.Get);
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    Console.WriteLine("Error fetching playlists:");
                    Console.WriteLine(response.Content);
                    break;
                }

                var json = JObject.Parse(response.Content);

                // Add playlists from this page
                playlists.AddRange(
                    json["items"]
                        .Select(p => (
                            Id: p["id"]?.ToString(),
                            Name: p["name"]?.ToString()
                        ))
                        .ToList()
                );

                // Get the next URL
                nextUrl = json["next"]?.ToString();

            } while (!string.IsNullOrEmpty(nextUrl));

            return playlists;
        }

        // Fetch all songs from the Liked songs library (not a playlist)
        public async Task<List<string>> FetchLikedSongs(string accessToken)
        {
            var likedSongIds = new List<string>();
            string nextUrl = "https://api.spotify.com/v1/me/tracks";
            var client = new RestClient();

            do
            {
                Console.WriteLine($"Fetching from URL: {nextUrl}");

                var request = new RestRequest(nextUrl, Method.Get);
                request.AddHeader("Authorization", $"Bearer {accessToken}");

                var response = await client.ExecuteAsync(request);

                // Log response status
                Console.WriteLine($"Response Status: {response.StatusCode}");

                if (!response.IsSuccessful)
                {
                    Console.WriteLine("Error fetching liked songs:");
                    Console.WriteLine(response.Content); // Log the error response content
                    break;
                }

                var json = JObject.Parse(response.Content);

                // Log JSON response
                Console.WriteLine($"Received JSON: {json}");

                // Check and log the "items" array
                if (json["items"] != null)
                {
                    likedSongIds.AddRange(
                        json["items"]
                            .Select(item => item["track"]?["id"]?.ToString())
                            .Where(id => id != null) // Filter out null IDs
                            .ToList()
                    );

                    Console.WriteLine($"Added {likedSongIds.Count} songs so far...");
                }

                nextUrl = json["next"]?.ToString(); // Get the next URL for pagination
                Console.WriteLine($"Next URL: {nextUrl}");

            } while (!string.IsNullOrEmpty(nextUrl));

            return likedSongIds;

            
            // var likedSongs = new List<string>();
            // string nextUrl = "https://api.spotify.com/v1/me/tracks";

            // var client = new RestClient();

            // do
            // {
            //     var request = new RestRequest(nextUrl, Method.Get);
            //     request.AddHeader("Authorization", $"Bearer {accessToken}");

            //     var response = await client.ExecuteAsync(request);

            //     if (!response.IsSuccessful)
            //     {
            //         Console.WriteLine("Error fetching liked songs:");
            //         Console.WriteLine(response.Content);
            //         break;
            //     }

            //     var json = JObject.Parse(response.Content);

            //     // Add liked songs from this page
            //     likedSongs.AddRange(
            //         json["items"]
            //             .Select(item => item["track"]["id"]?.ToString())
            //             .Where(id => id != null)
            //             .ToList()
            //     );

            //     // Get the next URL
            //     nextUrl = json["next"]?.ToString();

            // } while (!string.IsNullOrEmpty(nextUrl));

            // return likedSongs;
        }

        // Fetch all tracks from a selected playlist (fed by Id)

        public async Task<List<string>> FetchPlaylistTracks(string accessToken, string playlistId)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest($"v1/playlists/{playlistId}/tracks", Method.Get);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"Error fetching tracks for playlist {playlistId}:");
                Console.WriteLine(response.Content);
                return new List<string>();
            }

            var json = JObject.Parse(response.Content);
            return json["items"]
                .Select(t => t["track"]["id"]?.ToString())
                .Where(id => id != null)
                .ToList();
        }

        // Logic to select which playlist to fetch tracks from
        public async Task<List<string>> FetchTracks(string accessToken, string playlistId)
        {
            if (playlistId == "liked_songs")
            {
                // Fetch tracks from "Liked Songs"
                return await FetchLikedSongs(accessToken);
            }
            else
            {
                // Fetch tracks from a regular playlist
                return await FetchPlaylistTracks(accessToken, playlistId);
            }
        }


    }


}


