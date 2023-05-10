using System;
using UnityEngine;

[Serializable]
public class LevelObject : MonoBehaviour
{
    [ShowOnly]
    public string objectGUID;

    [HideInInspector] public Vector2Int position;
    [ContextMenu("Set GUID")]
    public void SetGUID()
    {
        objectGUID = System.Guid.NewGuid().ToString();
    }
}
