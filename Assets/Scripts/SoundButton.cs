using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {

	public Color backColor1;
	public Color backColor2;

	public GameObject soundOn;
	public GameObject soundOff;

	public bool colorSwitch;

	public Image backSpr;

    public AudioClip aClip;
    private AudioSource audioSource;

	void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();

        backColor1 = Manager.staticTheme[13];
        backColor2 = Manager.staticTheme[14];

        /*
         
        if (Manager.selectedTheme == 4)
        {
            soundOff.GetComponent<SpriteRenderer>().color = backColor1;
        }

        */

        if (Manager.soundEnabled) {
            colorSwitch = false;
        }
        else {
            colorSwitch = true;
        }

        colorSwitch = !colorSwitch;

        if (colorSwitch)
        {
            Manager.soundEnabled = true;

            Manager.SaveSoundSettings();

            soundOn.SetActive(true);
            soundOff.SetActive(false);

            backSpr.color = backColor1;
        }

        else
        {
            Manager.soundEnabled = false;

            Manager.SaveSoundSettings();

            soundOn.SetActive(false);
            soundOff.SetActive(true);

            backSpr.color = backColor2;
        }    
    }	

	void OnClick() {
		colorSwitch = !colorSwitch;

		if (colorSwitch) {
            Manager.soundEnabled = true;

            Manager.SaveSoundSettings();

			soundOn.SetActive (true);
			soundOff.SetActive (false);

			backSpr.color = backColor1;

			PlaySound ();
		} else {
            Manager.soundEnabled = false;

            Manager.SaveSoundSettings();

			soundOn.SetActive (false);
			soundOff.SetActive (true);

			backSpr.color = backColor2;
		}
	}

	void PlaySound() {
        audioSource.PlayOneShot(aClip);
    }
}
