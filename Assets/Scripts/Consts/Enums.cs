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

    public enum ChallengeButtonState
    {
        Selected = 0,
        Unselected = 1,
    }

    public enum CustomSliderType
    {
        BGM = 0,
        SFX = 1,
        Master = 2
    }
}