using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScript : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI commentText;
    public GameObject ShareBtn;

    void Start () {

        Manager.loadColors = false;

        scoreText.SetText(Manager.previousScore.ToString());
        ShareBtn.SendMessage("SetPreviousScore", Manager.previousScore);

        GenerateText();
	}
	
    void GenerateText() {
        string message;

        if (Manager.previousScore < 50) {
            message = "What were you thinking?";
        }
        else if (Manager.previousScore < 100)
        {
            message = "Terrible!";
        }
        else if (Manager.previousScore < 300)
        {
            message = "Still pretty bad...";
        }
        else if (Manager.previousScore < 500)
        {
            message = "You're getting there!";
        }
        else if (Manager.previousScore < 750)
        {
            message = "Ok!";
        }
        else if (Manager.previousScore < 1000)
        {
            message = "Pretty good!";
        }
        else if (Manager.previousScore < 1500)
        {
            message = "Great!";
        }
        else if (Manager.previousScore < 2000)
        {
            message = "Now you're hitting the charts!";
        }
        else {
            message = "Great but you should consider getting a life...";
        }

        commentText.SetText(message);
    }
}
