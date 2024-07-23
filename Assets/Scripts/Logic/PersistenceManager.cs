
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public static class PersistenceManager 
{
    private static string DataFilePath => Path.Combine(Application.persistentDataPath, "settings.json");

    public static SavedData LoadData()
    {        
        try
        {
            string jsonString = File.ReadAllText(DataFilePath);
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include
            };

            SavedData savedData = JsonConvert.DeserializeObject<SavedData>(jsonString, settings);

            //Debug.Log("Data loaded from: " + DataFilePath);
            //Debug.Log("First element: " + savedData.numberEntries[0].Rational.Numerator.ToString());
            return savedData;

        }
        catch (Exception e)
        {
            Debug.Log($"Could not read {DataFilePath}. {e.Message}");
        }

        return new SavedData(); //failed load: return default object
    }

    public static void SaveData(SavedData savedData)
    {

        string jsonString = JsonConvert.SerializeObject(savedData, Formatting.Indented);
        File.WriteAllText(DataFilePath, jsonString);
        Debug.Log("Data saved to: " + DataFilePath);

    }
}
