using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRang = .95f;
    public float highPitchRang = 1.05f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingel(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play(); 
    }

    public void RandomizeSfx (params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRang, highPitchRang);
       
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}
