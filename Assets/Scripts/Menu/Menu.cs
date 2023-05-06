using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt(Consts.PLAY_ONE_SHOT, 1);
        SceneLoader.instance.LoadScene(Consts.GAME);
    }
}
