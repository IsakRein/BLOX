using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibButton : MonoBehaviour
{
    public Color backColor1;
    public Color backColor2;

    public GameObject soundOn;
    public GameObject soundOff;

    public bool colorSwitch;

    public Image backSpr;

    void Start() {
        backColor1 = Manager.staticTheme[13];
        backColor2 = Manager.staticTheme[14];

        if (Manager.vibEnabled) {
            colorSwitch = false;
        }
        else {
            colorSwitch = true;
        }

        colorSwitch = !colorSwitch;

        if (colorSwitch)
        {
            Manager.vibEnabled = true;

            Manager.SaveVibSettings();

            soundOn.SetActive(true);
            soundOff.SetActive(false);

            backSpr.color = backColor1;
        }
        else
        {
            Manager.vibEnabled = false;

            Manager.SaveVibSettings();

            soundOn.SetActive(false);
            soundOff.SetActive(true);

            backSpr.color = backColor2;
        }
    }   

    public void OnClick()
    {
        colorSwitch = !colorSwitch;

        if (colorSwitch)
        {
            Manager.vibEnabled = true;

            Manager.SaveVibSettings();

            soundOn.SetActive(true);
            soundOff.SetActive(false);

            backSpr.color = backColor1;

            Vibrate();
        }
        else
        {
            Manager.vibEnabled = false;

            Manager.SaveVibSettings();

            soundOn.SetActive(false);
            soundOff.SetActive(true);

            backSpr.color = backColor2;
        }
    }

    void Vibrate()
    {
        Handheld.Vibrate();
    }
}
