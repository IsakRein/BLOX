using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBackground : MonoBehaviour {

    public Canvas canvas;
    public Canvas canvas2;
    private bool changeCanvas;
    public LineScript LineScript;

    private void Start()
    {
        changeCanvas = false;
    }

    void StartAnim () {
        LineScript.controlsEnabled = false;
    }

    void EndAnim () {
        if (changeCanvas) {
            canvas.planeDistance = 100;
            canvas2.planeDistance = 100;
            changeCanvas = false;
        }
        else {
            canvas.planeDistance = 101;
            canvas2.planeDistance = 101;
            changeCanvas = true;
        }

        gameObject.SetActive(false);
    }
}
