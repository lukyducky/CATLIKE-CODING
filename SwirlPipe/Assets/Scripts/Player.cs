using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PipeSystem pipeSystem;
    public float velocity;

    Pipe currentPipe;
    float distanceTraveled;
    
    float deltaToRotation, systemRotation, worldRotation;
    Transform world;

    private void Start()
    {
        world = pipeSystem.transform.parent;
        currentPipe = pipeSystem.SetupFirstPipe();
        SetUpCurrentPipe();

    }

    private void Update()
    {
        float delta = velocity * Time.deltaTime;
        distanceTraveled += delta;
        systemRotation += delta * deltaToRotation;

        if (systemRotation >= currentPipe.CurveAngle)
        {
            delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
            currentPipe = pipeSystem.SetupNextPipe();
            SetUpCurrentPipe();
            
            systemRotation = delta * deltaToRotation;
        }

        pipeSystem.transform.localRotation = Quaternion.Euler(0f, 0f, systemRotation);
    }

    void SetUpCurrentPipe(){
        deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
        worldRotation += currentPipe.RelativeRotation;
        if(worldRotation < 0f){
            worldRotation += 360f;
        }
        else if(worldRotation >= 360f){
            worldRotation -= 360f;
        }
        world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
    }
}
