using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineScriptForLevels : MonoBehaviour {

	#region variables
	public int lastSquare = 0;

	public GameObject square;
	public GameObject square2;
	public GameObject lastOkSquare;

	public GameObject dragLine;
	public GameObject dragCircle;

	private Vector3 point1;
	private Vector3 point2;

	[Space]
	[Space]

	public int score;
	public Text scoreText;

	public int defaultMovesLeft;
	public int movesLeft;
	public Text movesLeftText;

	[Space]
	[Space]

	public bool updateInitialize = true;
	public bool newSquareInitialize = true;
	public bool isOnMobile = true;

	public GameObject circlePrefab;
	GameObject circle;
	public GameObject linePrefab;
	GameObject line;

	public Game Squares;

	public GameObject squares;

	private GameObject lineChild;

	private float localScaleX;
	private float localScaleY;

	public Color[] setColor;

	public int currentSquare;
	public int currentColor;

	public SpriteRenderer dragLineSpr;
	public SpriteRenderer dragCircleSpr;

	public List<int> squareList = new List<int>();
	public List<int> colorList = new List<int>();


	public AudioClip[] hits;
	public AudioClip snap;

	public AudioClip snap2;

	private AudioSource audioSource;

	public int animationNumber = 0;

	public bool controlsEnabled = true;
	private bool controlSwitch;

	public float size;

	public int lastSquareNum;

	private int squareRows;

	private bool randomizeColors = false;

	#endregion

	void Start() {
		audioSource = GetComponent<AudioSource> ();

		animationNumber = 0;

		controlsEnabled = true;

		score = 0;
		scoreText.text = "" + score;

		movesLeft = defaultMovesLeft;	
		movesLeftText.text = "" + movesLeft;

		randomizeColors = false;

		squareRows = Squares.Rows;	

		foreach (Transform square in squares.transform)
		{
			square.gameObject.SetActive (true);
		}

		colorList.Clear ();
	}

	void Update () {
		#if UNITY_EDITOR
		isOnMobile = false;

		if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) && controlsEnabled) {
			if (lastSquare != 0) {
				if (updateInitialize == true) {
					square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

					dragLine.SetActive (true);
					dragCircle.SetActive (true);

					dragCircle.transform.localScale = new Vector3 (square2.transform.lossyScale.x*size*10, square2.transform.lossyScale.x*size*10, 1);

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
				dragLine.transform.localScale = new Vector3(direction.magnitude*1.25f,dragCircle.transform.lossyScale.x/50f,1);
			}
		} 

		else if (Input.GetMouseButtonUp(0)) {
			if (updateInitialize == false) {
				dragLine.SetActive (false);
				dragCircle.SetActive (false);

				if (squareList.Count == 0) {
					if (currentColor == 0) {
						currentColor = setColor.Length - 1;
					}
					else {
						currentColor = currentColor - 1;
					}
				}
				else 
				{
					movesLeft = movesLeft - 1;
					movesLeftText.text = "" + movesLeft;
				}

				RemoveLine ();
				SwitchColor ();
				updateInitialize = true;
			}
		}

		#endif

		if (isOnMobile) {
			if (Input.touchCount > 0 && controlsEnabled) {
				if (lastSquare != 0) {
					if (updateInitialize == true) {
						square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

						dragLine.SetActive (true);
						dragCircle.SetActive (true);

						dragCircle.transform.localScale = new Vector3 (square2.transform.lossyScale.x*size*10, square2.transform.lossyScale.x*size*10, 1);

						updateInitialize = false;
					}

					if (newSquareInitialize == true) {
						dragCircleSpr.color = setColor [currentColor];
						dragLineSpr.color = setColor [currentColor];

						lastOkSquare = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

						newSquareInitialize = false;
					}

					point1 = lastOkSquare.transform.position;
					point2 = Camera.main.ScreenToWorldPoint (Input.touches [0].position);
					point2 [2] = 0;

					dragCircle.transform.position = point2; 
					dragLine.transform.position = point1;

					float rot_z = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
					dragLine.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

					dragLine.transform.position = point1;
					Vector2 direction = point2 - point1;
					dragLine.transform.localScale = new Vector3(direction.magnitude*1.25f,dragCircle.transform.lossyScale.x/50f,1);
				}

			}
			else if (Input.touchCount == 0) {
				if (updateInitialize == false) {
					dragLine.SetActive (false);
					dragCircle.SetActive (false);

					if (squareList.Count == 0) {
						if (currentColor == 0) {
							currentColor = setColor.Length - 1;
						}
						else {
							currentColor = currentColor - 1;
						}
					}

					movesLeft = movesLeft - 1;
					movesLeftText.text = "" + movesLeft;

					RemoveLine ();
					SwitchColor ();
					updateInitialize = true;
				}
			}
		}
	}

	public void AddSquare(int squareNum, int colorNum, bool hoverSwitch)
	{ 
		if (controlsEnabled) {
			#region same square
			if (lastSquare == 0) {
				square = GameObject.Find ("Game/Squares/" + squareNum.ToString ());
				SquareScript squareScript = square.GetComponent<SquareScript> ();

				circle = Instantiate (circlePrefab, transform) as GameObject;
				circle.transform.position = square.transform.position;
				circle.name = "circle " + squareNum;
				circle.transform.localScale = new Vector3 (square.transform.lossyScale.x * size*10, square.transform.lossyScale.x * size*10, 1);

				if (colorNum == setColor.Length - 1) {
					currentColor = 0;
				} else {
					currentColor = colorNum + 1;
				}

				circle.GetComponent<SpriteRenderer> ().color = setColor [currentColor];

				squareScript.hoverSwitch = !squareScript.hoverSwitch;

				audioSource.PlayOneShot (hits [0]);

				lastSquare = squareNum;
				squareList.Add (squareNum);

				newSquareInitialize = true;
			} 
			#endregion

			#region next squares
			else if (lastSquare != squareNum && hoverSwitch == false) {	
				square = GameObject.Find ("Game/Squares/" + squareNum.ToString ());
				SquareScript squareScript = square.GetComponent<SquareScript> ();

				lastSquareNum = squareNum;

				if (lastSquare > 0) {
					square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());
				}

				if (square2.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color && !squareList.Contains (squareNum)) {
					#region squares close

					#region up
					if (squareNum == lastSquare - squareRows) {
						DrawLine (square, square2, squareNum, 90);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
						} else {
							audioSource.PlayOneShot (hits [hits.Length-1]);
						}	
					} 
					#endregion

					#region left
					else if (squareNum == lastSquare - 1 && (squareNum % squareRows != 0)) {
						DrawLine (square, square2, squareNum, 180);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
						} else {
							audioSource.PlayOneShot (hits [hits.Length-1]);
						}		
					}
					#endregion

					#region right
					else if (squareNum == lastSquare + 1 && ((squareNum - 1) % squareRows != 0)) {
						DrawLine (square, square2, squareNum, 0);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
						} else {
							audioSource.PlayOneShot (hits [hits.Length-1]);
						}	
					} 
					#endregion

					#region down
					else if (squareNum == lastSquare + squareRows) {
						DrawLine (square, square2, squareNum, 270);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
						} else {
							audioSource.PlayOneShot (hits [hits.Length-1]);
						}	
					}
					#endregion

					#endregion

					#region squares far

					#region up and down
					else if (squareNum % squareRows == lastSquare % squareRows) {

						#region up
						if (squareNum < lastSquare) {
							bool draw = true;
							for (int i = 0; i < ((Mathf.Abs (squareNum - lastSquare)) / squareRows); i++) {
								if (draw) {
									int tempSquareNum = lastSquare - ((i + 1) * squareRows);
									GameObject tempSquare = GameObject.Find ("Game/Squares/" + (tempSquareNum).ToString ());
									if ((tempSquare.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color) && !squareList.Contains (tempSquareNum)) {
										draw = true;
									} else {
										draw = false;
									}
								}
							}

							if (draw) {
								int loopLength = (Mathf.Abs (squareNum - lastSquare)) / squareRows;
								int currentLastSquare = lastSquare;

								for (int i = 0; i < loopLength; i++) {
									int squareCloseNum = currentLastSquare - ((i) * squareRows);
									int squareFarNum = currentLastSquare - ((i + 1) * squareRows);

									GameObject SquareClose = GameObject.Find ("Game/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 90);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;

									lastSquare = squareFarNum;
								}

								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
								} else {
									audioSource.PlayOneShot (hits [hits.Length-1]);
								}
							}				
						}
						#endregion

						#region down
						else if (squareNum > lastSquare) {
							bool draw = true;
							for (int i = 0; i < ((Mathf.Abs (lastSquare - squareNum)) / squareRows); i++) {
								if (draw) {
									int tempSquareNum = lastSquare + ((i + 1) * squareRows);
									GameObject tempSquare = GameObject.Find ("Game/Squares/" + (tempSquareNum).ToString ());
									if ((tempSquare.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color) && !squareList.Contains (tempSquareNum)) {
										draw = true;
									} else {
										draw = false;
									}
								}
							}

							if (draw) {
								int loopLength = (Mathf.Abs (lastSquare - squareNum)) / squareRows;
								int currentLastSquare = lastSquare;

								for (int i = 0; i < loopLength; i++) {
									int squareCloseNum = currentLastSquare + ((i) * squareRows);
									int squareFarNum = currentLastSquare + ((i + 1) * squareRows);

									GameObject SquareClose = GameObject.Find ("Game/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 270);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;
									lastSquare = squareFarNum;
								}

								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
								} else {
									audioSource.PlayOneShot (hits [hits.Length-1]);
								}
							}		
						}
						#endregion

					}
					#endregion

					#region left and right
					else if (Mathf.Ceil ((squareNum - 0.001F) / squareRows) == Mathf.Ceil ((lastSquare - 0.001F) / squareRows)) {

						#region left
						if (squareNum < lastSquare) {
							bool draw = true;
							for (int i = 0; i < (Mathf.Abs (squareNum - lastSquare)); i++) {
								if (draw) {
									int tempSquareNum = lastSquare - (i + 1);
									GameObject tempSquare = GameObject.Find ("Game/Squares/" + (tempSquareNum).ToString ());
									if ((tempSquare.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color) && !squareList.Contains (tempSquareNum)) {
										draw = true;
									} else {
										draw = false;
									}
								}
							}

							if (draw) {							
								int loopLength = Mathf.Abs (squareNum - lastSquare);
								int currentLastSquare = lastSquare;

								for (int i = 0; i < loopLength; i++) {
									int squareCloseNum = currentLastSquare - i;
									int squareFarNum = currentLastSquare - (i + 1);

									GameObject SquareClose = GameObject.Find ("Game/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 180);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;

									lastSquare = squareFarNum;
								}

								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
								} else {
									audioSource.PlayOneShot (hits [hits.Length-1]);
								}
							}				
						}
						#endregion

						#region right
						else if (squareNum > lastSquare) {
							bool draw = true;
							for (int i = 0; i < (Mathf.Abs (lastSquare - squareNum)); i++) {
								if (draw) {
									int tempSquareNum = lastSquare + (i + 1);
									GameObject tempSquare = GameObject.Find ("Game/Squares/" + (tempSquareNum).ToString ());
									if ((tempSquare.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color) && !squareList.Contains (tempSquareNum)) {
										draw = true;
									} else {
										draw = false;
									}
								}
							}

							if (draw) {
								int loopLength = Mathf.Abs (lastSquare - squareNum);
								int currentLastSquare = lastSquare;

								for (int i = 0; i < loopLength; i++) {
									int squareCloseNum = currentLastSquare + (i);
									int squareFarNum = currentLastSquare + (i + 1);

									GameObject SquareClose = GameObject.Find ("Game/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 0);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;

									lastSquare = squareFarNum;
								}

								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
								} else {
									audioSource.PlayOneShot (hits [hits.Length-1]);
								}
							}		
						}
						#endregion

					}
					#endregion

					#endregion
				}

			}
			#endregion

			#region regret
			else if ((lastSquare == squareNum) && hoverSwitch) {
				square = GameObject.Find ("Game/Squares/" + squareNum.ToString ());
				square2 = GameObject.Find ("Game/Squares/" + squareList [squareList.Count - 1].ToString ());

				SquareScript squareScript = square.GetComponent<SquareScript> ();

				GameObject circle = GameObject.Find ("circle " + lastSquare.ToString ());
				GameObject line = GameObject.Find ("line " + lastSquare.ToString ());

				GameObject.Destroy (circle);
				GameObject.Destroy (line);

				if (squareList.Count < hits.Length) {
					audioSource.PlayOneShot (hits [squareList.Count - 1]);	
				} else {
					audioSource.PlayOneShot (hits [hits.Length-1]);
				}

				if (squareList.Count > 1) {
					squareList.RemoveAt (squareList.Count - 1);
					lastSquare = squareList [squareList.Count - 1];

				} else if (squareList.Count == 1) {
					squareList.RemoveAt (0);
					lastSquare = -1;

					dragLine.SetActive (false);
					dragCircle.SetActive (false);

					controlsEnabled = false;
				}

				squareScript.hoverSwitch = !squareScript.hoverSwitch;

				newSquareInitialize = true;
			} 

			else if (squareList.Count > 1 && hoverSwitch) 
			{
				if ((squareNum == squareList [squareList.Count - 2])) {
					GameObject square = GameObject.Find ("Game/Squares/" + squareList [squareList.Count - 1].ToString ());
					square2 = GameObject.Find ("Game/Squares/" + squareList [squareList.Count - 1].ToString ());

					SquareScript squareScript = square.GetComponent<SquareScript> ();

					GameObject circle = GameObject.Find ("circle " + lastSquare.ToString ());
					GameObject line = GameObject.Find ("line " + lastSquare.ToString ());

					GameObject.Destroy (circle);
					GameObject.Destroy (line);

					if (squareList.Count > 1) {
						squareList.RemoveAt (squareList.Count - 1);
						lastSquare = squareList [squareList.Count - 1];
					} 

					if (squareList.Count < hits.Length) {
						audioSource.PlayOneShot (hits [squareList.Count - 1]);	
					} else {
						audioSource.PlayOneShot (hits [hits.Length-1]);
					}

					if (squareList.Count == 1) {
						GameObject finalCircle = GameObject.Find ("circle " + squareList [0].ToString ());
						GameObject.Destroy (finalCircle);

						squareList.RemoveAt (0);
						lastSquare = -1;

						dragLine.SetActive (false);
						dragCircle.SetActive (false);

						controlsEnabled = false;
					}

					squareScript.hoverSwitch = !squareScript.hoverSwitch;

					newSquareInitialize = true;
				}
			}
			#endregion
		}
	}

	void DrawLine(GameObject square, GameObject square2, int squareNum, float rotation) 
	{
		circle = Instantiate (circlePrefab, transform) as GameObject;
		circle.transform.position = square.transform.position;
		circle.name = "circle " + squareNum;
		circle.transform.localScale = new Vector3 (square.transform.lossyScale.x*size*10, square.transform.lossyScale.x*size*10, 1);
		circle.GetComponent<SpriteRenderer> ().color = setColor [currentColor];

		line = Instantiate (linePrefab, transform) as GameObject;
		line.transform.position = square2.transform.position;
		line.name = "line " + squareNum;
		line.transform.localEulerAngles = new Vector3 (0, 0, rotation);

		foreach (Transform child in line.transform)
		{
			if (child.name == "Square") {
				lineChild = child.gameObject;
			}
		}	

		localScaleX = (square.transform.lossyScale.x * 10)/9f;
		localScaleY = (circle.transform.lossyScale.x/50f);

		lineChild.transform.localPosition = new Vector2(((square.transform.lossyScale.x/2) * 10)/9f, 0);
		lineChild.transform.localScale = new Vector3 (localScaleX, localScaleY, 1);
		lineChild.GetComponent<SpriteRenderer> ().color = setColor [currentColor];

		lastSquare = squareNum;

		if (!squareList.Contains (squareNum)) {
			squareList.Add (squareNum);
		}

		newSquareInitialize = true;
	}

	void RemoveLine() {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void SwitchColor() {
		if (animationNumber < squareList.Count) 
		{
			if (randomizeColors)
			{
				currentColor = UnityEngine.Random.Range (0, setColor.Length);
			}

			GameObject squareToChange = GameObject.Find ("Game/Squares/" + squareList [animationNumber].ToString ());
			squareToChange.GetComponent<Animator> ().SetTrigger ("Trigger");

			squareToChange.GetComponent<SquareScript> ().colorNum = currentColor;

			colorList.RemoveAt (squareList [animationNumber] - 1);
			colorList.Insert (squareList [animationNumber] - 1, currentColor);

			animationNumber = animationNumber + 1;

			if (!randomizeColors)
			{
				score = score + 1;
				scoreText.text = "" + score;
			}

			if (animationNumber == squareList.Count) {

			}

			controlsEnabled = false;
		} 

		else {
			animationNumber = 0;
			lastSquare = 0;
			squareList.Clear ();

			checkMoves ();

			controlsEnabled = true;
			controlSwitch = true;
		}
	}

	public void EnableControllers() {
		if (controlSwitch) {
			randomizeColors = false;
			checkIfWon ();

			controlsEnabled = true;
			controlSwitch = false;
		}
	}

	public void PlaySound() {
		if (randomizeColors)
		{
			audioSource.PlayOneShot (snap2);
		}
		else
		{
			audioSource.PlayOneShot (snap);
		}
	}

	public void AddToColorList(int squareNum, int colorNum) 
	{
		colorList.Add (colorNum);
	}

	void checkIfWon () {
		bool shouldContinue = true;
		for (int i = 0; i < colorList.Count-1; i++) {
			if (shouldContinue) {
				if (colorList [i] != colorList [i + 1])
				{
					shouldContinue = false;
				}

				if (colorList.Count-2 == i)
				{
					ShuffleBoard();
				}
			}
		}
	}

	void checkMoves () {
		if (movesLeft <= 0)
		{
			Debug.Log ("You Lost!!!");
		}
	}

	public void ShuffleBoard () {
		squareList.Clear ();

		for (int i = 0; i < squareRows; i++)
		{
			if (i % 2 == 0)
			{
				for (int j = 0; j < squareRows; j++) 
				{
					squareList.Add (j + 1 + (i*squareRows));
				}
			}

			else
			{
				for (int k = squareRows; k > 0; k--) 
				{
					squareList.Add (k + (i*squareRows));
				}
			}
		}

		randomizeColors = true;

		SwitchColor ();
	}
}