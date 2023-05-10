using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelObject : MonoBehaviour
{
    [ShowOnly]
    public string objectGUID;

    public Vector2Int position;
    [ContextMenu("Set GUID")]
    public void SetGUID()
    {
        objectGUID = System.Guid.NewGuid().ToString();
    }
}
