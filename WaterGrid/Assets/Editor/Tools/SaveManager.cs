using System;
using System.IO;

public static class SaveManager
{
    public static void SaveMap(string path, string json)
    {
        if (path == "")
            return;

        File.WriteAllText(path, json);
    }

    public static void LoadMap(string path, Action<string> Load)
    {
        if (path == "")
            return;

        string json = File.ReadAllText(path);
        Load.Invoke(json);
    }
}