using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LineScript : MonoBehaviour
{

	#region variables

	/// <summary>
	/// The last square.
	/// </summary>
	public int lastSquare = 0;

	/// <summary>
	/// The square.
	/// </summary>
	public GameObject square;
	/// <summary>
	/// The square2.
	/// </summary>
	public GameObject square2;
	/// <summary>
	/// The last ok square.
	/// </summary>
	public GameObject lastOkSquare;

	/// <summary>
	/// The drag line.
	/// </summary>
	public GameObject dragLine;
	/// <summary>
	/// The drag circle.
	/// </summary>
	public GameObject dragCircle;

	private Vector3 point1;
	private Vector3 point2;

	[Space]
	[Space]

	private int score;
	/// <summary>
	/// The score text.
	/// </summary>
	public Text scoreText;

	[Space]
	[Space]

	private bool updateInitialize = true;
	private bool newSquareInitialize = true;
	private bool isOnMobile = true;

	/// <summary>
	/// The circle prefab.
	/// </summary>
	public GameObject circlePrefab;
	private GameObject circle;
	/// <summary>
	/// The line prefab.
	/// </summary>
	public GameObject linePrefab;
	private GameObject line;

	/// <summary>
	/// The squares.
	/// </summary>
	public Game Squares;
	/// <summary>
	/// The squares.
	/// </summary>
	public GameObject squares;
	private GameObject lineChild;

	private float localScaleX;
	private float localScaleY;

	/// <summary>
	/// The color of the set.
	/// </summary>
	public Color[] setColor;

	private int currentSquare;
	private int currentColor;

	/// <summary>
	/// The drag line spr.
	/// </summary>
	public SpriteRenderer dragLineSpr;
	/// <summary>
	/// The drag circle spr.
	/// </summary>
	public SpriteRenderer dragCircleSpr;

	/// <summary>
	/// The square list.
	/// </summary>
	public List<int> squareList = new List<int> ();
	/// <summary>
	/// The row list.
	/// </summary>
	public List<int> rowList = new List<int> ();
	public List<int> colorList = new List<int> ();

	/// <summary>
	/// The hits.
	/// </summary>
	public AudioClip[] hits;
	/// <summary>
	/// The snap.
	/// </summary>
	public AudioClip snap;
	/// <summary>
	/// The snap2.
	/// </summary>
	public AudioClip snap2;
	private AudioSource audioSource;

	private bool controlsEnabled = true;
	private bool randomizeColors = false;
	/// <summary>
	/// The fall down.
	/// </summary>
	public bool fallDown;
	private bool InitializeFallHasBeenCalled = false;

	/// <summary>
	/// The size.
	/// </summary>
	public float size;
	/// <summary>
	/// The minimum squares in move.
	/// </summary>
	public int minimumSquaresInMove;
	private int fallenSquareCounter;
	private int initializeCounter;
	private int lastSquareNum;
	private int squareRows;

	/// <summary>
	/// The square prefab.
	/// </summary>
	public GameObject squarePrefab;
	private	GameObject instSquare;

	#endregion

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();

		controlsEnabled = true;

		score = 0;
		scoreText.text = "" + score;

		randomizeColors = false;

		squareRows = Squares.Rows;	

		foreach (Transform square in squares.transform) {
			square.gameObject.SetActive (true);
		}

		for (int i = 0; i < squareRows; i++) {
			rowList.Add (0);
		}
	}

	void Update ()
	{
		#if UNITY_EDITOR
		isOnMobile = false;

		if ((Input.GetMouseButton (0) || Input.GetMouseButtonDown (0)) && controlsEnabled) {
			if (lastSquare != 0) {
				if (updateInitialize == true) {
					square2 = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

					dragLine.SetActive (true);
					dragCircle.SetActive (true);

					dragCircle.transform.localScale = new Vector3 (square2.transform.lossyScale.x * size * 10, square2.transform.lossyScale.x * size * 10, 1);

					updateInitialize = false;
				}

				if (newSquareInitialize == true) {
					dragCircleSpr.color = setColor [currentColor];
					dragLineSpr.color = setColor [currentColor];

					lastOkSquare = GameObject.Find ("Game/Squares/" + lastSquare.ToString ());

					newSquareInitialize = false;
				}
					
				point1 = lastOkSquare.transform.position;
				point2 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				point2 [2] = 0;

				dragCircle.transform.position = point2; 
				dragLine.transform.position = point1;

				float rot_z = Mathf.Atan2 (point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
				dragLine.transform.rotation = Quaternion.Euler (0f, 0f, rot_z);

				dragLine.transform.position = point1;
				Vector2 direction = point2 - point1;
				dragLine.transform.localScale = new Vector3 (direction.magnitude * 1.25f, dragCircle.transform.lossyScale.x / 50f, 1);
			}
		} else if (Input.GetMouseButtonUp (0)) {
			if (updateInitialize == false) {
				dragLine.SetActive (false);
				dragCircle.SetActive (false);

				controlsEnabled = true;

				if (minimumSquaresInMove <= squareList.Count) {
					if (squareList.Count == 0) {
						if (currentColor == 0) {
							currentColor = setColor.Length - 1;
						} else {
							currentColor = currentColor - 1;
						}
					} else {
						StartAnimation ();
					}		
				} else {
					FallingDone ();
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

						dragCircle.transform.localScale = new Vector3 (square2.transform.lossyScale.x * size * 10, square2.transform.lossyScale.x * size * 10, 1);

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

					float rot_z = Mathf.Atan2 (point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
					dragLine.transform.rotation = Quaternion.Euler (0f, 0f, rot_z);

					dragLine.transform.position = point1;
					Vector2 direction = point2 - point1;
					dragLine.transform.localScale = new Vector3 (direction.magnitude * 1.25f, dragCircle.transform.lossyScale.x / 50f, 1);
				}

			} else if (Input.touchCount == 0) {
				if (updateInitialize == false) {
					dragLine.SetActive (false);
					dragCircle.SetActive (false);

					controlsEnabled = true;

					if (minimumSquaresInMove <= squareList.Count) {
						if (squareList.Count == 0) {
							if (currentColor == 0) {
								currentColor = setColor.Length - 1;
							} else {
								currentColor = currentColor - 1;
							}
						} else {
							StartAnimation ();
						}
					} else {
						FallingDone ();
					}

					RemoveLine ();
					updateInitialize = true;
				}
			}
		}
	}

	/// <summary>
	/// Adds the square.
	/// </summary>
	/// <param name="squareNum">Square number.</param>
	/// <param name="colorNum">Color number.</param>
	/// <param name="hoverSwitch">If set to <c>true</c> hover switch.</param>
	public void AddSquare (int squareNum, int colorNum, bool hoverSwitch)
	{ 
		if (controlsEnabled) {
			#region same square
			if (lastSquare == 0) {
				square = GameObject.Find ("Game/Squares/" + squareNum.ToString ());
				SquareScript squareScript = square.GetComponent<SquareScript> ();

				circle = Instantiate (circlePrefab, transform) as GameObject;
				circle.transform.position = square.transform.position;
				circle.name = "circle " + squareNum;
				circle.transform.localScale = new Vector3 (square.transform.lossyScale.x * size * 10, square.transform.lossyScale.x * size * 10, 1);

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
							audioSource.PlayOneShot (hits [hits.Length - 1]);
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
							audioSource.PlayOneShot (hits [hits.Length - 1]);
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
							audioSource.PlayOneShot (hits [hits.Length - 1]);
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
							audioSource.PlayOneShot (hits [hits.Length - 1]);
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
									audioSource.PlayOneShot (hits [hits.Length - 1]);
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
									audioSource.PlayOneShot (hits [hits.Length - 1]);
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
									audioSource.PlayOneShot (hits [hits.Length - 1]);
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
									audioSource.PlayOneShot (hits [hits.Length - 1]);
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
					audioSource.PlayOneShot (hits [hits.Length - 1]);
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
			} else if (squareList.Count > 1 && hoverSwitch) {
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
						audioSource.PlayOneShot (hits [hits.Length - 1]);
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

	void DrawLine (GameObject square, GameObject square2, int squareNum, float rotation)
	{
		circle = Instantiate (circlePrefab, transform) as GameObject;
		circle.transform.position = square.transform.position;
		circle.name = "circle " + squareNum;
		circle.transform.localScale = new Vector3 (square.transform.lossyScale.x * size * 10, square.transform.lossyScale.x * size * 10, 1);
		circle.GetComponent<SpriteRenderer> ().color = setColor [currentColor];

		line = Instantiate (linePrefab, transform) as GameObject;
		line.transform.position = square2.transform.position;
		line.name = "line " + squareNum;
		line.transform.localEulerAngles = new Vector3 (0, 0, rotation);

		foreach (Transform child in line.transform) {
			if (child.name == "Square") {
				lineChild = child.gameObject;
			}
		}	

		localScaleX = (square.transform.lossyScale.x * 10) / 9f;
		localScaleY = (circle.transform.lossyScale.x / 50f);

		lineChild.transform.localPosition = new Vector2 (((square.transform.lossyScale.x / 2) * 10) / 9f, 0);
		lineChild.transform.localScale = new Vector3 (localScaleX, localScaleY, 1);
		lineChild.GetComponent<SpriteRenderer> ().color = setColor [currentColor];

		lastSquare = squareNum;

		if (!squareList.Contains (squareNum)) {
			squareList.Add (squareNum);
		}

		newSquareInitialize = true;
	}

	void RemoveLine ()
	{
		foreach (Transform child in transform) {
			GameObject.Destroy (child.gameObject);
		}
	}

	void CheckAvailableMoves ()
	{
		bool noMoves = true;

		for (int i = 1; i <= squareRows * squareRows; i++) {
			Color clr = GameObject.Find ("Game/Squares/" + i.ToString ()).GetComponent<SpriteRenderer> ().color;

			if (i-squareRows > 0) {
				if (clr == GameObject.Find ("Game/Squares/" + (i - squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
					int newSquare = i - squareRows;

					if (noMoves) {
						if (newSquare - squareRows > 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 1) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare + squareRows <= squareRows * squareRows) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
					}
				}
			}

			if (i % squareRows != 1) {
				if (clr == GameObject.Find ("Game/Squares/" + (i - 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
					int newSquare = i - 1;

					if (noMoves) {
						if (newSquare - squareRows > 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 1) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare + squareRows <= squareRows * squareRows) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
					}
				} 
			}

			if (i % squareRows != 0) {
				if (clr == GameObject.Find ("Game/Squares/" + (i + 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
					int newSquare = i + 1;

					if (noMoves) {
						if (newSquare - squareRows > 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 1) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare + squareRows <= squareRows * squareRows) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
					}
				} 
			}

			if (i + squareRows <= squareRows * squareRows) {
				if (clr == GameObject.Find ("Game/Squares/" + (i + squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
					int newSquare = i + squareRows;

					if (noMoves) {
						if (newSquare - squareRows > 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 1) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare - 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare % squareRows != 0) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + 1).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
						if (newSquare + squareRows <= squareRows * squareRows) {
							if (clr == GameObject.Find ("Game/Squares/" + (newSquare + squareRows).ToString ()).GetComponent<SpriteRenderer> ().color) {
								noMoves = false;
							}
						}
					}
				}
			}
		}

		if (noMoves == false) {
			Debug.Log ("There are moves left");
		} else {
			Debug.Log ("No moves left");
		}
	}

	void AddToColorList () {
		
	}

	#region animation

	/// <summary>
	/// Starts the animation.
	/// </summary>
	public void StartAnimation ()
	{
		controlsEnabled = false;

		foreach (int square in squareList) {
			GameObject squareObj = GameObject.Find (square.ToString ());
			squareObj.SendMessage ("Animate", SendMessageOptions.DontRequireReceiver);
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

	/// <summary>
	/// Initializes the fall.
	/// </summary>
	public void InitializeFall ()
	{
		initializeCounter = initializeCounter + 1;

		if (InitializeFallHasBeenCalled == false && initializeCounter == squareList.Count) {
			InitializeFallHasBeenCalled = true;
			initializeCounter = 0;

			for (int row = 0; row < squareRows; row++) {
				if (rowList [row] != 0) {
					float xPos = -((float)squareRows / 2) + row + 0.5f;

					for (int i = 0; i < rowList [row]; i++) {
						instSquare = Instantiate (squarePrefab, squares.transform) as GameObject; 

						float yPos = i + (((float)squareRows / 2) + 0.5f); 
						instSquare.transform.localPosition = new Vector2 (xPos, yPos);
						instSquare.SendMessage ("AddToFallCounter", rowList [row], SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			foreach (Transform child in squares.transform) {
				child.SendMessage ("InitializeFall", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	/// <summary>
	/// Adds to fall counter.
	/// </summary>
	public void AddToFallCounter ()
	{
		fallenSquareCounter = fallenSquareCounter + 1;

		if (fallenSquareCounter == squareRows * squareRows) {
			FallingDone ();
			fallenSquareCounter = 0;
		}
	}

	/// <summary>
	/// Fallings the done.
	/// </summary>
	public void FallingDone ()
	{
		controlsEnabled = true;
		lastSquare = 0;
		squareList.Clear ();
		fallDown = false;
		InitializeFallHasBeenCalled = false;

		int m = rowList.Max ();
		int n = rowList.IndexOf (m) + 1;

		foreach (Transform square in squares.transform) {
			square.SendMessage ("NameSquare", SendMessageOptions.DontRequireReceiver);
		}

		rowList.Clear ();
		for (int i = 0; i < squareRows; i++) {
			rowList.Add (0);
		}

		CheckAvailableMoves ();
	}

	/// <summary>
	/// Playes the sound.
	/// </summary>
	public void PlaySound ()
	{
		if (randomizeColors) {
			audioSource.PlayOneShot (snap2);
		} else {
			audioSource.PlayOneShot (snap);
		}
	}

	#endregion
}