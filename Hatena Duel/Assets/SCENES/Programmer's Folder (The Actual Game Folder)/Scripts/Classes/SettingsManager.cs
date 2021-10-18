using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum KeySettingsEnum
{
    LightAtt
}

// https://videlais.com/2021/02/25/using-jsonutility-in-unity-to-save-and-load-game-data/
public static class SettingsManager
{
    public static readonly string SettingsFilePath = Application.persistentDataPath + "/settings.json";

    /// <summary>
    /// Updates the globally accessible static class Settings with the new setting values defined in the SettingsSerializer parameter and automatically writes it to settings.json file
    /// </summary>
    /// <param name="newSettings"></param>
    public static void UpdateSettings(SettingsSerializer newSettings)
    {
        Settings.BGMusicVolume = newSettings.BGMusicVolume;
        Settings.SFXVolume = newSettings.SFXVolume;

        Settings.LeftKey = newSettings.LeftKey;
        Settings.JumpKey = newSettings.JumpKey;
        Settings.RightKey = newSettings.RightKey;
        Settings.GroundKey = newSettings.GroundKey;

        Settings.LightAttKey = newSettings.LightAttKey;
        Settings.HeavyAttKey = newSettings.HeavyAttKey;
        Settings.UltimateAttKey = newSettings.UltimateAttKey;

        if (Settings.Fullscreen == newSettings.Fullscreen)
        {
            if (Settings.Fullscreen)
                Screen.SetResolution(3200, 1800, FullScreenMode.FullScreenWindow);
            else
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        Settings.Fullscreen = newSettings.Fullscreen;

        SaveSettingsToJsonFile();
    }
    public static bool IsValidKeySettingChange(KeyCode newKey, SettingsSerializer newSetting = null)
    {
        if (newSetting == null)
            newSetting = SettingsSerializer.CreateFromSettings();
        return !UsedKeys(newSetting).Contains(newKey);
    }
    private static void SaveSettingsToJsonFile()
    {
        File.WriteAllText(SettingsFilePath, SettingsSerializer.CreateFromSettings().ToJsonString());
    }
    private static HashSet<KeyCode> UsedKeys(SettingsSerializer setting)
    {
        HashSet<KeyCode> usedKeys = new HashSet<KeyCode>();
        usedKeys.Add(setting.LeftKey);
        usedKeys.Add(setting.JumpKey);
        usedKeys.Add(setting.RightKey);
        usedKeys.Add(setting.GroundKey);
        usedKeys.Add(setting.LightAttKey);
        usedKeys.Add(setting.HeavyAttKey);
        usedKeys.Add(setting.UltimateAttKey);
        return usedKeys;
    }
}

// https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html
// https://docs.unity3d.com/ScriptReference/JsonUtility.ToJson.html
[System.Serializable]
public class SettingsSerializer
{
    public float BGMusicVolume;
    public float SFXVolume;

    public KeyCode LeftKey;
    public KeyCode JumpKey;
    public KeyCode RightKey;
    public KeyCode GroundKey;

    public KeyCode LightAttKey;
    public KeyCode HeavyAttKey;
    public KeyCode UltimateAttKey;

    public bool Fullscreen;

    public string ToJsonString()
    {
        return JsonUtility.ToJson(this);
    }
    public static SettingsSerializer CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SettingsSerializer>(jsonString);
    }
    public static SettingsSerializer CreateFromSettings()
    {
        return new SettingsSerializer()
        {
            BGMusicVolume = Settings.BGMusicVolume,
            SFXVolume = Settings.SFXVolume,

            LeftKey = Settings.LeftKey,
            JumpKey = Settings.JumpKey,
            RightKey = Settings.RightKey,
            GroundKey = Settings.GroundKey,

            LightAttKey = Settings.LightAttKey,
            HeavyAttKey = Settings.HeavyAttKey,
            UltimateAttKey = Settings.UltimateAttKey,

            Fullscreen = Settings.Fullscreen
        };
    }

}


