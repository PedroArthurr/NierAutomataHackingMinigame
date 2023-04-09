using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public void OnTriggerEnter(Collider other)
    {
        if (ignoreLayers.HasLayer(other.gameObject.layer))
            StartCoroutine(DelayedDestroy());
    }
}
