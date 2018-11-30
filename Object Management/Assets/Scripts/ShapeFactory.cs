using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject {
    //using ScriptableObject instead of monobehavior as it is part of the project, not a particular scene

    [SerializeField]
    Shape[] prefabs;

    [SerializeField]
    Material[] materials;

    public Shape Get(int shapeId = 0, int matId = 0) {
        Shape instance = Instantiate(prefabs[shapeId]);
        instance.ShapeId = shapeId;
        instance.SetMaterial(materials[matId], matId);
        return instance;
    }

    public Shape GetRandom() {
        return Get(
            Random.Range(0, prefabs.Length), 
            Random.Range(0, materials.Length));
    }
}
