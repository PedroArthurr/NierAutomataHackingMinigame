using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject gameOverPanel;

    private void Awake() => instance = this;

    public void SetGameOver()
    {
        GameManager.instance.OnGameOver();
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        gameOverPanel.SetActive(true);

        AudioManager.instance.PlaySound(AudioManager.instance.sounds.GetAudioClip("game_over"));
        yield return new WaitForSeconds(2f);
    }
}
