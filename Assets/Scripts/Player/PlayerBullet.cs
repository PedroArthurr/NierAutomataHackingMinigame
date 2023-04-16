using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [SerializeField] private GameObject collisionParticle;

    public void OnTriggerEnter(Collider other)
    {
        if (ignoreLayers.HasLayer(other.gameObject.layer))
        {
            var p = Instantiate(collisionParticle);
            p.transform.position = this.transform.position;
            Destroy(p.gameObject, .5f);
            StartCoroutine(DelayedDestroy());
        }
    }
}
