using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicScript : MonoBehaviour
{
    public AudioSource BGMusic;
    public void SetVolume(float volume)
    {
        BGMusic.volume = volume;
    }
    private void Update()
    {
        SetVolume(Settings.BGMusicVolume);
    }
}
