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

	void Start() {
		OnClick ();
	}	

	void OnClick() {
		colorSwitch = !colorSwitch;

		if (colorSwitch) {
			soundOn.SetActive (true);
			soundOff.SetActive (false);

			backSpr.color = backColor1;

			Vibrate ();
		} else {
			soundOn.SetActive (false);
			soundOff.SetActive (true);

			backSpr.color = backColor2;
		}
	}

	void Vibrate() {
		Handheld.Vibrate ();
	}
}
