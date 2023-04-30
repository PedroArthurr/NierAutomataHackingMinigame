using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObjects/Game Sounds", order = 1)]
public class GameSounds : ScriptableObject
{
    public List<GameSound> sounds = new List<GameSound>();

    public AudioClip GetAudioClip(string audioName)
    {
        foreach (GameSound sound in sounds)
            if (sound.audioName == audioName)
                return sound.clip;

        Debug.LogError("Audio clip '" + audioName + "' not found.");
        return null;
    }

}

[System.Serializable]
public struct GameSound
{
    public string audioName;
    public SoundType type;
    public AudioClip clip;
}
public enum SoundType
{
    SFX,
    BGM,
    Menu
}
