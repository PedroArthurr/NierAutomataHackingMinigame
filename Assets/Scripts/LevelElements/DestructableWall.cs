using System.Collections;
using UnityEngine;

public class DestructableWall : Enemy
{
    private Material wallMaterial;
    private bool flashing;

    public void OnTakeDamage()
    {
        StartCoroutine(FlashColor());
    }

    private IEnumerator FlashColor()
    {
        if (wallMaterial == null)
            wallMaterial = GetComponent<MeshRenderer>().material;

        if(flashing)
            yield break;

        flashing = true;
        Color currentEmissionColor = wallMaterial.GetColor("_EmissionColor");
        Color newEmissionColor = Color.white;
        float elapsedTime = 0f;
        float duration = 0.1f;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            wallMaterial.SetColor("_EmissionColor", Color.Lerp(currentEmissionColor, newEmissionColor * Mathf.Pow(2, 2), t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        duration = 0.05f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            wallMaterial.SetColor("_EmissionColor", Color.Lerp(newEmissionColor * Mathf.Pow(2, 2), currentEmissionColor, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wallMaterial.SetColor("_EmissionColor", currentEmissionColor);
        flashing = false;
    }

}
