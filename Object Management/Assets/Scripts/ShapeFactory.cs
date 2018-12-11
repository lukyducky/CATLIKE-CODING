using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject {
    //using ScriptableObject instead of monobehavior as it is part of the project, not a particular scene

    [SerializeField]
    Shape[] prefabs;

    [SerializeField]
    Material[] materials;

    [SerializeField]
    bool recycle;

    List<Shape>[] pools;

    Scene poolScene; //holds all recyclable shapes

    public Shape Get(int shapeId = 0, int matId = 0) {
        Shape instance;
        if(recycle) {
            if (pools == null) {
                CreatePools();
            }
            //extracting instance from correct pool; we just get last obj since order doesn't matter
            List<Shape> pool = pools[shapeId]; //use shape Id to get right pool
            int lastIndex = pool.Count - 1;
            if(lastIndex >= 0) {
                instance = pool[lastIndex]; //grab element from pool
                instance.gameObject.SetActive(true); //activate it
                pool.RemoveAt(lastIndex); //remove obj from pool
            }
            else {
                instance = Instantiate(prefabs[shapeId]);
                instance.ShapeId = shapeId;
                SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene); //migrating shapes to poolscene
            }
        }
        else {
            instance = Instantiate(prefabs[shapeId]);
            instance.ShapeId = shapeId;
        }    
        instance.SetMaterial(materials[matId], matId);
        return instance;
    }

    public void Reclaim(Shape reShape) { 
        //object pool reclaims objs for recycling
        if(recycle) {
            if (pools == null) {
                CreatePools();
            }
            pools[reShape.ShapeId].Add(reShape);
            reShape.gameObject.SetActive(false);
        }
        else {
            Destroy(reShape.gameObject);
        }
    }

    public Shape GetRandom() {
        return Get(
            Random.Range(0, prefabs.Length), 
            Random.Range(0, materials.Length));
    }

    void CreatePools() { //makes empty list in pools for each entry in prefabs
        pools = new List<Shape>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++) {
            pools[i] = new List<Shape>();
        }
        if(Application.isEditor) {
            if(poolScene.isLoaded) {
                GameObject[] rootObjs = poolScene.GetRootGameObjects();
                for (int i = 0; i < rootObjs.Length; i++) {
                    Shape pooledShape = rootObjs[i].GetComponent<Shape>();
                    if(!pooledShape.gameObject.activeSelf) {
                        pools[pooledShape.ShapeId].Add(pooledShape);
                    }
                }
                return;
            }
        }
        

        poolScene = SceneManager.CreateScene(name);
    }


}
