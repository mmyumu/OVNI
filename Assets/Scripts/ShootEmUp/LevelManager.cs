using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Boundaries boundaries;

    // Start is called before the first frame update
    void Start()
    {
        boundaries = GetComponent<Boundaries>();
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnWave()
    {
        Vector3 spawnPos = new Vector3(0, boundaries.maxY + 2);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0)) * enemyPrefab.transform.rotation);
    }
}
