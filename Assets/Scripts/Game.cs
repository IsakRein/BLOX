using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public GameObject squarePrefab;
	GameObject square;

	private float xPos;
	private float yPos;

	private float scale;

	public Vector3 touchPos;

	public int Rows;

	void Awake() {
		Application.targetFrameRate = 60;
	}

	void Start () {
		GenerateGrid ();
	}

	public void GenerateGrid() {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}

		scale = 12*(Mathf.Pow((3.67f * Rows), -1.0f));

		for (int i = 1; i <= Rows; i++) {
			yPos = -i + ((Rows + 1f) / 2); 

			for (int j = 1; j <= Rows; j++) {
				square = Instantiate (squarePrefab, transform) as GameObject; 

				xPos = j - ((Rows + 1f) / 2); 
				square.transform.localPosition = new Vector2 (xPos, yPos);
				square.name = "" + (j + ((i - 1) * Rows));

				transform.localScale = new Vector3 (scale, scale);
			}
		}
	}
}
