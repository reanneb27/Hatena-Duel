using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManagerScript : MonoBehaviour
{
    public Sound[] Sounds;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * Settings.BGMusicVolume;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        s.source.loop = s.loop;
        s.source.volume = s.volume * Settings.BGMusicVolume;

        if (!s.source.isPlaying)
            s.source.Play();
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(Sounds, item => item.name == sound);
        s.source.Stop();
    }

    public void UpdateVolume()
    {
        foreach (Sound s in Sounds)
        {
            s.source.volume = s.volume * Settings.BGMusicVolume;
        }
    }
}
