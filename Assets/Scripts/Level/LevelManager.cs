using System.Collections;
using UnityEngine;

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
        if (currentLevelName == Consts.END_GAME)
            return null;
        return levelList.GetLevelImage(currentLevelName);
    }

    public IEnumerator NextLevel()
    {
        PlayerPrefs.SetString(Consts.CURRENT_LEVEL, currentLevelName);
        PlayerPrefs.Save();
        Debug.Log("Waiting for reload the scene");
        yield return new WaitForSeconds(4f);

        if (PlayerPrefs.GetInt(Consts.PLAY_ONE_SHOT) == 1)
            SceneLoader.instance.LoadScene(Consts.MENU);

        else
        {
            currentLevelName = levelList.GetNextLevel(currentLevelName);

            Debug.Log("Reloading the scene with the new level - " + currentLevelName);
      
            if (currentLevelName == Consts.END_GAME)
                SceneLoader.instance.LoadScene(Consts.MENU);
            else
                SceneLoader.instance.ReloadScene();
        }
    }
}