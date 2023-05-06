using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Awake() => instance = this;

    [SerializeField] private GameLevels levelList;

    public string currentLevelName;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(Consts.CURRENT_LEVEL))
            currentLevelName = "LEVEL_1";
        else
            currentLevelName = PlayerPrefs.GetString(Consts.CURRENT_LEVEL);

        print(currentLevelName);
    }

    public Texture2D GetLevel()
    {
        return levelList.GetLevelImage(currentLevelName);
    }

    public IEnumerator NextLevel()
    {
        currentLevelName = levelList.GetNextLevel(currentLevelName);
        PlayerPrefs.SetString(Consts.CURRENT_LEVEL, currentLevelName);
        PlayerPrefs.Save();
        Debug.Log("Waiting for reload the scene");
        yield return new WaitForSeconds(2.5f);
        Debug.Log("Reloading the scene with the new level - " +  currentLevelName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}