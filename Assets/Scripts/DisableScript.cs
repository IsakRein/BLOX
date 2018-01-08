using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableScript : MonoBehaviour {

    public Canvas canvas;
    private bool changeCanvas;
    public Image image;
    public LineScript LineScript;

    void Start() {
        changeCanvas = false;
    }

    void Disable() {
        if (changeCanvas)
        {
            canvas.planeDistance = 100;
            changeCanvas = false;
        }
        else
        {
            canvas.planeDistance = 101;
            changeCanvas = true;
        }

        LineScript.controlsEnabled = true;

        gameObject.SetActive(false);
	}

    void EnableImage () {
        image.enabled = true;
    }
}
