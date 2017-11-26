using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LineScript : MonoBehaviour {

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
	public List<int> rowList = new List<int> ();

	public AudioClip[] hits;
	public AudioClip snap;
	public AudioClip snap2;
	private AudioSource audioSource;

	public bool controlsEnabled = true;
	private bool randomizeColors = false;
	public bool fallDown;
	private bool InitializeFallHasBeenCalled = false;

	public float size;
	private int fallenSquareCounter;
	private int initializeCounter;
	public int lastSquareNum;
	public int squareRows;

	public GameObject squarePrefab;
	private	GameObject instSquare;

	#endregion

	void Start() {
		audioSource = GetComponent<AudioSource> ();

		controlsEnabled = true;

		score = 0;
		scoreText.text = "" + score;

		randomizeColors = false;

		squareRows = Squares.Rows;	

		foreach (Transform square in squares.transform)
		{
			square.gameObject.SetActive (true);
		}

		for (int i = 0; i < squareRows; i++) {
			rowList.Add (0);
		}

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

				controlsEnabled = true;

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
					StartAnimation();
				}
					
				RemoveLine ();
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

					controlsEnabled = true;

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
						StartAnimation();
					}
						
					RemoveLine ();
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
					lastSquare = 0;

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
						lastSquare = 0;

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

	public void StartAnimation() {
		controlsEnabled = false;

		foreach (int square in squareList)
		{
			GameObject squareObj = GameObject.Find (square.ToString());
			squareObj.SendMessage("Animate", SendMessageOptions.DontRequireReceiver);
		}

		score = score + squareList.Count;
		scoreText.text = "" + score;

		foreach (int square in squareList) {
			int currentSquareRow = 0;

			if (square % squareRows == 0) {
				currentSquareRow = squareRows;
			} else {
				currentSquareRow = square % squareRows;
			}

			int value = rowList [currentSquareRow - 1] + 1;

			rowList.RemoveAt (currentSquareRow - 1);
			rowList.Insert (currentSquareRow - 1, value);
		}

		fallDown = true;
	}

	public void InitializeFall() {
		initializeCounter = initializeCounter + 1;

		Debug.Log (initializeCounter);
		Debug.Log (squareList.Count);

		if (InitializeFallHasBeenCalled == false && initializeCounter == squareList.Count)
		{
			InitializeFallHasBeenCalled = true;
			initializeCounter = 0;

			for (int row = 0; row < squareRows; row++)
			{
				if (rowList [row] != 0)
				{
					float xPos = -((float)squareRows / 2) + row + 0.5f;

					for (int i = 0; i < rowList [row]; i++)
					{
						instSquare = Instantiate (squarePrefab, squares.transform) as GameObject; 

						float yPos = i + (((float)squareRows / 2) + 0.5f); 
						instSquare.transform.localPosition = new Vector2 (xPos, yPos);
						instSquare.SendMessage ("AddToFallCounter", rowList [row], SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			foreach (Transform child in squares.transform)
			{
				child.SendMessage ("InitializeFall", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void AddToFallCounter() {
		fallenSquareCounter = fallenSquareCounter + 1;

		if (fallenSquareCounter == squareRows*squareRows)
		{
			FallingDone ();
			fallenSquareCounter = 0;
		}
	}

	public void FallingDone() {
		controlsEnabled = true;
		lastSquare = 0;
		squareList.Clear ();
		fallDown = false;
		InitializeFallHasBeenCalled = false;

		int m = rowList.Max();
		int n = rowList.IndexOf (m)+1;

		foreach (Transform square in squares.transform)
		{
			square.SendMessage ("NameSquare", SendMessageOptions.DontRequireReceiver);
		}

		rowList.Clear ();
		for (int i = 0; i < squareRows; i++) {
			rowList.Add (0);
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

	/*public void SwitchColor() {
		if (animationNumber < squareList.Count) 
		{
			if (randomizeColors)
			{
				currentColor = UnityEngine.Random.Range (0, setColor.Length);
			}

			GameObject squareToChange = GameObject.Find ("Game/Squares/" + squareList [animationNumber].ToString ());
			squareToChange.GetComponent<Animator> ().SetTrigger ("Trigger");

			squareToChange.GetComponent<SquareScript> ().colorNum = currentColor;

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

			fallDown = true;

			squareList.Clear ();

			controlsEnabled = true;
			controlSwitch = true;
		}
	}*/

	/*void checkBelow(int i, int square) {
		if (!(Mathf.Ceil((i + squareRows - 0.001F)/squareRows) == squareRows)) {
			if (squareList.Contains (i + squareRows))
			{
				checkBelow (i + squareRows, square);
			}
			else if (i != square)
			{
				GameObject.Find ("Game/Squares/" + square.ToString ()).SendMessage ("FallDown", ((i-square)/squareRows), SendMessageOptions.DontRequireReceiver);;
			}
		}

	}*/

	/*public void ShuffleBoard () {
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
	}*/
}