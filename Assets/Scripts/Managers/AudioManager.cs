using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public GameSounds sounds;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup bgmGroup;

    private AudioSource currentMusic;
    private float musicFadeDuration = 1.5f;

    void Awake() => instance = this;

    private void Start()
    {
        foreach (var s in sounds.sfx)
            sounds.AddSound(s.audioName, s.clip);

        foreach (var m in sounds.bgm)
            sounds.AddMusic(m.musicName, m.music);

        PlayMusic(sounds.GetMusic("City Ruins"), 1);

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
        if (currentMusic == null)
        {
            currentMusic = gameObject.AddComponent<AudioSource>();
            currentMusic.outputAudioMixerGroup = bgmGroup;
            currentMusic.clip = clip;
            currentMusic.volume = volume;
            currentMusic.loop = true;
            currentMusic.Play();
        }
        else if (currentMusic.clip != clip)
        {
            AudioSource newMusic = gameObject.AddComponent<AudioSource>();
            newMusic.outputAudioMixerGroup = bgmGroup;
            newMusic.clip = clip;
            newMusic.volume = 0f;
            newMusic.loop = true;
            newMusic.Play();

            StartCoroutine(CrossfadeMusic(currentMusic, newMusic, volume));
        }
    }

    private IEnumerator CrossfadeMusic(AudioSource current, AudioSource next, float targetVolume)
    {
        float startTime = Time.time;
        float startVolume = current.volume;
        float targetTime = startTime + musicFadeDuration;

        while (Time.time < targetTime)
        {
            float t = (Time.time - startTime) / musicFadeDuration;
            current.volume = Mathf.Lerp(startVolume, 0f, t);
            next.volume = Mathf.Lerp(0f, targetVolume, t);
            yield return null;
        }

        current.Stop();
        Destroy(current);
        currentMusic = next;
    }

}
