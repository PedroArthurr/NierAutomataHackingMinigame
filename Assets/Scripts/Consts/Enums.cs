using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    #region Enemy
    public enum EnemyType
    {
        Shooter,
        Stalker,
        Wall,
    }

    public enum ShootingBehaviour
    {
        SEMIAUTO,
        MACHINEGUN,
        SHOTGUN,
    }

    public enum BulletTypes
    {
        PURPLE,
        ORANGE,
    }
    #endregion Enemy

    public enum LevelPrefabType
    {
        Level = 0,
        Player = 1,
        Enemy = 2,
        ShieldEnemy = 3,
    }
}