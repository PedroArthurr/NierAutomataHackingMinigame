using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Awake() => instance = this;

    [SerializeField] private List<string> levelNames = new();
    [SerializeField] private List<LevelData> levelsList = new();

    private LevelData currentLevel;
    private string currentLevelName;

    public List<LevelData> LevelsList { get => levelsList; }
    public List<string> LevelNames { get => levelNames; }
    public LevelData CurrentLevel { get => currentLevel; set => currentLevel = value; }

    private void Start()
    {
        string filePath = Application.persistentDataPath + "/GameLevels/";
        foreach(string n in LevelNames)
        {
            string json = System.IO.File.ReadAllText(filePath + n + ".json");
            LevelsList.Add(JsonUtility.FromJson<LevelData>(json));
        }

        if (!PlayerPrefs.HasKey(Consts.CURRENT_LEVEL))
            currentLevelName = "HACKING GAME #01";
        else
            currentLevelName = PlayerPrefs.GetString(Consts.CURRENT_LEVEL);

        print(currentLevelName);
    }

    public IEnumerator NextLevel()
    {
        PlayerPrefs.SetString(Consts.CURRENT_LEVEL, currentLevelName);
        PlayerPrefs.Save();

        Debug.Log("Waiting for reload the scene");
        yield return new WaitForSeconds(4f);

        if (PlayerPrefs.GetInt(Consts.PLAY_ONE_SHOT) == 1)
        {
            SceneLoader.instance.LoadScene(Consts.MENU);
        }
        else
        {
            int currentLevelIndex = levelNames.IndexOf(currentLevelName);
            if (currentLevelIndex < levelNames.Count - 1)
            {
                currentLevelIndex++;
                currentLevelName = levelNames[currentLevelIndex];
                CurrentLevel = levelsList[currentLevelIndex];
                Debug.Log("Reloading the scene with the new level - " + currentLevelName);
                SceneLoader.instance.ReloadScene();
            }
            else
            {
                SceneLoader.instance.LoadScene(Consts.MENU);
            }
        }
    }
}