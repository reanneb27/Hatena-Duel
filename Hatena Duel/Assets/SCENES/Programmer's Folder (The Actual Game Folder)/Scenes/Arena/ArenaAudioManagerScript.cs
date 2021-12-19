using UnityEngine.Audio;
using UnityEngine;
using System;

public class ArenaAudioManagerScript : MonoBehaviour
{
    public Sound[] Sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * Settings.SFXVolume;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        s.source.loop = s.loop;
        s.source.volume = s.volume * Settings.SFXVolume;
        s.source.Play();
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(Sounds, item => item.name == sound);
        s.source.Stop();
    }
}
