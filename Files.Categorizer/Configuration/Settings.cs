using Newtonsoft.Json;

namespace Files.Categorizer.Configuration;
internal class Settings
{
    private const string StgPath = @$"Configuration\settings.json";

    public bool ExcludeEquals { get; set; }
    public string[] Excludes { get; set; }
    public KeyValuePair<string, string[]>[] Includes { get; set; }

    public Settings()
    {
        Excludes = Array.Empty<string>();
        Includes = Array.Empty<KeyValuePair<string, string[]>>();
    }

    public static Settings Instance
        => JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Path.GetFullPath(StgPath))) ?? new();
}
