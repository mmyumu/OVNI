using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAI : MonoBehaviour {
    public float moveSpeed = 1.0f;
    private Vector3 spawnPos;
    private Vector3 destPos;

    // Start is called before the first frame update
    void Start() {
        spawnPos = transform.position;
        destPos = spawnPos;
        destPos.y = destPos.y - 3;
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.SqrMagnitude(transform.position -destPos) > 0.01) {
            Vector3 direction = destPos - transform.position;
            direction.Normalize();
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
