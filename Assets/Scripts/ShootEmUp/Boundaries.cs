using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour {
    public float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start() {
        UpdateBoundaries();
    }

    // Update is called once per frame
    void Update() {
        UpdateBoundaries();
    }

    void UpdateBoundaries() {
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        minX = bottomCorner.x + transform.localScale.x / 2;
        maxX = topCorner.x - transform.localScale.x / 2;
        minY = bottomCorner.y + transform.localScale.y / 2;
        maxY = topCorner.y - transform.localScale.y / 2;
    }

    public bool Inside(Vector3 position) {
        return minX < position.x && maxX > position.x && minY < position.y && maxY > position.y;
    }
}
