using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

public static class SaveManager
{
    public static void Save(string path, string json)
    {
        if (path == "")
            return;

        File.WriteAllText(path, json);
    }

    public static bool Load<T>(string path, out T data) where T : ILoadDatable
    {
        data = default(T);

        if (path == "")
            return false;

        try
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);

            if (data == null || data.IsValid())
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }
}