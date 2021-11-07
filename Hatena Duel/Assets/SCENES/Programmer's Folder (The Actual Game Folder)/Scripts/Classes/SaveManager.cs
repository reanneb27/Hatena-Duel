using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static readonly string SaveFilePath = Application.persistentDataPath + "/save.json";

    /// <summary>
    /// Updates the globally accessible static class Save with the new save values defined in the SaveSerializer parameter and automatically writes it to save.json file
    /// </summary>
    /// <param name="newSettings"></param>
    public static void UpdateSave(SaveSerializer newSave)
    {
        Save.HeavenStageCompleted = newSave.HeavenStageCompleted;
        Save.VoidStageCompleted = newSave.VoidStageCompleted;

        UpdateSaveToJsonFile();
    }
    private static void UpdateSaveToJsonFile()
    {
        File.WriteAllText(SaveFilePath, SaveSerializer.CreateFromSettings().ToJsonString());
    }
}

[System.Serializable]
public class SaveSerializer
{
    public bool HeavenStageCompleted = false;
    public bool VoidStageCompleted = false;

    public string ToJsonString()
    {
        return JsonUtility.ToJson(this);
    }
    public static SaveSerializer CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SaveSerializer>(jsonString);
    }
    public static SaveSerializer CreateFromSettings()
    {
        return new SaveSerializer()
        {
            HeavenStageCompleted = Save.HeavenStageCompleted,
            VoidStageCompleted = Save.VoidStageCompleted
        };
    }

}
