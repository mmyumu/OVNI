using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour {
    public BezierSpline spline;
    public float moveSpeed = 1f;
    // public float duration = 5f;
    public bool lookForward;
    public SplineWalkerMode mode;

    private float progress;
    private bool goingForward = true;

    public enum SplineWalkerMode {
        Once,
        Loop,
        PingPong
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    private void Update() {
        if (spline) {
            if (goingForward) {
                progress += GetProgress();
                if (progress > 1f) {
                    if (mode == SplineWalkerMode.Once) {
                        progress = 1f;
                    } else if (mode == SplineWalkerMode.Loop) {
                        progress -= 1f;
                    } else {
                        progress = 2f - progress;
                        goingForward = false;
                    }
                }
            } else {
                progress -= GetProgress();
                if (progress < 0f) {
                    progress = -progress;
                    goingForward = true;
                }
            }

            Vector3 position = spline.GetPoint(progress);
            transform.localPosition = position;
            if (lookForward) {
                // 2D
                transform.right = spline.GetDirection(progress);

                // 3D
                // transform.LookAt(position + spline.GetDirection(progress));
            }
        }
    }

    float GetProgress() {
        // return Time.deltaTime / duration;
        return Time.deltaTime * moveSpeed;
    }
}
