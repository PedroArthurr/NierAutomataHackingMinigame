using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Assets", menuName = "ScriptableObjects/Game Assets")]
public class GamePrefabs : ScriptableObject
{
    public List<LevelObject> levelObjects = new();
    public List<LevelObject> enemies = new();
    public List<LevelObject> player = new();
}
