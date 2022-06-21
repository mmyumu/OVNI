using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletsPrefabs))]
public class BulletsPrefabsEditor : Editor {
    private BulletsPrefabs bulletsPrefabs;

    public override void OnInspectorGUI() {
        bulletsPrefabs = target as BulletsPrefabs;
       
        foreach (string key in bulletsPrefabs.GetKeys()) {
            GameObject bulletPrefab = bulletsPrefabs.GetKey(key);
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key);
            EditorGUILayout.ObjectField(bulletPrefab, typeof(GameObject), true);
            if (GUILayout.Button("-")) {
                Undo.RecordObject(bulletsPrefabs, "Remove bullet prefab");
                bulletsPrefabs.Remove(key);
            }
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("New");
        
        EditorGUI.BeginChangeCheck();
        GameObject gameObject = (GameObject) EditorGUILayout.ObjectField(null, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(bulletsPrefabs, "Add Bullet Prefab");
            bulletsPrefabs.Add(gameObject);
            EditorUtility.SetDirty(bulletsPrefabs);
        }
        GUILayout.EndHorizontal();
    }
}