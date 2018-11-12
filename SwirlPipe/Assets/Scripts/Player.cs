using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    #region data

    public PipeSystem pipeSystem;
    public float velocity, rotationVelocity, startVelocity;
    public MainMenu mainMenu;
    public float[] accelerations;
    public HUD hud;

    Pipe currentPipe;
    float distanceTraveled, acceleration;
    
    float deltaToRotation, systemRotation, worldRotation, avatarRotation;
    Transform world, rotater;
#endregion

    private void Update()
    {
        velocity += acceleration * Time.deltaTime;
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
        UpdateAvatarRotation();

        hud.SetValues(distanceTraveled, velocity);
    }

    private void Awake(){
        world = pipeSystem.transform.parent;
        rotater = transform.GetChild(0);
        gameObject.SetActive(false);
    }

    public void StartGame(int accelMode){
        distanceTraveled = 0f;
        avatarRotation = 0f;
        systemRotation = 0f;

        acceleration = accelerations[accelMode];
        velocity = startVelocity;

        currentPipe = pipeSystem.SetupFirstPipe();
        SetUpCurrentPipe();
        gameObject.SetActive(true);

        hud.SetValues(distanceTraveled, velocity);
    }

    void UpdateAvatarRotation()
    {
        avatarRotation += rotationVelocity * Time.deltaTime * Input.GetAxis("Horizontal");
        if (avatarRotation < 0f)
        {
            avatarRotation += 360f;
        }
        else if(avatarRotation >= 360f)
        {
            avatarRotation -= 360f;
        }
        rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
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

    public void Die(){
        mainMenu.EndGame(distanceTraveled);
        gameObject.SetActive(false);

    }

    
}
