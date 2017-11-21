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

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	private bool isOnMobile = true;

	[Space]

	public bool hoverSwitch = false;

	[Space]

	public bool isHovering = false;

	public bool addSquareHasBeenCalled = false;

	public bool interactable = false;

	private int squareRows;

	void Start() {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();

		Line = GameObject.Find ("LineHolder");
		lineScript = Line.GetComponent<LineScript>();

		setColor = lineScript.setColor;

		colorNum = Random.Range (0, setColor.Length);
		spriteRenderer.color = setColor [colorNum];

		addSquareHasBeenCalled = false;

		isHovering = false;

		hoverSwitch = false;

		lineScript.AddToColorList (System.Convert.ToInt32 (gameObject.name), colorNum);

		squareRows = lineScript.squareRows;

		animator.SetTrigger ("OnEnable");
		NameSquare ();
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

	void NameSquare() {
		float x = (transform.localPosition.x - (0.5f-(squareRows / 2)))+1;
		float y = (((squareRows / 2)-0.5f) - transform.localPosition.y);

		gameObject.name = "" + ((squareRows * y) + x);
	}


	void OnTouchDown() {
		isHovering = true;


		if (addSquareHasBeenCalled == false && interactable) {
			int num = System.Convert.ToInt32 (gameObject.name);
			lineScript.AddSquare (num, colorNum, hoverSwitch);

			addSquareHasBeenCalled = true;
		}
	}
	
	void OnTouchUp() {
		isHovering = false;
	}	

	void OnTouchStay() {
		isHovering = true;


		if (addSquareHasBeenCalled == false && interactable) {
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

	void MakeInteractable () {
		interactable = true;
	}

	void DisableGameObject () {
		GameObject.Destroy (gameObject);	
	}
}