using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance {get; private set;}

    public bool DontDestroyOnLoad;

    public static GameObject audioManager;

    public string MapBgm;

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager");
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            if (DontDestroyOnLoad)
            {
                DontDestroyOnLoad(audioManager);
            }
        }
    }
    #endregion

    #region Properties
    [Range(0.0f, 1.0f)] public float MasterVolume = 1.0f;
    [Range(0.0f, 1.0f)] public float MusicVolume = 1.0f;
    [Range(0.0f, 1.0f)] public float SoundVolume = 1.0f;

    // Todo: ambient sounds

    // Sounds and music
    [SerializeField] AudioLibrary AudioLibrary;
    [SerializeField] Transform SoundsContainer;
    [SerializeField] AudioSource MusicSource;
    Dictionary<string, Sound> AllSounds = new Dictionary<string, Sound>();
    Dictionary<string, MusicAsset> AllMusic = new Dictionary<string, MusicAsset>();

    string CurrentPlayingMusic = "";
    #endregion


    #region Functions
    private void Start()
    {
        // Load all audio assets from AudioLibrary
        for (int i = 0; i < AudioLibrary.sounds.Count; i++)
        {
            Sound s = AudioLibrary.sounds[i];
            if (s.id == "") { continue; }
            // Only add sound if an instance does not already exist
            Sound soundToAdd;
            if (!AllSounds.TryGetValue(s.id, out soundToAdd))
            {
                // Instantiate new sound
                GameObject so = new GameObject("sound_" + s.id);
                so.transform.SetParent(SoundsContainer);
                // Set sound properties
                soundToAdd= s;
                soundToAdd.SetAudioSource(so.AddComponent<AudioSource>());
                // Add to dictionary
                AllSounds.Add(s.id, soundToAdd);
            }
        }

        for (int i = 0; i < AudioLibrary.music.Count; i++)
        {
            MusicAsset s = AudioLibrary.music[i];
            MusicAsset outMusic;
            if (!AllMusic.TryGetValue(s.ID, out outMusic))
            {
                AllMusic.Add(s.ID, s);
            }
        }

        // OnSceneLoad stuff
        // UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneLoaded;
        if (MapBgm.Length > 0)
        {
            PlayBgm(MapBgm, 0.0f, 2.0f);
        }
        else
        {
            PlayBgm("defiance", 0.0f, 2.0f);
        }
    }

    // Music
    public void PlayBgm(string id, float fadeOutDuration = 0.2f, float fadeInDuration = 0.2f)
    {
        MusicAsset musicToPlay;
        if (AllMusic.TryGetValue(id, out musicToPlay))
        {
            PlayBgm_Internal(musicToPlay, fadeOutDuration, fadeInDuration);
        }
    }

    void PlayBgm_Internal(MusicAsset music, float fadeOutDuration = 0.2f, float fadeInDuration = 0.2f)
    {
        if (music.Track == null) { return; }

        MusicSource.loop = music.Looping;

        // Stop/Fade out current track
        StartCoroutine(CrossFadeMusic(music, fadeOutDuration, fadeInDuration));
    }

    IEnumerator CrossFadeMusic(MusicAsset nextSong, float fadeOutDuration = 0.2f, float fadeInDuration = 0.2f)
    {
        // Fade out current song
        float t = 0;
        if (MusicSource.isPlaying)
        {
            // Skip if the same song is playing
            if (CurrentPlayingMusic == nextSong.ID) { yield break; }

            float startVolume = MusicSource.volume;
            while (t < fadeOutDuration)
            {
                yield return null;
                t += Time.deltaTime;
                MusicSource.volume = Mathf.Lerp(startVolume, 0.0f, t / fadeOutDuration);
            }
        }
      

        // Set next song
        MusicSource.clip = nextSong.Track;
        CurrentPlayingMusic = nextSong.ID;
        if (nextSong.Looping)
        {
            MusicSource.loop = true;
            if (nextSong.UseLoopPoints)
            {
                MusicSource.PlayScheduled(AudioSettings.dspTime);
            }
        }

        if (!MusicSource.isPlaying)
        {
            MusicSource.Play();
        }

        // Fade in next song
        t = 0;
        float endVolume = GetMusicVolume();
        while (t < fadeInDuration)
        {
            yield return null;
            t += Time.deltaTime;
            MusicSource.volume = Mathf.Lerp(0.0f, endVolume, t / fadeInDuration);
        }
    }

    public void StopBgm()
    {
        MusicSource.Stop();
    }

    public void PauseBgm()
    {
        MusicSource.Pause();
    }

    // Sounds
    public void PlaySound(string soundName, float delay = 0.0f)
    {
        Sound soundToPlay;
        if (AllSounds.TryGetValue(soundName, out soundToPlay))
        {
            if (delay > 0.0f)
            {
                soundToPlay.source.PlayDelayed(0.0f);
            }
            else
            {
                soundToPlay.source.PlayOneShot(soundToPlay.clip, GetSFXVolume());
            }
        }
    }

    // Settings
    public float GetMusicVolume()
    {
        return MasterVolume * MusicVolume;
    }

    public float GetSFXVolume()
    {
        return MasterVolume * SoundVolume;
    }

    #endregion
}
