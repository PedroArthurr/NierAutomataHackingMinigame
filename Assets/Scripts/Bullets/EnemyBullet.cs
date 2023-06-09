using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    [SerializeField] private bool destructable;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactionLayers.HasLayer(other.gameObject.layer))
            StartCoroutine(DelayedDestroy());
    }
}
