using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCompleteExit : MonoBehaviour {
    public float moveSpeed = 1f;
    public float acceleration = 2f;

    public GameObject topLeftPrefab;
    public GameObject topRightPrefab;
    public GameObject bottomLeftPrefab;
    public GameObject bottomRightPrefab;

    private BezierSpline spline;
    private Boundaries boundaries;
    private ShootEmUpManager shootEmUpManager;
    private bool exiting = false;
    private bool waiting = true;
    private float progress;
    private float currentSpeed;
    

    // Start is called before the first frame update
    void Start() {
        boundaries = GetComponent<Boundaries>();
        shootEmUpManager = GameObject.Find("ShootEmUpManager").GetComponent<ShootEmUpManager>();

        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update() {
        if (shootEmUpManager.IsMissionComplete()) {
            if (!exiting) {
                exiting = true;
                GameObject splinePrefab = GetSplinePrefab();
                GameObject go = Instantiate(splinePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)) * transform.rotation);
                spline = go.GetComponent<BezierSpline>();
                StartCoroutine(WaitBeforeExit());
            } else if (!waiting) {
                Exit();
            }
        }
    }

    private GameObject GetSplinePrefab() {
        if (boundaries.IsTopLeft(transform.position)) return bottomRightPrefab;
        if (boundaries.IsTopRight(transform.position)) return bottomLeftPrefab;
        if (boundaries.IsBottomLeft(transform.position)) return topRightPrefab;
        if (boundaries.IsBottomRight(transform.position)) return topLeftPrefab;

        return topLeftPrefab;
    }

    private IEnumerator WaitBeforeExit() {
        yield return new WaitForSeconds(2f);
        waiting = false;
    }

    private void Exit() {
        currentSpeed = currentSpeed + (Time.deltaTime * acceleration);
        progress += GetProgress();
        if (progress > 1f) {
            progress = 1f;
        }

        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
    }

    private float GetProgress() {
        return Time.deltaTime * currentSpeed;
    }
}
