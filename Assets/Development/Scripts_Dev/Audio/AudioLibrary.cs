using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Library/AudioLibrary", order = 32)]
public class AudioLibrary : ScriptableObject
{
    [Header("Sound Effects")]
    [SerializeField] public List<Sound> sounds = new List<Sound>();

    [Space][Header("Music")]
    [SerializeField] public List<MusicAsset> music = new List<MusicAsset>();
}

[System.Serializable]
public class Sound
{
    public string id;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;

    [HideInInspector] public AudioSource source;

    public void SetAudioSource(AudioSource audioSource)
    {
        this.source = audioSource;
        source.clip = clip;
    }

    public void SetPitchVolume()
    {
        source.volume = volume; //* GlobalSettings.SFXVolume;
        source.pitch = pitch;
    }
}