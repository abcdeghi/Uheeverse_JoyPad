using System.Collections.Generic;
using System.IO;

public static class EnvLoader
{
    private static Dictionary<string, string> envVars;

    public static void Load(string path = ".env")
    {
        envVars = new Dictionary<string, string>();
        if (!File.Exists(path)) return;

        foreach (var line in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
            var parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                envVars[parts[0].Trim()] = parts[1].Trim();
            }
        }
    }

    public static string Get(string key, string defaultValue = null)
    {
        if (envVars != null && envVars.ContainsKey(key))
            return envVars[key];
        return defaultValue;
    }
}
