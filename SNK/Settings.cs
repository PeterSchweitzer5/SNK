using System.Text.Json;

namespace SNK;

public class Settings
{
    public string Language { get; set; } = "en";

    private static string FilePath => Path.Combine(AppContext.BaseDirectory, "settings.json");

    public static Settings Load()
    {
        if (!File.Exists(FilePath))
        {
            return new Settings();
        }

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
    }

    public void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this, options));
    }
}