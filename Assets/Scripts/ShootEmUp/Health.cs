using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int max = 100;
    public int current;

    // Start is called before the first frame update
    void Start()
    {
        current = max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int damage(int damage)
    {
        current -= damage;

        if (current <= 0)
        {
            Destroy(gameObject);
        }

        return current;
    }
}
