using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class EnemyDefinition {
    private EnemyMovement movement;
    private EnemyAim aim;
    private EnemyShoot shoot;
    private int health;

    public EnemyDefinition(EnemyMovement movement, EnemyAim aim, EnemyShoot shoot, int health) {
        this.movement = movement;
        this.aim = aim;
        this.shoot = shoot;
        this.health = health;
    }

    //public GameObject SpawnEnemy(GameObject enemyPrefab, Boundaries boundaries, int subwave, float subwaveTime, int enemyNumber, int numberOfEnemiesInSubWave) {
    //    Vector3 spawnPos = movement.GetSpawnPos(boundaries, subwave, enemyNumber, numberOfEnemiesInSubWave);
    //    return enemy;
    //}

    //public void AddComponents(GameObject enemy, Boundaries boundaries, Vector3 spawnPos, float subwaveTime, int enemyNumber) {

    //}

    public EnemyMovement GetEnemyMovement() {
        return movement;
    }

    public EnemyAim GetEnemyAim() {
        return aim;
    }

    public EnemyShoot GetEnemyShoot() {
        return shoot;
    }
    public int GetHealth() {
        return health;
    }

    private int GetPoints() {
        return movement.Difficulty * aim.Difficulty * shoot.Difficulty * health;
    }

    public override string ToString() {
        return $"{movement}, {aim}, {shoot} [{health} hp] -> {GetPoints()} points";
    }
}

public class EnemyDifficulties {
    public List<EnemyMovement> enemyMovements;
    public List<EnemyAim> enemyAim;
    public List<EnemyShoot> enemyShoot;

    public EnemyDifficulties(ShootEmUpManager shootEmUpManager) {
        enemyMovements = new List<EnemyMovement> { new MoveToFromTop(shootEmUpManager), new HorizontalSinusoide(shootEmUpManager), new HorizontalSinusoideSquad(shootEmUpManager) };
        enemyAim = new List<EnemyAim> { new Straight(shootEmUpManager) };
        enemyShoot = new List<EnemyShoot> { new CircleStandard(shootEmUpManager) };
    }

    public EnemyMovement GetRandomEnemyMovement() {
        var random = new System.Random();
        int index = random.Next(enemyMovements.Count);
        return enemyMovements[index];
    }

    public EnemyAim GetRandomEnemyAim() {
        var random = new System.Random();
        int index = random.Next(enemyAim.Count);
        return enemyAim[index];
    }

    public EnemyShoot GetRandomEnemyShoot() {
        var random = new System.Random();
        int index = random.Next(enemyShoot.Count);
        return enemyShoot[index];
    }    
}
public class EnemyDifficulty {
    protected ShootEmUpManager shootEmUpManager;
    public EnemyDifficulty(ShootEmUpManager shootEmUpManager) {
        this.shootEmUpManager = shootEmUpManager;
    }

    public int Difficulty { get; protected set; }
}

public abstract class EnemyMovement : EnemyDifficulty {
    protected EnemyMovement(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
    }

    public abstract void AddComponent(GameObject enemy, Vector3 spawnPos, Boundaries boundaries, float subwaveTime, int enemyNumber);
    public abstract Vector3 GetSpawnPos(Boundaries boundaries, int subwave, int enemyNumber, int numberOfEnemiesInSubWave);
}

public abstract class EnemyAim : EnemyDifficulty {
    protected EnemyAim(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
    }

    public abstract void AddComponent(GameObject enemy, GameObject bulletPrefab);
}

public abstract class EnemyShoot : EnemyDifficulty {
    protected EnemyShoot(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
    }

    public abstract GameObject GetPrefab();

    protected GameObject GetPrefab(string key) {
        BulletsPrefabs bulletsPrefabs = shootEmUpManager.GetComponent<BulletsPrefabs>();
        return bulletsPrefabs.GetKey(key);
    }
}

/**
 * Concrete classes for enemy definitions 
 */

/*
 * Movements
 */
public class MoveToFromTop : EnemyMovement {
    public MoveToFromTop(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
        Difficulty = 2;
    }

    public override void AddComponent(GameObject enemy, Vector3 spawnPos, Boundaries boundaries, float subwaveTime, int enemyNumber) {
        MoveToAI moveToAI = enemy.AddComponent<MoveToAI>();
        moveToAI.destPos = new Vector3();
    }

    public override Vector3 GetSpawnPos(Boundaries boundaries, int subwave, int enemyNumber, int numberOfEnemiesInSubWave) {
        float offset = enemyNumber - ((numberOfEnemiesInSubWave - 1) / 2);
        float x = boundaries.GetCenterX() + offset;
        float y = boundaries.maxY + 0.5f + subwave;
        return new Vector3(x, y);
    }
}

public class HorizontalSinusoide : EnemyMovement {
    public HorizontalSinusoide(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
        Difficulty = 10;
    }

    public override void AddComponent(GameObject enemy, Vector3 spawnPos, Boundaries boundaries, float subwaveTime, int enemyNumber) {
        HorizontalWaveAI horizontalWave = enemy.AddComponent<HorizontalWaveAI>();
        UpdateComponent(horizontalWave, spawnPos, boundaries, enemyNumber);
    }

    public override Vector3 GetSpawnPos(Boundaries boundaries, int subwave, int enemyNumber, int numberOfEnemiesInSubWave) {
        return new Vector3(boundaries.minX / 2 - 0.5f, boundaries.maxY - subwave - 1);
    }

    protected void UpdateComponent(HorizontalWaveAI horizontalWave, Vector3 spawnPos, Boundaries boundaries, int enemyNumber) {
        horizontalWave.destPos = new Vector3(boundaries.maxX / 2 + 0.5f, spawnPos.y);
        horizontalWave.moveSpeed = 4.0f;
        horizontalWave.frequency = 4.0f;
        horizontalWave.xOffset = enemyNumber;
    }
}

public class HorizontalSinusoideSquad : HorizontalSinusoide {
    public HorizontalSinusoideSquad(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
        Difficulty = 8;
    }

    public override void AddComponent(GameObject enemy, Vector3 spawnPos, Boundaries boundaries, float subwaveTime, int enemyNumber) {
        HorizontalWaveAI horizontalWave = enemy.AddComponent<HorizontalWaveAI>();
        UpdateComponent(horizontalWave, spawnPos, boundaries, enemyNumber);

        // Giving same initial time for all the enemies of the subwave will force them to spawn together as a squad
        horizontalWave.initialTime = subwaveTime;
    }
}

/*
 * Aim
 */
public class Straight : EnemyAim {
    public Straight(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
        Difficulty = 2;
    }
    
    public override void AddComponent(GameObject enemy, GameObject bulletPrefab) {
        StraightShootAI straightShootAI = enemy.AddComponent<StraightShootAI>();
        straightShootAI.bulletPrefab = bulletPrefab;
    }
}

/*
 * Shoot/Bullets
 */
public class CircleStandard: EnemyShoot {
    public CircleStandard(ShootEmUpManager shootEmUpManager) : base(shootEmUpManager) {
        Difficulty = 2;
    }

    public override GameObject GetPrefab() {
        return GetPrefab("CircleBullet");
    }
}