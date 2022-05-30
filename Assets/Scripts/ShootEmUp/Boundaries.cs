using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour {
    [HideInInspector] public float minX, maxX, minY, maxY;

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

    public bool IsTop(Vector3 position) {
        return Inside(position) && position.y >= ((maxY + minY) / 2);
    }

    public bool IsBottom(Vector3 position) {
        return Inside(position) && position.y < ((maxY + minY) / 2);
    }

    public bool IsRight(Vector3 position) {
        return Inside(position) && position.x >= ((maxX + minX) / 2);
    }

    public bool IsLeft(Vector3 position) {
        return Inside(position) && position.x < ((maxX + minX) / 2);
    }

    public bool IsTopLeft(Vector3 position) {
        return IsTop(position) && IsLeft(position);
    }

    public bool IsTopRight(Vector3 position) {
        return IsTop(position) && IsRight(position);
    }

    public bool IsBottomLeft(Vector3 position) {
        return IsBottom(position) && IsLeft(position);
    }

    public bool IsBottomRight(Vector3 position) {
        return IsBottom(position) && IsRight(position);
    }
}
