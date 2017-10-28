using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour {
	public int lastSquare = 0;

	public GameObject square;
	public GameObject square2;
	public GameObject lastOkSquare;

	public GameObject dragLine;
	public GameObject dragCircle;

	private Vector3 point1;
	private Vector3 point2;

	public bool updateInitialize = true;
	public bool newSquareInitialize = true;
	public bool isOnMobile = true;

	public GameObject circlePrefab;
	GameObject circle;
	public GameObject linePrefab;
	GameObject line;

	public Game Squares;

	private GameObject lineChild;

	private float localScaleX;
	private float localScaleY;
	 
	public Color[] setColor;

	public int currentSquare;
	public int currentColor;

	public SpriteRenderer dragLineSpr;
	public SpriteRenderer dragCircleSpr;

	private int lastDrawnSquare;

	public List<int> squareList = new List<int>();

	void Start() {
		
	}
	
	void Update () {

		#if UNITY_EDITOR
		isOnMobile = false;

		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) {
			if (lastSquare != 0) {
				if (updateInitialize == true) {
					square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

					dragLine.SetActive (true);
					dragCircle.SetActive (true);

					dragCircle.transform.localScale = new Vector3 (square2.transform.lossyScale.x*1.5f, square.transform.lossyScale.x*1.5f, 1);

					updateInitialize = false;
				}

				if (newSquareInitialize == true) {
					dragCircleSpr.color = setColor [currentColor];
					dragLineSpr.color = setColor [currentColor];

					lastOkSquare = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

					newSquareInitialize = false;
				}
					
				point1 = lastOkSquare.transform.position;
				point2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				point2[2] = 0;

				dragCircle.transform.position = point2; 
				dragLine.transform.position = point1;

				float rot_z = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
				dragLine.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

				dragLine.transform.position = point1;
				Vector2 direction = point2 - point1;
				dragLine.transform.localScale = new Vector3(direction.magnitude*1.25f,circle.transform.lossyScale.x/5f,1);
			}
		} 

		else if (Input.GetMouseButtonUp(0)) {
			if (updateInitialize == false) {
				dragLine.SetActive (false);
				dragCircle.SetActive (false);

				RemoveLine ();
				SwitchColor();
				lastSquare = 0;
				squareList.Clear ();

				updateInitialize = true;
			}
		}


		#endif

		if (isOnMobile) {
			if (Input.touchCount > 0) {
				if (lastSquare != 0) {
					if (updateInitialize == true) {
						square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

						dragLine.SetActive (true);
						dragCircle.SetActive (true);

						dragCircle.transform.localScale = new Vector3 (square2.transform.lossyScale.x * 1.5f, square.transform.lossyScale.x * 1.5f, 1);


						updateInitialize = false;
					}

					if (newSquareInitialize == true) {
						dragCircleSpr.color = setColor [currentColor];
						dragLineSpr.color = setColor [currentColor];

						lastOkSquare = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

						newSquareInitialize = false;
					}


					Vector3 point1 = lastOkSquare.transform.position;
					Vector3 point2 = Camera.main.ScreenToWorldPoint (Input.touches [0].position);
					point2 [2] = 0;

					dragCircle.transform.position = point2; 
					dragLine.transform.position = point1;

					float rot_z = Mathf.Atan2 (point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
					dragLine.transform.rotation = Quaternion.Euler (0f, 0f, rot_z);

					dragLine.transform.position = point1;
					Vector2 direction = point2 - point1;
					dragLine.transform.localScale = new Vector3 (direction.magnitude * 1.25f, circle.transform.lossyScale.x / 5f, 1);
				}

			}
			else {
				if (updateInitialize == false) {
					dragLine.SetActive (false);
					dragCircle.SetActive (false);

					RemoveLine ();
					SwitchColor();
					lastSquare = 0;
					squareList.Clear ();

					updateInitialize = true;
				}
			}
		}
	}

	public void AddSquare(int squareNum, int colorNum) {
		square = GameObject.Find ("Game/Squares/" + squareNum.ToString());
		square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString());

		if (lastSquare == 0) {
			circle = Instantiate (circlePrefab, transform) as GameObject;
			circle.transform.position = square.transform.position;
			circle.transform.localScale = new Vector3 (square.transform.lossyScale.x*1.5f, square.transform.lossyScale.x*1.5f, 1);

			if (colorNum == setColor.Length-1) {
				currentColor = 0;
			} else {
				currentColor = colorNum + 1;
			}

			square.GetComponent<SquareScript>().colorNum = currentColor;

			circle.GetComponent<SpriteRenderer> ().color = setColor [currentColor];

			newSquareInitialize = true;
				
			lastSquare = squareNum;
			squareList.Add (squareNum);

		} else {	
			if (square2.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color && !squareList.Contains(squareNum)) {
				int squarerows = Squares.Rows;

				if (squareNum == lastSquare - squarerows) {
					DrawLine (square, square2, squareNum, 90, colorNum);
				} else if (squareNum == lastSquare - 1 && (squareNum % squarerows != 0)) {
					DrawLine (square, square2, squareNum, 180, colorNum);
				} else if (squareNum == lastSquare + 1 && ((squareNum - 1) % squarerows != 0)) {
					DrawLine (square, square2, squareNum, 0, colorNum);
				} else if (squareNum == lastSquare + squarerows) {
					DrawLine (square, square2, squareNum, 270, colorNum);
				}

				//squares more than one square away
				else if (squareNum % squarerows == lastSquare % squarerows) {
					bool drawOrNot = true;

					for (int i = 0; i < ((Mathf.Abs(squareNum - lastSquare))/squarerows); i++) {


					}
				}
			}
		}
	}

	void DrawLine(GameObject square, GameObject square2, int squareNum, float rotation, int colorNum) 
	{
		circle = Instantiate (circlePrefab, transform) as GameObject;
		circle.transform.position = square.transform.position;
		circle.transform.localScale = new Vector3 (square.transform.lossyScale.x*1.5f, square.transform.lossyScale.x*1.5f, 1);

		line = Instantiate (linePrefab, transform) as GameObject;
		line.transform.position = square2.transform.position;
		line.transform.localEulerAngles = new Vector3 (0, 0, rotation);

		foreach (Transform child in line.transform)
		{
			if (child.name == "Square") {
				lineChild = child.gameObject;
			}
		}	

		localScaleX = (square.transform.lossyScale.x * 10)/9f;
		localScaleY = (circle.transform.lossyScale.x/5f);

		lineChild.transform.localPosition = new Vector2(((square.transform.lossyScale.x/2) * 10)/9f, 0);

		lineChild.transform.localScale = new Vector3 (localScaleX, localScaleY, 1);

		if (colorNum == setColor.Length-1) {
			currentColor = 0;
		} else {
			currentColor = colorNum + 1;
		}
			
		square.GetComponent<SquareScript>().colorNum = currentColor;

		circle.GetComponent<SpriteRenderer> ().color = setColor [currentColor];
		lineChild.GetComponent<SpriteRenderer> ().color = setColor [currentColor];

		lastDrawnSquare = squareNum;
		lastSquare = squareNum;
		squareList.Add (squareNum);

		newSquareInitialize = true;
	}

	void RemoveLine() {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	void SwitchColor() {
		foreach (int square in squareList) {
			GameObject squareToChange = GameObject.Find ("Game/Squares/" + square.ToString ());
			//animate
			squareToChange.GetComponent<SpriteRenderer>().color = setColor [currentColor];
		}
	}
}