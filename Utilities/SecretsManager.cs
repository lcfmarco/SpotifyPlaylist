using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace SpotifyPlaylist.Utilities
{

  public static class SecretsManager
  {



    // Custom method to load secrets from secrets.json
    public static Dictionary<string, string> LoadSecrets()
    {
      // Path to the secrets.json file (in the project directory)
      string filePath = "secrets.json";

      if (!File.Exists(filePath))
      {
        throw new FileNotFoundException($"The file {filePath} was not found.");
      }

      // Read the file and parse it into a dictionary
      string json = File.ReadAllText(filePath);
      var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

      if (secrets == null)
      {
        throw new Exception("Failed to deserialize secrets.json");
      }

      return secrets;
    }

  }

}