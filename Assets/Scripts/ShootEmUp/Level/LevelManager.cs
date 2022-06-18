using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    //public GameObject basicEnemyPrefab;
    //public GameObject waveEnemyPrefab;
    // public GameObject horizontalWavePrefab;
    // public Dictionary<string, GameObject> enemiesPrefabs;
    public int waves = 3;

    private ShootEmUpManager shootEmUpManager;
    private WavesManager wavesManager;

    private int wave = 0;
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start() {
        shootEmUpManager = GameObject.Find("ShootEmUpManager").GetComponent<ShootEmUpManager>();
        wavesManager = GameObject.Find("WavesManager").GetComponent<WavesManager>();

    }

    // Update is called once per frame
    void Update() {
        if (shootEmUpManager.IsPlaying()) {
            if (wavesManager.GetCurrentEnemiesCount() == 0) {
                bool ended = CheckLevelEnded();

                if (!ended) {
                    StartCoroutine(SpawnWave());
                }
            }
        }
    }

    private bool CheckLevelEnded() {
        if (wave >= waves) {
            Debug.Log($"Level ended");
            shootEmUpManager.MissionComplete();
            return true;
        }
        return false;
    }

    private IEnumerator SpawnWave(Level level = Level.Low, int size = 6) {
        if (!isSpawning) {
            isSpawning = true;

            float modifier = ComputeModifier();
            float modifiedSize = ((int)size) * modifier;

            int intLevel = (int)level;

            Debug.Log($"Spawning wave {wave} of size {modifiedSize} and level {intLevel}");

            yield return StartCoroutine(wavesManager.SpawnWave(intLevel, modifiedSize));
            wave++;

            isSpawning = false;
            Debug.Log($"Wave {wave} spawned");
        }
    }

    private float ComputeModifier() {
        return 1 + ((float)wave / (float)waves);
    }
}