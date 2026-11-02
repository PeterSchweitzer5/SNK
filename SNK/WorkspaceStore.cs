using System.Text.Json;

namespace SNK;

public static class WorkspaceStore
{
    private static string Folder => Path.Combine(AppContext.BaseDirectory, "workspaces");

    private static string GetPath(string name) => Path.Combine(Folder, name + ".json");

    public static bool Exists(string name) => File.Exists(GetPath(name));

    public static Workspace? Load(string name)
    {
        if (!Exists(name))
        {
            return null;
        }

        string json = File.ReadAllText(GetPath(name));
        return JsonSerializer.Deserialize<Workspace>(json);
    }

    public static void Save(string name, Workspace workspace)
    {
        Directory.CreateDirectory(Folder);
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(GetPath(name), JsonSerializer.Serialize(workspace, options));
    }

    public static void Delete(string name) => File.Delete(GetPath(name));

    public static List<string> ListNames()
    {
        List<string> names = new();

        if (!Directory.Exists(Folder))
        {
            return names;
        }

        foreach (string file in Directory.GetFiles(Folder, "*.json"))
        {
            names.Add(Path.GetFileNameWithoutExtension(file));
        }

        return names;
    }
}