using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAI : MonoBehaviour
{
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Fire()
    {
        Vector3 spawnPos = transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0, 0)) * transform.rotation);

        bullet.tag = "Enemy";
    }
}
