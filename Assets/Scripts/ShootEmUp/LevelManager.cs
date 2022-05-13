using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    private Boundaries boundaries;
    private bool isSpawning = false;
    private List<GameObject> currentEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        boundaries = GetComponent<Boundaries>();
        
    }

    // Update is called once per frame
    void Update()
    {
        currentEnemies.RemoveAll(item => item == null);
        if (currentEnemies.Count == 0) {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        
        if (!isSpawning) {
            isSpawning = true;
            SpawnEnemy(-1, boundaries.maxY + 2);
            yield return new WaitForSeconds(1f);
            SpawnEnemy(0, boundaries.maxY + 2);
            yield return new WaitForSeconds(1f);
            SpawnEnemy(1, boundaries.maxY + 2);
            isSpawning = false;
        }
    }

    private void SpawnEnemy(float x, float y) {
        Vector3 spawnPos = new Vector3(x, y);
        currentEnemies.Add(Instantiate(enemyPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0)) * enemyPrefab.transform.rotation));
    }
}
