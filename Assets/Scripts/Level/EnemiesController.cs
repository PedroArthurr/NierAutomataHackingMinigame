using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public List<Enemy> enemies = new();
    public List<Enemy> shieldEnemies = new();

    public void AddEnemy(Enemy e) => enemies.Add(e);

    public void AddShieldEnemy(Enemy e) => shieldEnemies.Add(e);

    public void OnDestroyEnemy(Enemy d)
    {
        if (enemies.Contains(d))
            enemies.Remove(d);
        if (shieldEnemies.Contains(d))
            shieldEnemies.Remove(d);

        if (enemies.Count == 0)
        {
            if (shieldEnemies.Count > 0)
            {
                foreach (var e in shieldEnemies)
                    e.DestroyShield();
            }
            if(shieldEnemies.Count == 0)
                UIManager.instance.SetGameOver();
        }
    }
}
