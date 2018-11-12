using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Player player;
    public Text scoreLabel;

    public void StartGame(int mode){
        player.StartGame(mode);
        gameObject.SetActive(false);
    }

    public void EndGame(float disTraveled)
    {
        scoreLabel.text = ((int)(disTraveled * 10f)).ToString();
        gameObject.SetActive(true);
    }
}
