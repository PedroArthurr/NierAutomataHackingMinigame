using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GamePrefabs gamePrefabs;

    private Vector3 middle;

    private void Start()
    {
        int playRandomSong = Random.Range(0, 100);
        if (playRandomSong > 70)
        {
            Debug.Log("Playing new song");
            AudioManager.instance.PlayMusicCrossfade(AudioManager.instance.sounds.GetMusic(AudioManager.instance.GetRandomMusicName()));
        }

        if (Application.isPlaying)
            Generate();
    }

    public void Generate()
    {
        LevelData levelData = LevelManager.instance.CurrentLevel;
        middle = new Vector3(levelData.gridSize / 2, 0, levelData.gridSize / 2);

        foreach (var o in levelData.objects)
        {
            var prefab = gamePrefabs.FindObjectByGUID(o.prefabGUID);

            Vector2Int rotatedPosition = new Vector2Int(o.position.y, -o.position.x);
            Vector3 finalPosition = new Vector3(rotatedPosition.x - middle.x, 0, rotatedPosition.y + middle.z);

            GenerateLevelObject(prefab, finalPosition);
        }
    }

    private void GenerateLevelObject(LevelObject objectData, Vector3 p)
    {
        List<LevelObject> l = new(gamePrefabs.levelObjects.Concat(gamePrefabs.enemies).Concat(gamePrefabs.player));

        LevelObject levelObjectPrefab = l.Find(lo => lo.objectGUID == objectData.objectGUID);

        if (levelObjectPrefab != null)
        {
            var o = Instantiate(levelObjectPrefab, p, Quaternion.identity);
            if (o.GetComponent<Enemy>())
            {
                var e = o.GetComponent<Enemy>();
                if (e.GetComponentInChildren<EnemyShield>())
                    GameManager.instance.enemiesController.AddShieldEnemy(e);
                else
                    GameManager.instance.enemiesController.AddEnemy(e);
            }
            //print(o.name + " -> " + p.x + " -> " + p.z);
        }
        else
        {
            Debug.LogWarning($"No LevelObject found with GUID {objectData.objectGUID}");
        }
    }
}