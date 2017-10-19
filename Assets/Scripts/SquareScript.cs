using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SquareScript : MonoBehaviour {

	public GameObject Line;

	public LineScript lineScript;

	public Color[] setColor;
	private int colorNum;

	public SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		Line = GameObject.Find ("LineHolder");
		lineScript = Line.GetComponent<LineScript>();

		colorNum = Random.Range (0, setColor.Length);
		spriteRenderer.color = setColor [colorNum];
	}

	void OnTouchDown() {
		
	}
	
	void OnTouchUp() {

	}	

	void OnTouchStay() {
		int num = System.Convert.ToInt32 (gameObject.name);

		//SquareSend send = new SquareSend{ num, colorNum };  

		//Line.SendMessage ("AddSquare", send);

		lineScript.AddSquare (num, colorNum);
	}

	void OnTouchExit() {
		
	}
}
