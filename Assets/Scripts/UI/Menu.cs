using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private SelectableMenuItem challengeButtonPrefab;
    [SerializeField] private Transform challengeButtonsParent;

    private void Start()
    {
        foreach(var level in LevelManager.instance.LevelsList)
        {
            var button = Instantiate(challengeButtonPrefab, challengeButtonsParent);
            button.SetButton(LevelManager.instance.LevelNames[LevelManager.instance.LevelsList.IndexOf(level)], ()=> StartGame(level));
        }
    }

    public void StartGame(LevelData level)
    {
        LevelManager.instance.CurrentLevel = level;
        AudioManager.instance.PlayMusicCrossfade(AudioManager.instance.sounds.GetMusic("Amusement Park"));
        PlayerPrefs.SetInt(Consts.PLAY_ONE_SHOT, 0);
        SceneLoader.instance.LoadScene(Consts.GAME);
    }
}
