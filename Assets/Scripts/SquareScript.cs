using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SquareScript : MonoBehaviour {

	public GameObject Line;

	public Color[] setColor;
	private int colorNum;

	public SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		Line = GameObject.Find ("LineHolder");

		colorNum = Random.Range (0, setColor.Length);
		spriteRenderer.color = setColor [colorNum];
	}

	void OnTouchDown() {
		
	}
	
	void OnTouchUp() {

	}	

	void OnTouchStay() {
		int num = System.Convert.ToInt32 (gameObject.name);
		Line.SendMessage ("AddSquare", num);
	}

	void OnTouchExit() {
		
	}
}
