using System.Collections;
using UnityEngine;

public class DestructableWall : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 8;
    [Space]
    [Header("Flash Settings")]
    [SerializeField] protected Color flashColor;
    [SerializeField] protected float flashTime = 0.05f;
    protected Material enemyMaterial;
    protected bool flashing;

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
            Destroy(this.gameObject);
        StartCoroutine(FlashColor());
    }

    private IEnumerator FlashColor()
    {
        if (enemyMaterial == null)
            enemyMaterial = GetComponent<MeshRenderer>().material;

        if (flashing)
            yield break;

        flashing = true;
        Color currentEmissionColor = enemyMaterial.GetColor("_EmissionColor");
        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < flashTime)
        {
            float t = elapsedTime / duration;
            enemyMaterial.SetColor("_EmissionColor", Color.Lerp(currentEmissionColor, flashColor * Mathf.Pow(2, 2), t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < flashTime / 2)
        {
            float t = elapsedTime / duration;
            enemyMaterial.SetColor("_EmissionColor", Color.Lerp(flashColor * Mathf.Pow(2, 2), currentEmissionColor, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyMaterial.SetColor("_EmissionColor", currentEmissionColor);
        flashing = false;
    }
}
