using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public Text distanceLabel, speedLabel;

    public void SetValues(float distanceTraveled, float velocity)
    {
        distanceLabel.text = ((int)(distanceTraveled * 10f)).ToString();
        speedLabel.text = ((int)(velocity * 10f)).ToString();
    }

}
