using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WavesManager : MonoBehaviour {
    public GameObject enemyPrefab;

    private Boundaries boundaries;
    private ShootEmUpManager shootEmUpManager;
    private List<GameObject> currentEnemies = new List<GameObject>();

    private EnemyDifficulties enemyDifficulties;

    // Start is called before the first frame update
    void Start() {
        boundaries = GetComponent<Boundaries>();
        shootEmUpManager = GameObject.Find("ShootEmUpManager").GetComponent<ShootEmUpManager>();

        enemyDifficulties = new EnemyDifficulties(shootEmUpManager);
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
        Vector3 spawnPos = enemyDefinition.GetEnemyMovement().GetSpawnPos(boundaries, subwave, enemyNumber, numberOfEnemiesInSubWave);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0)) * enemyPrefab.transform.rotation);
        AddComponents(enemy, enemyDefinition, spawnPos, subwaveTime, enemyNumber);
        currentEnemies.Add(enemy);

        yield return new WaitForSeconds(1f);
    }

    private void AddComponents(GameObject enemy, EnemyDefinition enemyDefinition, Vector3 spawnPos, float subwaveTime, int enemyNumber) {
        enemyDefinition.GetEnemyMovement().AddComponent(enemy, spawnPos, boundaries, subwaveTime, enemyNumber);

        GameObject bulletPrefab = enemyDefinition.GetEnemyShoot().GetPrefab();
        enemyDefinition.GetEnemyAim().AddComponent(enemy, bulletPrefab);

        Health health = enemy.GetComponent<Health>();
        health.max = health.current = enemyDefinition.GetHealth();
    }

    //private void AddMovementComponent(GameObject enemy, EnemyMovement enemyMovement, Vector3 spawnPos, float subwaveTime, int enemyNumber) {
    //    enemyMovement.AddComponent(enemy, spawnPos, boundaries, subwaveTime, enemyNumber);
    //}

    //private void AddAimComponent(GameObject enemy, EnemyAim enemyAim) {
    //    throw new NotImplementedException();
    //}

    //private void AddShootComponent(GameObject enemy, EnemyShoot enemyShoot) {
    //    throw new NotImplementedException();
    //}

    private EnemyDefinition ComputeEnemyWithLevel(int level) {
        EnemyMovement enemyMovement = enemyDifficulties.GetRandomEnemyMovement();
        EnemyAim enemyAim = enemyDifficulties.GetRandomEnemyAim();
        EnemyShoot enemyShoot = enemyDifficulties.GetRandomEnemyShoot();

        int health = (int)level / (enemyMovement.Difficulty * enemyAim.Difficulty * enemyShoot.Difficulty);

        return new EnemyDefinition(enemyMovement, enemyAim, enemyShoot, health); 
    }
}