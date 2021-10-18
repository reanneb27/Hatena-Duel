using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InitialScript
{
    // https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html

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
