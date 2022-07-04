using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public int max = 100;
    public int current;

    private ShootEmUpManager shootEmUpManager;
    private Loots loots;

    // Start is called before the first frame update
    void Start() {
        shootEmUpManager = GameObject.Find("ShootEmUpManager").GetComponent<ShootEmUpManager>();
        current = max;

        loots = gameObject.GetComponent<Loots>();
    }

    // Update is called once per frame
    void Update() {

    }

    public int damage(int damage) {
        if (shootEmUpManager.IsPlaying()) {
            current -= damage;

            if (current <= 0) {

                Destroy(gameObject);
            }
        }

        return current;
    }

    private void OnDestroy() {
        if (loots) {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + loots.money);
        }
        Destroy(gameObject);
    }
}
