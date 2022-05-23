using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

     public Audio[] audios;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach (var aud in audios)
        {
            aud.source = gameObject.AddComponent<AudioSource>();
            aud.source.volume = aud.volume;
            aud.source.pitch = aud.pitch;
            aud.source.clip = aud.clip;
        }
    }

    //this function can be called whereever, when we need an audio to play...
    public void PlayAudio(string name)
    {
        print(name);
        Audio audio = Array.Find(audios, audio => audio.name == name);
        if (audio.source.isPlaying == false)
            audio.source.Play();
    }


    //this class holds the details of an audio...
    [System.Serializable]
    public class Audio
    {
        public string name;
        public AudioClip clip;
        public AudioSource source;
        [Range(0.1f,1)]
        public float volume = 1;
        public float pitch = 1;
    }
}
