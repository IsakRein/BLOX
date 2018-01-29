using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public float totalWidth;

	public GameObject squarePrefab;
	GameObject square;

	private float xPos;
	private float yPos;

	private float scale;

	public Vector3 touchPos;

	public int Rows;

	public Transform circles;
	public GameObject circlePrefab;
    GameObject circle;

	void Awake () {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}

		GenerateGrid ();

        Manager.loadColors = true;
	}

	public void GenerateGrid() {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}

		scale = totalWidth/Rows;

		for (int i = 1; i <= Rows; i++) {
			yPos = -i + ((Rows + 1f) / 2); 

			for (int j = 1; j <= Rows; j++) {
				square = Instantiate (squarePrefab, transform) as GameObject; 

				xPos = j - ((Rows + 1f) / 2); 
				square.transform.localPosition = new Vector2 (xPos, yPos);

                SquareScript squareScript = square.GetComponent<SquareScript>();
                squareScript.loadColors = Manager.loadColors;
                squareScript.number = (i-1)*Rows + j;

                square.GetComponent<Animator>().SetTrigger("OnEnable");

				transform.localScale = new Vector3 (scale, scale);
            }
		}

		GenerateCircles ();
	}

	public void GenerateCircles() {
		for (int i = 1; i <= Rows; i++) {
			circle = Instantiate (circlePrefab, circles) as GameObject; 

			xPos = i - ((Rows + 1f) / 2);
			yPos = (Rows + 1f) / 2;

            circle.name = "" + i;

            NextSquareScript nextSquareScript = circle.GetComponentInChildren<NextSquareScript>();

            nextSquareScript.num = i;

            nextSquareScript.loadColors = Manager.loadColors;

			circle.transform.localPosition = new Vector2 (xPos, yPos);

			circle.GetComponentInChildren<Animator> ().SetTrigger ("Entry");

			circles.localScale = new Vector3 (scale, scale);
		}
	}
}
