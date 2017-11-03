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

	[Space]

	public bool hoverSwitch = false;

	[Space]

	public bool isHovering = false;

	public bool addSquareHasBeenCalled = false;

	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		Line = GameObject.Find ("LineHolder");
		lineScript = Line.GetComponent<LineScript>();

		setColor = lineScript.setColor;

		colorNum = Random.Range (0, setColor.Length);
		spriteRenderer.color = setColor [colorNum];

		addSquareHasBeenCalled = false;

		isHovering = false;

		hoverSwitch = false;
	}


	void Update () {
		if (!isHovering) {
			addSquareHasBeenCalled = false;
		} 

		#if UNITY_EDITOR
		isOnMobile = false;

		if (!(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))) {
			addSquareHasBeenCalled = false;

			hoverSwitch = false;		
		}
		#endif

		if (isOnMobile) {
			if (Input.touchCount == 0) {
				addSquareHasBeenCalled = false;

				hoverSwitch = false;
			}
		}
	}


	void OnTouchDown() {
		
	}
	
	void OnTouchUp() {
		isHovering = false;
	}	

	void OnTouchStay() {
		isHovering = true;


		if (addSquareHasBeenCalled == false) {
			int num = System.Convert.ToInt32 (gameObject.name);
			lineScript.AddSquare (num, colorNum, hoverSwitch);

			addSquareHasBeenCalled = true;
		}
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