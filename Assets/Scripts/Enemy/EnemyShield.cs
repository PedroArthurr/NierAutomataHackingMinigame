using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private LayerMask playerBulletLayer;
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private GameObject destroyParticle;
    private AudioClip shieldHit;
    private Enemy enemy;

    private void Start()
    {
        shieldHit = AudioManager.instance.sounds.GetAudioClip("core_hit_shield");
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (playerBulletLayer.HasLayer(collision.gameObject.layer))
        {
            var p = Instantiate(hitParticle, this.transform);
            p.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
            Destroy(p, .5f);
            AudioManager.instance.PlaySound(shieldHit, .3f);
            Destroy(collision.gameObject);
        }
    }

    [ContextMenu("Destroy Shield")]
    public void Remove()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.sounds.GetAudioClip("core_broken"), .3f);
        var p = Instantiate(destroyParticle, this.transform);
        p.transform.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        Destroy(p, .5f);
        Destroy(this.gameObject);
    }
}