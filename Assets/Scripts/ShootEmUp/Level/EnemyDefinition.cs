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
    public List<EnemyMovement> enemyMovements = new List<EnemyMovement> { new MoveToFromTop(), new HorizontalSinusoide(), new HorizontalSinusoideSquad() };
    public List<EnemyAim> enemyAim = new List<EnemyAim> { EnemyAim.Straight };
    public List<EnemyShoot> enemyShoot = new List<EnemyShoot> { EnemyShoot.BulletLine };

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
    public EnemyDifficulty() {
    }

    public int Difficulty { get; protected set; }
}

public abstract class EnemyMovement : EnemyDifficulty {
    public abstract void AddComponent(GameObject enemy, Vector3 spawnPos, Boundaries boundaries, float subwaveTime, int enemyNumber);
    public abstract Vector3 GetSpawnPos(Boundaries boundaries, int subwave, int enemyNumber, int numberOfEnemiesInSubWave);
}

public class EnemyAim : EnemyDifficulty {
    public static EnemyAim Straight = new EnemyAim { Difficulty = 2 };
}

public class EnemyShoot : EnemyDifficulty {
    public static EnemyShoot BulletLine = new EnemyShoot { Difficulty = 2 };
}

/***
 * Concrete classes for enemy definitions 
 */
public class MoveToFromTop : EnemyMovement {
    public MoveToFromTop() {
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
    public HorizontalSinusoide() {
        Difficulty = 10;
    }

    public override void AddComponent(GameObject enemy, Vector3 spawnPos, Boundaries boundaries, float subwaveTime, int enemyNumber) {
        HorizontalWaveAI horizontalSinusoide = enemy.AddComponent<HorizontalWaveAI>();
        horizontalSinusoide.destPos = new Vector3(boundaries.maxX / 2 + 0.5f, spawnPos.y);
        horizontalSinusoide.moveSpeed = 4.0f;
        horizontalSinusoide.frequency = 4.0f;
        horizontalSinusoide.xOffset = enemyNumber;

        //SplineWalker splineWalker = enemy.AddComponent<SplineWalker>();
        //splineWalker.spline = GameObject.Find("HorizontalWave").GetComponent<BezierSpline>();
        //splineWalker.moveSpeed = 0.2f;
        //splineWalker.mode = SplineWalker.SplineWalkerMode.PingPong;
    }

    public override Vector3 GetSpawnPos(Boundaries boundaries, int subwave, int enemyNumber, int numberOfEnemiesInSubWave) {
        return new Vector3(boundaries.minX / 2 - 0.5f, boundaries.maxY - subwave - 1);
    }
}

public class HorizontalSinusoideSquad : HorizontalSinusoide {
    public HorizontalSinusoideSquad() {
        Difficulty = 8;
    }

    public override void AddComponent(GameObject enemy, Vector3 spawnPos, Boundaries boundaries, float subwaveTime, int enemyNumber) {
        base.AddComponent(enemy, spawnPos, boundaries, subwaveTime, enemyNumber);
        
        HorizontalWaveAI horizontalSinusoide = enemy.AddComponent<HorizontalWaveAI>();
        
        // Giving same initial time for all the enemies of the subwave will force them to spawn together as a squad
        horizontalSinusoide.initialTime = subwaveTime;
    }
}