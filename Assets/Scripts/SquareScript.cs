using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SquareScript : MonoBehaviour {

	public GameObject Line;

	public LineScript lineScript;

	public Color[] setColor;
	public int colorNum;

	public SpriteRenderer spriteRenderer;

	private bool isOnMobile = true;

	public bool touchSwitch = true;

	public bool hoverSwitch = false;

	public bool hoverSwitch2 = false;

	[Space]

	public bool isHovering = false;
	public int counter = 0;



	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		Line = GameObject.Find ("LineHolder");
		lineScript = Line.GetComponent<LineScript>();

		setColor = lineScript.setColor;

		colorNum = Random.Range (0, setColor.Length);
		spriteRenderer.color = setColor [colorNum];

		isHovering = false;

		touchSwitch = false;
		hoverSwitch = false;
		hoverSwitch2 = true;
	}


	void Update () {
		if (isHovering) {
			if (touchSwitch) {
				hoverSwitch2 = !hoverSwitch2;
				touchSwitch = false;

				if (hoverSwitch2) {
					hoverSwitch = !hoverSwitch;
				}
			} else {
				hoverSwitch = false;
			}
		} 
		else {
			touchSwitch = true;
		}

		#if UNITY_EDITOR
		isOnMobile = false;

		if (!(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))) {
			touchSwitch = false;
			hoverSwitch = false;
			hoverSwitch2 = true;		
		}
		#endif

		if (isOnMobile) {
			if (Input.touchCount == 0) {
				touchSwitch = false;
				hoverSwitch = false;
				hoverSwitch2 = true;
			}
		}
	}


	void OnTouchDown() {
		
	}
	
	void OnTouchUp() {
		isHovering = false;
	}	

	void OnTouchStay() {
		int num = System.Convert.ToInt32 (gameObject.name);
		lineScript.AddSquare (num, colorNum, hoverSwitch, hoverSwitch2);

		isHovering = true;
	}

	void OnTouchExit() {
		isHovering = false;
	}


	void TriggerNextSquare () {
		lineScript.SwitchColor ();
	}

	void ChangeColor() {
		spriteRenderer.color = setColor [colorNum];
	}

	void ControlSwitch() {
		lineScript.EnableControllers ();
	}

	void PlaySound() {
		lineScript.PlaySound ();
	}
}