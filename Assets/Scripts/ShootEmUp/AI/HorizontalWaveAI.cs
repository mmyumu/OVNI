using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalWaveAI : MonoBehaviour {
    public Vector3 destPos;
    public float xOffset = 0.0f;
    public float moveSpeed = 4.0f;
    public float frequency = 4.0f;
    public float? initialTime = null;

    private Vector3 spawnPos;
    

    // Start is called before the first frame update
    void Start() {
        //boundaries = GetComponent<Boundaries>();
        spawnPos = transform.position;

        if (spawnPos.y != destPos.y) {
            Debug.LogError($"Spawn position and Dest position are not horizontal (spawnPos.y=={spawnPos.y} != destPos.y=={destPos.y})");
        }
    }

    // Update is called once per frame
    void Update() {
        if (initialTime == null) {
            initialTime = Time.time;
        }
        //elapsedTime += Time.deltaTime;
        //float cosineValue = Mathf.Cos(2.0f * Mathf.PI * frequency * elapsedTime);
        //transform.position = spawnPos + (destPos - spawnPos) * 0.5f * (1 - cosineValue);

        //Vector3 direction = destPos - transform.position;
        //direction.Normalize();
        //transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // float newX = (destPos.x - spawnPos.x) * Mathf.Sin(Time.time * moveSpeed);
        //float newX = abs((x++ % 6) - 3);

        float xDiff = Mathf.Abs(destPos.x - spawnPos.x);

        // Triangular function for X coordinate (use triangular instead of Sin otherwise it slows down too much on the edge)
        float newX = Mathf.Abs(((GetTime() * moveSpeed + xOffset) % (xDiff * 4)) - (xDiff * 2)) - xDiff;

        // Sinusoidal on Y coordinate
        float newY = spawnPos.y + Mathf.Sin(GetTime() * frequency);

        transform.position = new Vector3(newX, newY);
    }

    private float GetTime() {
        return Time.time - (float) initialTime;
    }
}