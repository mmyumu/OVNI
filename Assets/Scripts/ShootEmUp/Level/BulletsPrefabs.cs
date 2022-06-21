using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPrefabs : MonoBehaviour {
    [SerializeField]
    private List<string> keys = new List<string>();

    [SerializeField]
    private List<GameObject> values = new List<GameObject>();

    public int Count() {
        return keys.Count;
    }

    public bool ContainsKey(string key) {
        return keys.Contains(key);
    }

    public List<string> GetKeys() {
        return keys;
    }

    public GameObject GetKey(string key) {
        if (keys.Contains(key)) {
            foreach (GameObject obj in values) {
                if (obj.name == key)
                    return obj;
            }
        }
        return null;
    }

    public void Add(GameObject gameObject) {
        if (!keys.Contains(gameObject.name)) {
            // create
            keys.Add(gameObject.name);
            values.Add(gameObject);
        }
    }

    public void Remove(string key) {
        if (keys.Contains(key)) {
            foreach (GameObject obj in values) {
                if (obj.name == key) {
                    values.Remove(obj);
                    break;
                }
            }
            keys.Remove(key);
        }
    }
}
