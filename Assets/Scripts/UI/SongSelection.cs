using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text songName;
    private List<BGM> songs = new();
    private GameSounds sounds;
    private string lastTrack;
    private int trackIndex;

    private void Start()
    {
        sounds = AudioManager.instance.sounds;
        songs = AudioManager.instance.sounds.bgm;
        lastTrack = songs[0].musicName;
        print(lastTrack);
        trackIndex = 0;
        //Next();
    }

    public void Next()
    {
        trackIndex++;
        if(trackIndex < songs.Count)
        {
            lastTrack = songs[trackIndex].musicName;
            AudioManager.instance.PlayMusic(sounds.GetMusic(lastTrack));
            SetMenuName(lastTrack);
        }
        else if(trackIndex > songs.Count) 
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
        {
            lastTrack = songs[trackIndex].musicName;
        }

        AudioManager.instance.PlayMusic(sounds.GetMusic(lastTrack));
        SetMenuName(lastTrack);

        AudioManager.instance.currentMusicIndex = trackIndex;
    }

    public void SetMenuName(string trackName)
    {
        print(trackName);
        songName.text = trackName;
    }
}
