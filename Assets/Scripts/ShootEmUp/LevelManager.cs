using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public GameObject basicEnemyPrefab;
    public Dictionary<string, GameObject> enemiesPrefabs;
    public int waves = 3;

    private Boundaries boundaries;
    private bool isSpawning = false;
    private List<GameObject> currentEnemies = new List<GameObject>();
    private ShootEmUpManager shootEmUpManager;
    private bool started = false;

    private int wave = 0;
    

    enum Level {
        Low,
        Medium,
        High
    }

    enum Size {
        Low,
        Medium,
        High
    }

    // Start is called before the first frame update
    void Start() {
        boundaries = GetComponent<Boundaries>();
        shootEmUpManager = GameObject.Find("ShootEmUpManager").GetComponent<ShootEmUpManager>();

    }

    // Update is called once per frame
    void Update() {
        if (shootEmUpManager.IsPlaying()) {
            currentEnemies.RemoveAll(item => item == null);
            
            if (currentEnemies.Count == 0) {
                bool ended = CheckLevelEnded();

                if (!ended) {
                    StartCoroutine(SpawnWave());
                }
            }
        }
    }

    //private void StartLevel() {
    //    if (!started) {

    //    }
        
    //}

    private bool CheckLevelEnded() {
        if (wave >= waves) {
            Debug.Log($"Level ended");
            shootEmUpManager.MissionComplete();
            return true;
        }
        return false;
    }

    private IEnumerator SpawnWave(Level level = Level.Low, Size size = Size.Low) {
        if (!isSpawning) {
            Debug.Log($"Spawning wave {wave}");
            isSpawning = true;
            wave++;
            // yield return new WaitForSeconds(1f);
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
        GameObject enemy = Instantiate(basicEnemyPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0)) * basicEnemyPrefab.transform.rotation);
        currentEnemies.Add(enemy);
    }
}
