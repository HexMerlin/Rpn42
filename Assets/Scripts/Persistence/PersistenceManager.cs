using UnityEngine;
using System;
using System.IO;
using System.Text.Json;

public static class PersistenceManager
{
    private static string DataFilePath => Path.Combine(Application.persistentDataPath, "settings.json");

    public static SavedData LoadData()
    {
        Debug.Log($"Read file {DataFilePath}");
        try
        {
            string jsonString = File.ReadAllText(DataFilePath);

            var options = new JsonSerializerOptions
            {
                Converters = { new SavedDataJsonConverter() },
                WriteIndented = true // Optional
            };

            SavedData savedData = JsonSerializer.Deserialize<SavedData>(jsonString, options);

            return savedData;
        }
        catch (Exception e)
        {
            Debug.Log($"Could not read {DataFilePath}. {e.Message}");
        }

        return new SavedData(); // failed load: return a new default object
    }

    public static void SaveData(SavedData savedData)
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new SavedDataJsonConverter() },
            WriteIndented = true // Optional
        };
        Debug.Log($"---------------------------------------Write file {DataFilePath}");
        string jsonString = JsonSerializer.Serialize(savedData, options);
        File.WriteAllText(DataFilePath, jsonString);
    }
}
