using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObjects/Game Sounds", order = 1)]
public class GameSounds : ScriptableObject
{
    public List<SFX> sfx = new List<SFX>();
    public List<BGM> bgm = new List<BGM>();

    public Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    public Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();

    public void AddSound(string audioName, AudioClip clip)
    {
        if (!sfxDictionary.ContainsKey(audioName))
            sfxDictionary.Add(audioName, clip);
    }

    public AudioClip GetAudioClip(string audioName)
    {
        if (sfxDictionary.ContainsKey(audioName))
            return sfxDictionary[audioName];
        else
            Debug.LogError("Audio clip '" + audioName + "' not found.");
        return null;
    }

    public void AddMusic(string music, AudioClip clip)
    {
        if (!bgmDictionary.ContainsKey(music))
            bgmDictionary.Add(music, clip);
    }

    public AudioClip GetMusic(string musicName)
    {
        if(bgmDictionary.ContainsKey(musicName))
            return
                bgmDictionary[musicName];
        Debug.LogError("Music '" + musicName + "' not found.");
        return null;
    }

    [ContextMenu("Set Names")]
    public void SetNames()
    {
        foreach(var s in sfx)
            s.SetName();
    }
}

[System.Serializable]
public class SFX
{
    public string audioName;
    public AudioClip clip;

    public void SetName()
    {
        audioName = clip.name;
    }
}

[System.Serializable]
public class BGM
{
    public string musicName;
    public AudioClip music;

    public void SetName()
    {
        musicName = music.name;
    }
}