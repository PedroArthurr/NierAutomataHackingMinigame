using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int gridSize;
    public List<LevelPrefab> objects;
}

[Serializable]
public class LevelPrefab
{
    public Vector2Int position;
    [ShowOnly]
    public string prefabGUID;
}