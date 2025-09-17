using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
using AOT;
#endif

public static class SaveSystem
{
    private static readonly string SaveFilePath =
        Path.Combine(Application.persistentDataPath, "save.json");

    public static bool SaveExists() => File.Exists(SaveFilePath);

    /// <summary>Save data and sync to IndexedDB on WebGL</summary>
    public static void Save(SaveData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(SaveFilePath, json);

#if UNITY_WEBGL && !UNITY_EDITOR
        WebGL.FileSystem.Syncfs(err =>
        {
            if (string.IsNullOrEmpty(err))
                Debug.Log("SaveSystem Sync OK");
            else
                Debug.LogError("SaveSystem Sync error: " + err);
        });
#endif
    }

    public static SaveData Load()
    {
        if (!SaveExists()) return null;
        string json = File.ReadAllText(SaveFilePath);
        return JsonConvert.DeserializeObject<SaveData>(json);
    }

    public static void Delete()
    {
        if (SaveExists()) File.Delete(SaveFilePath);
    }

    public static bool DataExists() => SaveExists();

    public static SaveData GenerateUserData(string name, string mail)
    {
        var data = new SaveData();
        if (!DataExists())
        {
            data.answeredQuestions = new Dictionary<string, List<OutQuestion>>
            {
                { "1", new List<OutQuestion>() },
                { "2", new List<OutQuestion>() },
                { "3", new List<OutQuestion>() },
                { "4", new List<OutQuestion>() }
            };
            data.completedLevels = new Dictionary<string, bool>();

            data.playerName = mail;
            data.playerNombre = name;
        }
        return data;
    } 
}

#if UNITY_WEBGL && !UNITY_EDITOR
namespace WebGL
{
    public static class FileSystem
    {
        [DllImport("__Internal")]
        private static extern void FileSystemSyncfs(int id);

        [DllImport("__Internal")]
        private static extern void FileSystemSyncfsAddEvent(Action<int, string> callback);

        private static readonly Dictionary<int, Action<string>> callbacks = new();
        private static int nextId = 1;

        static FileSystem()
        {
            FileSystemSyncfsAddEvent(Callback);
        }

        [MonoPInvokeCallback(typeof(Action<int, string>))]
        private static void Callback(int id, string error)
        {
            if (callbacks.TryGetValue(id, out var cb))
            {
                callbacks.Remove(id);
                cb?.Invoke(string.IsNullOrEmpty(error) ? null : error);
            }
        }

        public static void Syncfs(Action<string> cb)
        {
            int id = nextId++;
            callbacks.Add(id, cb);
            FileSystemSyncfs(id);
        }
    }
}
#else
namespace WebGL
{
    // Stub for non-WebGL platforms
    public static class FileSystem
    {
        public static void Syncfs(Action<string> cb)
        {
            cb?.Invoke(null);
        }
    }
}
#endif
