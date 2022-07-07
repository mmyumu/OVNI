using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFight : MonoBehaviour {
    public GameObject fightPrefab;

    // Start is called before the first frame update
    void Start() {
        Bounds mapBoundaries = gameObject.GetComponent<SpriteRenderer>().sprite.bounds;
        PolygonCollider2D[] polygons = gameObject.GetComponentsInChildren<PolygonCollider2D>();

        bool onGround = false;
        while(!onGround) {
            float x = Random.Range(mapBoundaries.min.x, mapBoundaries.max.x);
            float y = Random.Range(mapBoundaries.min.y, mapBoundaries.max.y);

            Debug.Log($"Try x={x}, y={y}");
            foreach (PolygonCollider2D polygon in polygons) {
                if (polygon.OverlapPoint(new Vector2(x, y))) {
                    onGround = true;
                    GameObject fight = Instantiate(fightPrefab, new Vector3(x, y, 0), Quaternion.Euler(new Vector3(0, 0, 0)) * transform.rotation);
                    fight.GetComponent<SpriteRenderer>().sortingOrder = 10;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
