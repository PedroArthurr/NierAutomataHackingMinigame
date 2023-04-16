//using UnityEngine.C;
using UnityEngine;

[ExecuteAlways]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private ColorToPrefab[] prefabs;
    [SerializeField] private Texture2D map;


    public void GenerateLevel()
    {
        for (int x = 0; x < map.width; x++)
            for (int z = 0; z < map.height; z++)
                GenerateTile(x, z);
    }

    private void GenerateTile(int x, int z)
    {
        Color pixelColor = map.GetPixel(x, z);
        if (pixelColor.a <= 0)
            return;
        foreach (var p in prefabs)
        {
            //Debug.Log(">>>" + ColorUtility.ToHtmlStringRGB(p.color));
            if(ColorUtility.ToHtmlStringRGB(p.color) == ColorUtility.ToHtmlStringRGB(pixelColor))
            {
                var position = new Vector3((int)x, 0, (int)z);
                if (p.prefab != null)
                    Instantiate(p.prefab, position, Quaternion.identity);
            }
        }
    }
    [ContextMenu("Atualizar Cores")]
    private void UpdateColors()
    {
        foreach (var p in prefabs)
            Debug.Log(p.Hex);
    }
}

[System.Serializable]
public class ColorToPrefab
{
    public GameObject prefab;
    public Color color;
    [SerializeField] private string hex;
    public string Hex
    {
        get
        {
            hex = ColorUtility.ToHtmlStringRGB(color);
            return hex;
        }
        set
        {
            Color newColor;
            if (UnityEngine.ColorUtility.TryParseHtmlString(value, out newColor))
            {
                color = newColor;
                hex = value;
            }
        }
    }
}