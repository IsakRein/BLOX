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

	void Update () {
		if (Input.touchCount > 0) {
			
		}
	}

	public void AddSquare(int squareNum) {
		GameObject square;
		GameObject square2;

		square = GameObject.Find ("Game/Squares/" + squareNum.ToString());
		square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString());

		if (lastSquare == 0) {
			circle = Instantiate (circlePrefab, transform) as GameObject;
			circle.transform.position = square.transform.position;
			lastSquare = squareNum;
		}

		else if (squareNum == lastSquare - Squares.Rows)
		{
			circle = Instantiate (circlePrefab, transform) as GameObject;
			circle.transform.position = square.transform.position;

			line = Instantiate (linePrefab, transform) as GameObject;
			line.transform.position = square2.transform.position;
			line.transform.localEulerAngles = new Vector3 (0, 0, 90);

			lastSquare = squareNum;


		}

		else if (squareNum == lastSquare - 1)
		{
			circle = Instantiate (circlePrefab, transform) as GameObject;
			circle.transform.position = square.transform.position;

			line = Instantiate (linePrefab, transform) as GameObject;
			line.transform.position = square2.transform.position;
			line.transform.localEulerAngles = new Vector3 (0, 0, 180);

			lastSquare = squareNum;
		}

		else if (squareNum == lastSquare + 1)
		{
			circle = Instantiate (circlePrefab, transform) as GameObject;
			circle.transform.position = square.transform.position;

			line = Instantiate (linePrefab, transform) as GameObject;
			line.transform.position = square2.transform.position;
			line.transform.localEulerAngles = new Vector3 (0, 0, 0);

			lastSquare = squareNum;
		}

		else if (squareNum == lastSquare + Squares.Rows)
		{
			circle = Instantiate (circlePrefab, transform) as GameObject;
			circle.transform.position = square.transform.position;

			line = Instantiate (linePrefab, transform) as GameObject;
			line.transform.position = square2.transform.position;
			line.transform.localEulerAngles = new Vector3 (0, 0, 270);

			foreach (Transform child in line.transform)
			{
				if (child.name == "Square") {
					lineChild = child.gameObject;
				}
			}	

			localScaleX = square.transform.lossyScale.x;
			localScaleY = circle.transform.lossyScale.y;

			lineChild.transform.localPosition = new Vector2(square.transform.lossyScale.x/2, 0);

			lineChild.transform.localScale = new Vector3 (localScaleX, 0.2f, 1);


			lastSquare = squareNum;
		}
	}
		
}
