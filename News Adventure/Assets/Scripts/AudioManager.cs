using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    //menu - main theme - fire attack - fire hit - fire death - player attack cac - player attack mid - player attack range - player hit - player death 
    // - victory - lose - tornado - tree cut - tree fired - water taken - truck coming
    // Lenght = 17




    public static AudioManager instance;

    void Awake()
    {
        /*
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        */

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Game");
    }

    public void Play(string name)
    {
        Sound s  = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }

        s.source.Play();
    }
}
