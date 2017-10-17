using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour {

	public Color color1;
	public Color color2;

	public Color[] color;
	private int colorNum;

	public SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		colorNum = Random.Range (0, 3);
		spriteRenderer.color = color [colorNum];
	}

	void OnTouchDown() {
		gameObject.SetActive (false);
	}

	void OnTouchUp() {
		spriteRenderer.color = color1;
	}

	void OnTouchStay() {
		spriteRenderer.color = color2;
	}

	void OnTouchExit() {
		spriteRenderer.color = color2;
	}
}
