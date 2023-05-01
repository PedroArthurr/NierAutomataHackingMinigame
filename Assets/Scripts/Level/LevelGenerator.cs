using UnityEngine;

[ExecuteAlways]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private ColorToPrefab[] prefabs;
    [SerializeField] private Texture2D map;
    private Vector3 middle;

    public void GenerateLevel()
    {
        middle = new Vector3(map.width / 2, 0, map.height / 2);
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
            //Debug.Log(p.type + " >>> " + ColorUtility.ToHtmlStringRGB(pixelColor) + " >>> " + ColorUtility.ToHtmlStringRGB(p.color));
            if (ColorUtility.ToHtmlStringRGB(p.color) == ColorUtility.ToHtmlStringRGB(pixelColor))
            {
                var position = new Vector3((int)x - middle.x, 0, (int)z - middle.z);
                if (p.prefab != null)
                {
                    var i = Instantiate(p.prefab, position, Quaternion.identity);
                    if (p.type == LevelPrefabType.Enemy)
                        GameManager.Instance.enemiesController.AddEnemy(i.GetComponent<Enemy>());
                    else if (p.type == LevelPrefabType.ShieldEnemy)
                        GameManager.Instance.enemiesController.AddShieldEnemy(i.GetComponent<Enemy>());
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
    public LevelPrefabType type;
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

public enum LevelPrefabType
{
    Level = 0,
    Player = 1,
    Enemy = 2,
    ShieldEnemy = 3,
}