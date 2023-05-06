using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private ColorToPrefab[] prefabs;

    private Vector3 middle;
    private Texture2D currentMap;

    private void Start()
    {
        Generate(LevelManager.instance.GetLevel());
    }

    public void Generate(Texture2D map)
    {
        currentMap = map;
        middle = new Vector3(map.width / 2, 0, map.height / 2);
        for (int x = 0; x < map.width; x++)
            for (int z = 0; z < map.height; z++)
                GenerateTile(x, z);
    }

    private void GenerateTile(int x, int z)
    {
        Color pixelColor = currentMap.GetPixel(x, z);
        if (pixelColor.a <= 0)
            return;
        foreach (var p in prefabs)
        {
            //Debug.Log(p.type + " >>> " + ColorUtility.ToHtmlStringRGB(pixelColor) + " >>> " + ColorUtility.ToHtmlStringRGB(p.color));
            if (ColorUtility.ToHtmlStringRGB(p.color) == ColorUtility.ToHtmlStringRGB(pixelColor))
            {
                var position = new Vector3((int)x - middle.x, 0, (int)z - middle.z);
                if (p.prefab != null)
                {
                    var i = Instantiate(p.prefab, position, Quaternion.identity);
                    if (p.type == Enums.LevelPrefabType.Enemy)
                        GameManager.instance.enemiesController.AddEnemy(i.GetComponent<Enemy>());
                    else if (p.type == Enums.LevelPrefabType.ShieldEnemy)
                        GameManager.instance.enemiesController.AddShieldEnemy(i.GetComponent<Enemy>());
                }

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
    public Enums.LevelPrefabType type;
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
            if (ColorUtility.TryParseHtmlString(value, out Color newColor))
            {
                color = newColor;
                hex = value;
            }
        }
    }
}