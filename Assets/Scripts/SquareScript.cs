using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SquareScript : MonoBehaviour {

	public GameObject squares;
	public GameObject Line;

	public Game squareScript;
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

	public int fallCounter;

	public float speed = 100f;

	private Vector3 targetPos;

	private bool fallInitialized;

	public bool largestValue = false;

	public bool resetTimer;

	private float time = 0.5f;

	void Start() {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();

		Line = GameObject.Find ("LineHolder");
		lineScript = Line.GetComponent<LineScript>();

		squares = GameObject.Find ("Squares");
		squareScript = squares.GetComponent<Game>();

		setColor = lineScript.setColor;

		colorNum = Random.Range (0, setColor.Length);
		spriteRenderer.color = setColor [colorNum];

		addSquareHasBeenCalled = false;

		isHovering = false;

		hoverSwitch = false;

		NameSquare ();
	}


	void Update () {
		if (!isHovering) {
			addSquareHasBeenCalled = false;
		} 

		if (lineScript.fallDown == true && fallCounter > 0)
		{
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed/75);

			if (targetPos == transform.localPosition)
			{
				fallCounter = 0;

				if (largestValue)
				{
					resetTimer = true;
				}
			}
		}

		if (resetTimer)
		{
			time = time - Time.deltaTime;

			if (time <= 0)
			{
				lineScript.FallingDone ();
				resetTimer = false;
				time = 0.5f;
			}
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
		squareRows = squareScript.Rows;

		float x = (transform.localPosition.x - (0.5f-(((float)squareRows)/2)))+1;
		float y = (((((float)squareRows)/2)-0.5f) - transform.localPosition.y);

		gameObject.name = "" + ((squareRows * y) + x);
	}


	void SetLargestValue() {
		largestValue = true;
	}


	void DisableLargestValue() {
		largestValue = false;
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
		
	void InitializeFall() {
		targetPos = transform.localPosition;
		targetPos.y = transform.localPosition.y - fallCounter;
	}

	void Animate() {
		animator.SetTrigger("Trigger");
	}

	void OnTouchExit() {
		isHovering = false;
	}

	void EnableFall() {

		lineScript.InitializeFall ();
	}

	void PlaySound() {
		lineScript.PlaySound ();
	}

	void MakeInteractable () {
		interactable = true;
	}

	void DisableGameObject () {

		for (int i = int.Parse (name); i > 0; i = i - squareRows)
		{
			GameObject.Find ("Game/Squares/" + i.ToString ()).SendMessage ("AddToFallCounter", 1, SendMessageOptions.DontRequireReceiver);
		}

		GameObject.Destroy (gameObject);	
	}

	void AddToFallCounter(int howMuch) 
	{
		fallCounter = fallCounter + howMuch;
	}


}