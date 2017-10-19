using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour {
	public int lastSquare = 0;

	public GameObject circlePrefab;
	GameObject circle;
	public GameObject linePrefab;
	GameObject line;

	public Game Squares;

	private GameObject lineChild;

	private float localScaleX;
	private float localScaleY;
	 
	public Color[] setColor;

	void Start() {
		
	}
	
	void Update () {
		if (Input.touchCount > 0) {
			
		}
	}

	public void AddSquare(int squareNum, int colorNum) {
		GameObject square;
		GameObject square2;

		square = GameObject.Find ("Game/Squares/" + squareNum.ToString());
		square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString());

		if (lastSquare == 0) {
			circle = Instantiate (circlePrefab, transform) as GameObject;
			circle.transform.position = square.transform.position;
			circle.transform.localScale = new Vector3 (square.transform.lossyScale.x/3, square.transform.lossyScale.x/3, 1);

			lastSquare = squareNum;
		} else {
			if (square2.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color) {
				if (squareNum == lastSquare - Squares.Rows) {
					DrawLine (square, square2, squareNum, 90);
				} else if (squareNum == lastSquare - 1) {
					DrawLine (square, square2, squareNum, 180);
				} else if (squareNum == lastSquare + 1) {
					DrawLine (square, square2, squareNum, 0);
				} else if (squareNum == lastSquare + Squares.Rows) {
					DrawLine (square, square2, squareNum, 270);
				}
			}
		}
	}

	void DrawLine(GameObject square, GameObject square2, int squareNum, float rotation) 
	{
		circle = Instantiate (circlePrefab, transform) as GameObject;
		circle.transform.position = square.transform.position;

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
		localScaleY = (circle.transform.lossyScale.y * 10)/9f;

		lineChild.transform.localPosition = new Vector2(((square.transform.lossyScale.x/2) * 10)/9f, 0);

		lineChild.transform.localScale = new Vector3 (localScaleX, 0.2f, 1);


		lastSquare = squareNum;
	}
}
