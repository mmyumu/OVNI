using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionEntrance : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    private Boundaries boundaries;
    private ShootEmUpManager shootEmUpManager;
    private bool spawned = false;
    private Vector3 destPos;

    // Start is called before the first frame update
    void Start()
    {
        boundaries = gameObject.GetComponent<Boundaries>();
        shootEmUpManager = GameObject.Find("ShootEmUpManager").GetComponent<ShootEmUpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootEmUpManager.PlayingIntroduction()) {
            if (!spawned) {
                transform.position = new Vector3(0, boundaries.minY - 1);
                destPos = new Vector3(0, boundaries.minY + 2);
                spawned = true;
            } else {
                if (Vector3.SqrMagnitude(transform.position - destPos) > 0.001) {
                    Vector3 direction = destPos - transform.position;
                    direction.Normalize();
                    transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
    }
}
