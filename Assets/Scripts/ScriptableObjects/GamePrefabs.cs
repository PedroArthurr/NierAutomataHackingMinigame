using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Assets", menuName = "ScriptableObjects/Game Assets")]
public class GamePrefabs : ScriptableObject
{
    public List<LevelObject> levelObjects = new();
    public List<LevelObject> enemies = new();
    public List<LevelObject> player = new();

    public LevelObject FindObjectByGUID(string guid)
    {
        foreach (LevelObject levelObject in this.levelObjects)
        {
            if (levelObject.objectGUID == guid)
            {
                return levelObject;
            }
        }

        foreach (LevelObject enemy in this.enemies)
        {
            if (enemy.objectGUID == guid)
            {
                return enemy;
            }
        }

        foreach (LevelObject player in this.player)
        {
            if (player.objectGUID == guid)
            {
                return player;
            }
        }

        return null;
    }

}
