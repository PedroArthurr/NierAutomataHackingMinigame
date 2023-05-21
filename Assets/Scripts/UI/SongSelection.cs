using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SongSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text songName;
    private List<BGM> songs = new();
    private GameSounds sounds;
    private string lastTrack;
    private int trackIndex = -1;

    private void Start()
    {
        sounds = AudioManager.instance.sounds;
        songs = AudioManager.instance.sounds.bgm;
        if (!IsAudioGroupPlaying(AudioManager.instance.bgmGroup))
            Next();
    }

    private bool IsAudioGroupPlaying(AudioMixerGroup group)
    {
        if (group == null)
            return false;

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.outputAudioMixerGroup == group && audioSource.isPlaying)
            {
                return true;
            }
        }

        return false;
    }

    public void Next()
    {
        trackIndex++;
        if (trackIndex < songs.Count)
        {
            lastTrack = songs[trackIndex].musicName;
            AudioManager.instance.PlayMusic(sounds.GetMusic(lastTrack));
            SetMenuName(lastTrack);
        }
        else if (trackIndex > songs.Count)
        {
            trackIndex = -1;
            Next();
        }

        AudioManager.instance.currentMusicIndex = trackIndex;
    }

    public void Previous()
    {
        trackIndex--;

        if (trackIndex < 0)
        {
            trackIndex = songs.Count - 1;
            lastTrack = songs[trackIndex].musicName;
        }
        else
            lastTrack = songs[trackIndex].musicName;

        AudioManager.instance.PlayMusic(sounds.GetMusic(lastTrack));
        SetMenuName(lastTrack);

        AudioManager.instance.currentMusicIndex = trackIndex;
    }

    public void SetMenuName(string trackName)
    {
        Debug.Log("Playing: " + trackName);
        songName.text = trackName;
    }
}
