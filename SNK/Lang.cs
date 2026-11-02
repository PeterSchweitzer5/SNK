using YamlDotNet.Serialization;

namespace SNK;

public static class Lang
{
    private static Dictionary<string, string> texts = new();

    public static void Load(string language)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "lang", language + ".yml");

        if (!File.Exists(path))
        {
            path = Path.Combine(AppContext.BaseDirectory, "lang", "en.yml");
        }

        if (!File.Exists(path))
        {
            texts = new();
            return;
        }

        string yaml = File.ReadAllText(path);
        texts = new DeserializerBuilder().Build().Deserialize<Dictionary<string, string>>(yaml);
    }

    public static string T(string key, params object[] values)
    {
        if (!texts.TryGetValue(key, out string? text))
        {
            return key;
        }

        return string.Format(text, values);
    }
}