using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public GameSounds sounds;
    public AudioMixerGroup master;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup bgmGroup;
    public int currentMusicIndex = 0;

    [Space]
    [HideInInspector] public AudioMixerController controller;

    private AudioSource currentMusic;
    private float musicFadeDuration = 1.5f;

    public delegate void MenuNameEventHandler();
    public static event MenuNameEventHandler OnMusicEnd;
    void Awake() => instance = this;

    private void Start()
    {
        foreach (var s in sounds.sfx)
            sounds.AddSound(s.audioName, s.clip);

        foreach (var m in sounds.bgm)
            sounds.AddMusic(m.musicName, m.music);

        //PlayMusic(sounds.GetMusic("a"));
        PlayMusic(sounds.GetMusic("Significance"));
    }

    private void Update()
    {
        if (currentMusic != null)
        {
            if (!currentMusic.isPlaying)
            {
                if (Mathf.Approximately(currentMusic.time, currentMusic.clip.length))
                    PlayNextMusic();
            }
        }
    }

    public void PlaySound(AudioClip clip, float volume = .9f)
    {
        GameObject sfxObj = new GameObject(clip.name);
        AudioSource audioSource = sfxObj.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(sfxObj, clip.length);
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
            Destroy(currentMusic);
        }

        currentMusic = gameObject.AddComponent<AudioSource>();
        currentMusic.outputAudioMixerGroup = bgmGroup;
        currentMusic.clip = clip;
        currentMusic.volume = volume;
        currentMusic.loop = false;
        currentMusic.Play();
    }

    public void PlayMusicCrossfade(AudioClip clip, float volume = 1f)
    {
        if (currentMusic == null)
        {
            currentMusic = gameObject.AddComponent<AudioSource>();
            currentMusic.outputAudioMixerGroup = bgmGroup;
            currentMusic.clip = clip;
            currentMusic.volume = volume;
            currentMusic.loop = false;
            currentMusic.Play();
        }
        else if (currentMusic.clip != clip)
        {
            AudioSource newMusic = gameObject.AddComponent<AudioSource>();
            newMusic.outputAudioMixerGroup = bgmGroup;
            newMusic.clip = clip;
            newMusic.volume = 0f;
            newMusic.loop = false;
            newMusic.Play();

            StartCoroutine(CrossfadeMusic(currentMusic, newMusic, volume));
        }
        else
        {
            Debug.LogWarning("Trying to play the same music that is already playing.");
            PlayNextMusic();
        }
    }

    private IEnumerator CrossfadeMusic(AudioSource current, AudioSource newMusic, float targetVolume)
    {
        float startTime = Time.time;
        float startVolume = current.volume;
        float targetTime = startTime + musicFadeDuration;

        while (Time.time < targetTime)
        {
            float t = (Time.time - startTime) / musicFadeDuration;
            current.volume = Mathf.Lerp(startVolume, 0f, t);
            newMusic.volume = Mathf.Lerp(0f, targetVolume, t);
            yield return null;
        }

        current.Stop();
        Destroy(current);
        currentMusic = newMusic;
    }

    public void PlayNextMusic(float volume = 1f)
    {
       
        currentMusicIndex++;
        print(currentMusicIndex);
        if (currentMusicIndex > sounds.bgm.Count)
        {
            currentMusicIndex = 1;
        }

        AudioClip nextClip = sounds.bgm[currentMusicIndex - 1].music;

        var songSelectionMenu = FindObjectOfType<SongSelection>();
        if (songSelectionMenu != null)
        {
            songSelectionMenu.SetMenuName(sounds.bgm[currentMusicIndex - 1].musicName);
        }

        PlayMusicCrossfade(nextClip, volume);
    }

}
