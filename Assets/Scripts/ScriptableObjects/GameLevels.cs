using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Levels", menuName = "ScriptableObjects/Levels")]
public class GameLevels : ScriptableObject
{
    public List<Level> levels = new();

    private Dictionary<string, Texture2D> levelImages = null;

    public Texture2D GetLevelImage(string levelName)
    {
        if (levelImages == null)
        {
            levelImages = new Dictionary<string, Texture2D>();
            foreach (Level level in levels)
                levelImages[level.levelName] = level.levelImage;
        }

        if (levelImages.ContainsKey(levelName))
            return levelImages[levelName];
        else
        {
            Debug.LogError("Não foi encontrada nenhuma imagem para o nível " + levelName);
            return null;
        }
    }

    public string GetNextLevel(string currentLevelName)
    {
        int currentLevelIndex = levels.FindIndex(level => level.levelName == currentLevelName);
        if (currentLevelIndex >= 0)
        {
            int nextLevelIndex = currentLevelIndex + 1;

            if (nextLevelIndex < levels.Count)
                return levels[nextLevelIndex].levelName;
            else
            {
                Debug.Log("No more levels");
                return default;
            }
        }
        else
        {
            Debug.LogError("Level not found");
            return default;
        }
    }
}

[System.Serializable]
public struct Level
{
    public string levelName;
    public Texture2D levelImage;
}