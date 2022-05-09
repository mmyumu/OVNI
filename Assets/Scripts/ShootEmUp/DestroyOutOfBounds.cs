using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private Boundaries boundaries;

    // Start is called before the first frame update
    void Start()
    {
        boundaries = gameObject.GetComponent<Boundaries>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boundaries)
        {
            if (transform.position.x > boundaries.maxX + 10) Destroy(gameObject);
            if (transform.position.x < boundaries.minX - 10) Destroy(gameObject);
            if (transform.position.y > boundaries.maxY + 10) Destroy(gameObject);
            if (transform.position.y < boundaries.minY - 10) Destroy(gameObject);
        } else
        {
            Debug.Log("Cannot destroy object " + gameObject.name + " because boundaries are missing");
        }
    }
}

