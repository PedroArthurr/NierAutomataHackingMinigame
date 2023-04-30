using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public GameSounds sounds;
    public AudioMixerGroup sfxGroup;

    void Awake() => instance = this;

    public void PlaySound(AudioClip clip, float volume = .9f)
    {
        GameObject soundObj = new GameObject(clip.name);
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(soundObj, clip.length);
    }

}
