using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InitialScript
{
    // https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html
    [RuntimeInitializeOnLoadMethod]
    public static void SaveFileInit()
    {
        if (File.Exists(SaveManager.SaveFilePath))
        {
            string saveJsonString = File.ReadAllText(SaveManager.SaveFilePath);
            SaveSerializer save = SaveSerializer.CreateFromJSON(saveJsonString);

            SaveManager.UpdateSave(save);

            Debug.Log("Retrieved save file: \n" + save);
        }
    }

    [RuntimeInitializeOnLoadMethod]
    public static void SettingsInit()
    {
        Debug.Log(Application.persistentDataPath);

        if (File.Exists(SettingsManager.SettingsFilePath))
        {
            string controlsJsonString = File.ReadAllText(SettingsManager.SettingsFilePath);
            SettingsSerializer controls = SettingsSerializer.CreateFromJSON(controlsJsonString);

            SettingsManager.UpdateSettings(controls);

            Debug.Log("Retrieved settings: \n" + controls);
        }
    }
}
