using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAI : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Boundaries boundaries;
    private bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        boundaries = GetComponent<Boundaries>();

        if (!boundaries)
        {
            StartFire();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Start to fire only when actually in screen boundaries
        if (!isFiring && boundaries && boundaries.Inside(transform.position)) {
            StartFire();
        }
    }

    private void StartFire() {
        if (!isFiring) {
            InvokeRepeating("Fire", 0, 1f);
            isFiring = true;
        }
    }

    private void Fire()
    {
        Vector3 spawnPos = transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0, 0)) * transform.rotation);

        bullet.tag = "Enemy";
    }
}
