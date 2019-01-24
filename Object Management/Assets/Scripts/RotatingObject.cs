using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : PersistableObject {
    [SerializeField]
    Vector3 angularVelocity;
    [SerializeField]
    bool isRotating;

    private void FixedUpdate() {
        if(isRotating) {
            transform.Rotate(angularVelocity * Time.deltaTime); 
        }
        
    }

}
