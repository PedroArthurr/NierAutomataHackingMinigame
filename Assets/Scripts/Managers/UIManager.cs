using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject sceneTransitionPanel;

    private void Awake() => instance = this;

    public void SetGameOver(bool win)
    {
        if (win)
            GameManager.instance.OnFinishLevel(win);

        StartCoroutine(GameOver(win));
    }

    private IEnumerator GameOver(bool win)
    {
        if (win)
        {
            yield return new WaitForSeconds(1f);
            gameOverPanel.SetActive(true);
            AudioManager.instance.PlaySound(AudioManager.instance.sounds.GetAudioClip("game_over"));
        }

        yield return new WaitForSeconds(win ? 2 : 1);
        sceneTransitionPanel.SetActive(true);
    }
}
