using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        AudioManager.instance.PlayMusicCrossfade(AudioManager.instance.sounds.GetMusic("Amusement Park"));
        PlayerPrefs.SetInt(Consts.PLAY_ONE_SHOT, 0);
        SceneLoader.instance.LoadScene(Consts.GAME);
    }
}
