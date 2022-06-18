using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WavesManager : MonoBehaviour {
    public GameObject enemyPrefab;
    //public int numberOfEnemiesInSubwave = 3;

    private Boundaries boundaries;
    private ShootEmUpManager shootEmUpManager;
    private List<GameObject> currentEnemies = new List<GameObject>();

    private EnemyDifficulties enemyDifficulties = new EnemyDifficulties();

    // Start is called before the first frame update
    void Start() {
        boundaries = GetComponent<Boundaries>();
        shootEmUpManager = GameObject.Find("ShootEmUpManager").GetComponent<ShootEmUpManager>();
    }

    // Update is called once per frame
    void Update() {
        if (shootEmUpManager.IsPlaying()) {
            currentEnemies.RemoveAll(item => item == null);
        }
    }

    public int GetCurrentEnemiesCount() {
        return currentEnemies.Count;
    }

    public IEnumerator SpawnWave(int level, float size) {
        Debug.Log($"Spawn wave of size {size} and level {level}");
        int numberOfEnemyTypes = (int)UnityEngine.Random.Range(2, 4);
        int numberOfEnemiesInSubwave = (int)size / numberOfEnemyTypes;
        int remainder = (int) size;
        
        for (int i = 0; i < numberOfEnemyTypes; i++) {
            remainder -= numberOfEnemiesInSubwave;

            float y = boundaries.maxY + 1 + i;
            EnemyDefinition enemyDefinition = ComputeEnemyWithLevel(level);

            if (i == numberOfEnemyTypes - 1) {
                numberOfEnemiesInSubwave += remainder;
            }

            yield return SpawnSubwave(i, enemyDefinition, numberOfEnemiesInSubwave);
        }
    }

    private IEnumerator SpawnSubwave(int subwave, EnemyDefinition enemyDefinition, int size) {
        Debug.Log($"Spawn subwave of size {size}: {enemyDefinition}");

        float y = boundaries.maxY + subwave;
        float subwaveTime = Time.time;

        for (int i = 0; i < size; i++) {
            //yield return SpawnEnemy(enemyDefinition, centerX + i, y, i);
            yield return SpawnEnemy(enemyDefinition, subwave, subwaveTime, i, size);
        }
    }

    private IEnumerator SpawnEnemy(EnemyDefinition enemyDefinition, int subwave, float subwaveTime,  int enemyNumber, int numberOfEnemiesInSubWave) {
        //Vector3 spawnPos = new Vector3(x, y);

        Vector3 spawnPos = ComputeSpawnPos(enemyDefinition.GetEnemyMovement(), subwave, enemyNumber, numberOfEnemiesInSubWave);
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0)) * enemyPrefab.transform.rotation);
        currentEnemies.Add(enemy);

        AddComponents(enemy, enemyDefinition, spawnPos, subwaveTime, enemyNumber);

        yield return new WaitForSeconds(1f);
    }

    private Vector3 ComputeSpawnPos(EnemyMovement enemyMovement, int subwave, int enemyNumber, int numberOfEnemiesInSubWave) {
        //float centerX = boundaries.GetCenterX();
        //float x = centerX + enemyNumber;
        //float y = boundaries.maxY + 1 + subwave;

        return enemyMovement.GetSpawnPos(boundaries, subwave, enemyNumber, numberOfEnemiesInSubWave);
        //switch (enemyMovement) {
        //    case EnemyMovement.MoveToFromTop:
        //        float offset = enemyNumber - ((numberOfEnemiesInSubWave - 1) / 2);
        //        x = centerX + offset;
        //        return new Vector3(x, y);
        //    //case EnemyMovement.HorizontalSinusoide:
        //    //    break;
        //    case EnemyMovement.HorizontalWave:
        //        break;
        //}

        //return spawnPos;
    }
    private void AddComponents(GameObject enemy, EnemyDefinition enemyDefinition, Vector3 spawnPos, float subwaveTime, int enemyNumber) {
        AddMovementComponent(enemy, enemyDefinition.GetEnemyMovement(), spawnPos, subwaveTime, enemyNumber);
        //AddAimComponent(enemy, enemyDefinition.GetEnemyAim());
        //AddShootComponent(enemy, enemyDefinition.GetEnemyShoot());
        Health health = enemy.GetComponent<Health>();
        health.max = health.current = enemyDefinition.GetHealth();
    }

    private void AddMovementComponent(GameObject enemy, EnemyMovement enemyMovement, Vector3 spawnPos, float subwaveTime, int enemyNumber) {
        enemyMovement.AddComponent(enemy, spawnPos, boundaries, subwaveTime, enemyNumber);
        //switch (enemyMovement) {
        //    case EnemyMovement.MoveToFromTop:
        //        MoveToAI moveToAI = enemy.AddComponent<MoveToAI>();
        //        moveToAI.destPos = new Vector3();
        //        break;
        //    case EnemyMovement.HorizontalWave:
        //        SplineWalker splineWalker = enemy.AddComponent<SplineWalker>();
        //        splineWalker.spline = GameObject.Find("HorizontalWave").GetComponent<BezierSpline>();
        //        splineWalker.moveSpeed = 0.2f;
        //        // splineWalker.mode = SplineWalker.SplineWalkerMode.PingPong;
        //        break;
        //}
    }

    private void AddAimComponent(GameObject enemy, EnemyAim enemyAim) {
        throw new NotImplementedException();
    }

    private void AddShootComponent(GameObject enemy, EnemyShoot enemyShoot) {
        throw new NotImplementedException();
    }

    private EnemyDefinition ComputeEnemyWithLevel(int level) {
        EnemyMovement enemyMovement = enemyDifficulties.GetRandomEnemyMovement();
        EnemyAim enemyAim = enemyDifficulties.GetRandomEnemyAim();
        EnemyShoot enemyShoot = enemyDifficulties.GetRandomEnemyShoot();

        int health = (int)level / (enemyMovement.Difficulty * enemyAim.Difficulty * enemyShoot.Difficulty);

        return new EnemyDefinition(enemyMovement, enemyAim, enemyShoot, health); 
    }
}