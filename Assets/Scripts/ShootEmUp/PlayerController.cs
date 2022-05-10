using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public GameObject bulletPrefab;

    private Vector3 direction;
    private bool isFiring = false;
    private Boundaries boundaries;


    // Start is called before the first frame update
    void Start()
    {
        boundaries = gameObject.GetComponent<Boundaries>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        CheckBoundaries();
    }

    private void CheckBoundaries()
    {
        if (boundaries)
        {
            // Get current position
            Vector3 pos = transform.position;

            // Horizontal contraint
            if (pos.x < boundaries.minX) pos.x = boundaries.minX;
            if (pos.x > boundaries.maxX) pos.x = boundaries.maxX;

            // vertical contraint
            if (pos.y < boundaries.minY) pos.y = boundaries.minY;
            if (pos.y > boundaries.maxY) pos.y = boundaries.maxY;

            // Update position
            transform.position = pos;
        } else
        {
            Debug.Log("Cannot check player boundaries");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Move!");
        direction = context.ReadValue<Vector2>();
    }

    private void Fire()
    {
        Vector3 spawnPos = transform.position;
        // spawnPos.y += transform.localScale.y / 2;


        //GameObject bullet = Instantiate(bulletPrefab, spawnPos, new Quaternion(1f, 0f, 1f, 0f));
        //GameObject bullet = Instantiate(bulletPrefab, spawnPos, transform.rotation);
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(new Vector3(0, 0, 0)) * transform.rotation );
        
        bullet.tag = "Player";
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        bool pressedFire = context.ReadValueAsButton();
        if (pressedFire && !isFiring)
        {
            InvokeRepeating("Fire", 0, 0.2f);
            isFiring = true;

        } else if (!pressedFire)
        {
            CancelInvoke("Fire");
            isFiring = false;
        }
    }
}
