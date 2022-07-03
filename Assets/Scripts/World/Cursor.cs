using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour {
    public float moveSpeed = 1.0f;

    private Vector3 direction;
    private GameObject map;

    // Start is called before the first frame update
    void Start() {
        map = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        CheckMapBoundaries();
    }

    public void OnMove(InputAction.CallbackContext context) {
        Debug.Log("Move!");
        direction = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context) {
        bool pressedFire = context.ReadValueAsButton();
        Debug.Log("Press!");
    }

    //public void OnBack(InputAction.CallbackContext context) {
    //    // bool pressedCancel = context.ReadValueAsButton();
    //    Debug.Log("Back!");
    //}

    private void CheckMapBoundaries() {
        Bounds mapBoundaries = map.GetComponent<SpriteRenderer>().sprite.bounds;

        // Get current position
        Vector3 pos = transform.localPosition;
        // Vector3 pos = transform.position;



        // Horizontal contraint
        if (pos.x < mapBoundaries.min.x) pos.x = mapBoundaries.min.x;
        if (pos.x > mapBoundaries.max.x) pos.x = mapBoundaries.max.x;

        // vertical contraint
        if (pos.y < mapBoundaries.min.y) pos.y = mapBoundaries.min.y;
        if (pos.y > mapBoundaries.max.y) pos.y = mapBoundaries.max.y;

        // Update position
        transform.localPosition = pos;
    }
}
