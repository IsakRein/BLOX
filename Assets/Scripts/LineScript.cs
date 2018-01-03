﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class LineScript : MonoBehaviour
{

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
	public TextMeshProUGUI scoreText;

    public int highScore;
    public Text highScoreText;

    [Space]
    [Space]

    private bool updateInitialize = true;
    private bool newSquareInitialize = true;

    //0=editor; 1=ios; 2=android
    private int platformInt;

    public GameObject circlePrefab;
    private GameObject circle;
    public GameObject linePrefab;
    private GameObject line;

    public Game Squares;
    public GameObject squares;
    public Transform circles;
    private GameObject lineChild;

    private float localScaleX;
    private float localScaleY;

    public Color[] setColor;

    private int currentSquare;
    private int currentColor;

    public SpriteRenderer dragLineSpr;
    public SpriteRenderer dragCircleSpr;

    public List<int> squareList = new List<int>();
    public List<int> rowList = new List<int>();
    public List<int> colorList = new List<int>();

    public AudioClip[] hits;
    public AudioClip snap;
    public AudioClip snap2;

    private AudioSource audioSource;

    public bool controlsEnabled = true;
    private bool randomizeColors = false;
    public bool fallDown;
    private bool InitializeFallHasBeenCalled = false;

    public float size;
    public int minimumSquaresInMove;
    private int fallenSquareCounter;
    private int initializeCounter;
    private int squareRows;

    public GameObject squarePrefab;
    private GameObject instSquare;

    #endregion

    void Start()
    {

        #if UNITY_EDITOR
        platformInt = 0;
#endif
        #if UNITY_IOS && !UNITY_EDITOR
        platformInt = 1;

#endif
        #if UNITY_ANDROID && !UNITY_EDITOR
        platformInt = 2;
#endif

        Debug.Log(platformInt);

        audioSource = GetComponent<AudioSource> ();

		controlsEnabled = true;

		score = 0;
		scoreText.SetText("" + score);

		if (score >= highScore) {
			highScore = score;
			highScoreText.text = "<color=#B7A921ff>★</color>" + score;
		}

		randomizeColors = false;

		squareRows = Squares.Rows;	

		foreach (Transform square in squares.transform) {
			square.gameObject.SetActive (true);
		}

		colorList.Clear ();
		for(int i=0;i<=squareRows*squareRows;i++)
		{
			colorList.Add (0);
		}

		colorList [0] = -1;

		for (int i = 0; i < squareRows; i++) {
			rowList.Add (0);
		}
	}

	void Update ()
	{
        if (platformInt == 0) {
            if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) && controlsEnabled)
            {
                if (lastSquare != 0)
                {
                    if (updateInitialize == true)
                    {
                        square2 = GameObject.Find("Game/GameCanvas/BG1/BG2/Squares/" + lastSquare.ToString());

                        dragLine.SetActive(true);
                        dragCircle.SetActive(true);

                        dragCircle.transform.localScale = new Vector3(square2.transform.lossyScale.x * size * 10, square2.transform.lossyScale.x * size * 10, 1);

                        updateInitialize = false;
                    }

                    if (newSquareInitialize == true)
                    {
                        dragCircleSpr.color = setColor[currentColor];
                        dragLineSpr.color = setColor[currentColor];

                        lastOkSquare = GameObject.Find("Game/GameCanvas/BG1/BG2/Squares/" + lastSquare.ToString());

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
                    dragLine.transform.localScale = new Vector3(direction.magnitude * 1.25f, dragCircle.transform.lossyScale.x / 75f, 1);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (updateInitialize == false)
                {
                    dragLine.SetActive(false);
                    dragCircle.SetActive(false);

                    controlsEnabled = true;

                    if (minimumSquaresInMove <= squareList.Count)
                    {
                        StartAnimation();
                        VibSuccess();

                    }
                    else
                    {
                        foreach (int square in squareList)
                        {
                            GameObject squareObj = GameObject.Find("Game/GameCanvas/BG1/BG2/Squares/" + square.ToString());
                            squareObj.SendMessage("AnimateError", SendMessageOptions.DontRequireReceiver);
                        }
                        FallingDone();
                    }

                    RemoveLine();
                    updateInitialize = true;
                }
            }  
        }

        if (platformInt > 0) {
			if (Input.touchCount > 0 && controlsEnabled) {
				if (lastSquare != 0) {
					if (updateInitialize == true) {
						square2 = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + lastSquare.ToString ());

						dragLine.SetActive (true);
						dragCircle.SetActive (true);

						dragCircle.transform.localScale = new Vector3 (square2.transform.lossyScale.x * size * 10, square2.transform.lossyScale.x * size * 10, 1);

						updateInitialize = false;
					}

					if (newSquareInitialize == true) {
						dragCircleSpr.color = setColor [currentColor];
						dragLineSpr.color = setColor [currentColor];

						lastOkSquare = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + lastSquare.ToString ());

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
					dragLine.transform.localScale = new Vector3 (direction.magnitude * 1.25f, dragCircle.transform.lossyScale.x / 75f, 1);
				}

			} else if (Input.touchCount == 0) {
                if (updateInitialize == false)
                {
                    dragLine.SetActive(false);
                    dragCircle.SetActive(false);

                    controlsEnabled = true;

                    if (minimumSquaresInMove <= squareList.Count)
                    {
                        StartAnimation();
                        VibSuccess();

                    }
                    else
                    {
                        foreach (int square in squareList)
                        {
                            GameObject squareObj = GameObject.Find("Game/GameCanvas/BG1/BG2/Squares/" + square.ToString());
                            squareObj.SendMessage("AnimateError", SendMessageOptions.DontRequireReceiver);
                        }

                        if (squareList.Count > 0) {
                            VibError(); 
                        }

						if (squareList.Count > 0) {
							VibError ();
						}

                        FallingDone();
                    }

                    RemoveLine();
                    updateInitialize = true;
                }
			}
		}
	}
		
	public void ControlChange() {
		controlsEnabled = !controlsEnabled;
	}

	public void AddSquare (int squareNum, int colorNum, bool hoverSwitch)
	{ 
		if (controlsEnabled) {
#region same square
			if (lastSquare == 0) {
				square = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareNum.ToString ());
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
                VibMove();

				lastSquare = squareNum;
				squareList.Add (squareNum);

				newSquareInitialize = true;
			} 
#endregion

#region next squares
			else if (lastSquare != squareNum && hoverSwitch == false) {	
				square = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareNum.ToString ());
				SquareScript squareScript = square.GetComponent<SquareScript> ();

				if (lastSquare > 0) {
					square2 = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + lastSquare.ToString ());
				}

				if (square2.GetComponent<SpriteRenderer> ().color == square.GetComponent<SpriteRenderer> ().color && !squareList.Contains (squareNum)) {
#region squares close

#region up
					if (squareNum == lastSquare - squareRows) {
						DrawLine (square, square2, squareNum, 90);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                            VibMove();

						} else {
							audioSource.PlayOneShot (hits [hits.Length - 1]);
                            VibMove();

						}	
					} 
#endregion

#region left
					else if (squareNum == lastSquare - 1 && (squareNum % squareRows != 0)) {
						DrawLine (square, square2, squareNum, 180);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                            VibMove();

						} else {
							audioSource.PlayOneShot (hits [hits.Length - 1]);
                            VibMove();

						}		
					}
#endregion

#region right
					else if (squareNum == lastSquare + 1 && ((squareNum - 1) % squareRows != 0)) {
						DrawLine (square, square2, squareNum, 0);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                            VibMove();

						} else {
							audioSource.PlayOneShot (hits [hits.Length - 1]);
                            VibMove();

						}	
					} 
#endregion

#region down
					else if (squareNum == lastSquare + squareRows) {
						DrawLine (square, square2, squareNum, 270);

						squareScript.hoverSwitch = !squareScript.hoverSwitch;

						if (squareList.Count < hits.Length) {
							audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                            VibMove();

						} else {
							audioSource.PlayOneShot (hits [hits.Length - 1]);
                            VibMove();

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
									GameObject tempSquare = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + (tempSquareNum).ToString ());
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

									GameObject SquareClose = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 90);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;

									lastSquare = squareFarNum;
								}
									
								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                                    VibMove();

								} else {
									audioSource.PlayOneShot (hits [hits.Length - 1]);
                                    VibMove();

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
									GameObject tempSquare = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + (tempSquareNum).ToString ());
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

									GameObject SquareClose = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 270);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;
									lastSquare = squareFarNum;
								}

								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                                    VibMove();

								} else {
									audioSource.PlayOneShot (hits [hits.Length - 1]);
                                    VibMove();

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
									GameObject tempSquare = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + (tempSquareNum).ToString ());
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

									GameObject SquareClose = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 180);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;

									lastSquare = squareFarNum;
								}
									
								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                                    VibMove();

								} else {
									audioSource.PlayOneShot (hits [hits.Length - 1]);
                                    VibMove();

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
									GameObject tempSquare = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + (tempSquareNum).ToString ());
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

									GameObject SquareClose = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareCloseNum.ToString ());
									GameObject SquareFar = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareFarNum.ToString ());
									DrawLine (SquareFar, SquareClose, squareFarNum, 0);

									SquareScript squareScript2 = SquareFar.GetComponent<SquareScript> ();
									squareScript2.hoverSwitch = !squareScript2.hoverSwitch;

									lastSquare = squareFarNum;
								}

								if (squareList.Count < hits.Length) {
									audioSource.PlayOneShot (hits [squareList.Count - 1]);	
                                    VibMove();

								} else {
									audioSource.PlayOneShot (hits [hits.Length - 1]);
                                    VibMove();

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
			/* else if ((lastSquare == squareNum) && hoverSwitch) {
				square = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareNum.ToString ());
				square2 = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareList [squareList.Count - 1].ToString ());

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
			} */
            else if (squareList.Count > 1 && hoverSwitch) {
				if ((squareNum == squareList [squareList.Count - 2])) {
					GameObject square = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareList [squareList.Count - 1].ToString ());
					square2 = GameObject.Find ("Game/GameCanvas/BG1/BG2/Squares/" + squareList [squareList.Count - 1].ToString ());

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
                        VibMove();

					} else {
						audioSource.PlayOneShot (hits [hits.Length - 1]);
                        VibMove();

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
		localScaleY = (circle.transform.lossyScale.x / 75f);

		lineChild.transform.localPosition = new Vector2 (((square.transform.lossyScale.x / 2)) / 0.85f, 0);
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

	/*
	public void CheckAvailableMoves() {
		checkAroundCounter = checkAroundCounter + 1;

		Debug.Log (checkAroundCounter);

		if (checkAroundCounter == squareRows*squareRows) {
			bool movePossible = false;

			for (int i = 1; i <= squareRows * squareRows; i++) {
				if (!movePossible) {
					if (CheckAroundSquare1 (i)) {
						movePossible = true;
					}
				}
			}

			if (movePossible) {
				Debug.Log ("go");
			} else {
				Debug.Log ("game over");
			}
		}
	}
	*/
		
	bool CheckAroundSquare1(int squareNum) {
		bool movePossible = false;

		if (squareNum - squareRows > 0 && !movePossible) {
			if (colorList [squareNum] == colorList [squareNum - squareRows]) {
				movePossible = CheckAroundSquare2(squareNum - squareRows, squareNum);
			}
		}
		if (Convert.ToDouble(squareNum) % squareRows != 1 && !movePossible) {
			if (colorList [squareNum] == colorList [squareNum - 1]) {
				movePossible = CheckAroundSquare2(squareNum - 1, squareNum);
			}
		}
		if (Convert.ToDouble(squareNum) % squareRows != 0 && !movePossible) {
			if (colorList [squareNum] == colorList [squareNum + 1]) {
				movePossible = CheckAroundSquare2(squareNum + 1, squareNum);
			}
		}
		if (Convert.ToDouble(squareNum) + squareRows <= squareRows * squareRows && !movePossible) {
			if (colorList [squareNum] == colorList [squareNum + squareRows]) {
				movePossible = CheckAroundSquare2(squareNum + squareRows, squareNum);
			}
		}

		return movePossible;
	}

	/*

	bool CheckAroundSquare2(int squareNum) {
		bool movePossible = false;

		if (squareNum - squareRows > 0) {
			if (colorList [squareNum] == colorList [squareNum - squareRows]) {
				movePossible = CheckAroundSquare3(squareNum - squareRows);
			}
		}
		if (Convert.ToDouble(squareNum) % squareRows != 1) {
			if (colorList [squareNum] == colorList [squareNum - 1]) {
				movePossible = CheckAroundSquare3(squareNum - 1);
			}
		}
		if (Convert.ToDouble(squareNum) % squareRows != 0) {
			if (colorList [squareNum] == colorList [squareNum + 1]) {
				movePossible = CheckAroundSquare3(squareNum + 1);
			}
		}
		if (Convert.ToDouble(squareNum) + squareRows <= squareRows * squareRows) {
			if (colorList [squareNum] == colorList [squareNum + squareRows]) {
				movePossible = CheckAroundSquare3(squareNum + squareRows);
			}
		}

		return movePossible;
	}

*/

	bool CheckAroundSquare2(int squareNum, int orgSquareNum) {
		bool movePossible = false;

		if (squareNum - squareRows != orgSquareNum && !movePossible) {
			if (squareNum - squareRows > 0) {
				if (colorList [squareNum] == colorList [squareNum - squareRows]) {
					movePossible = true;
				}
			}
		}

		if (squareNum - 1 != orgSquareNum && !movePossible) {
			if (Convert.ToDouble (squareNum) % squareRows != 1) {
				if (colorList [squareNum] == colorList [squareNum - 1]) {
					movePossible = true;
				}
			}
		}

		if (squareNum + 1 != orgSquareNum && !movePossible) {
			if (Convert.ToDouble (squareNum) % squareRows != 0) {
				if (colorList [squareNum] == colorList [squareNum + 1]) {
					movePossible = true;
				}
			}
		}

		if (squareNum + squareRows != orgSquareNum && !movePossible) {
			if (Convert.ToDouble (squareNum) + squareRows <= squareRows * squareRows) {
				if (colorList [squareNum] == colorList [squareNum + squareRows]) {
					movePossible = true;
				}
			}
		}

		return movePossible;
	}
		
	public void AddToColorList(int index, int value) {
		colorList[index] = value;
	}

#region animation

	public void StartAnimation ()
	{
		controlsEnabled = false;

		foreach (int square in squareList) {
            GameObject squareObj = GameObject.Find("Game/GameCanvas/BG1/BG2/Squares/" + square.ToString ());
			squareObj.SendMessage ("Animate");
		}

		score = score + squareList.Count;
		scoreText.text = "" + score;

		if (score >= highScore) {
			highScore = score;
			highScoreText.text = "<color=#B7A921ff>★</color>" + score;
		}

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
                        instSquare.GetComponent<SquareScript>().rowNum = row + 1;

                        if (i == 0)
                        {
                            instSquare.GetComponent<SquareScript>().takeColorFromTop = true;
                        }

                        else {
                            instSquare.GetComponent<SquareScript>().takeColorFromTop = false;
                        }
                    }
				}
			}
			foreach (Transform child in squares.transform) {
				child.SendMessage ("InitializeFall", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
				
	public void AddToFallCounter ()
	{
		fallenSquareCounter = fallenSquareCounter + 1;

		if (fallenSquareCounter == squareRows * squareRows) {
			FallingDone ();
			fallenSquareCounter = 0;
		}
	}

	public void FallingDone ()
	{
		controlsEnabled = true;
		lastSquare = 0;
		fallDown = false;
		InitializeFallHasBeenCalled = false;

        foreach (Transform square in squares.transform)
        {
            square.SendMessage("NameSquare", SendMessageOptions.DontRequireReceiver);
        }

		rowList.Clear ();
		for (int i = 0; i < squareRows; i++) {
			rowList.Add (0);
		}

		bool movePossible = false;

		for (int i = 1; i <= squareRows * squareRows; i++) {
			if (!movePossible) {
				if (CheckAroundSquare1 (i)) {
					movePossible = true;
				}
			}
		}

		if (movePossible) {
			Debug.Log ("go");
		} else {
			Debug.Log ("game over");
		}

        squareList.Clear ();
	}
				
	public void PlaySound ()
	{
		if (randomizeColors) {
			audioSource.PlayOneShot (snap2);
		} else {
			audioSource.PlayOneShot (snap);
		}
	}

    void VibMove() {
        if (platformInt == 1) {
            iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)2);  
        }
        else if (platformInt == 2) {
            Vibration.Vibrate(15);
        }    
    }

    void VibSuccess() {
        if (platformInt == 1)
        {
            iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)4);
        }
        else if (platformInt == 2)
        {
            Vibration.Vibrate(15);
        }
    }

    void VibError() {
        if (platformInt == 1)
        {
            iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)6);
        }
        else if (platformInt == 2)
        {
            Vibration.Vibrate(15);
        }
    }

#endregion
}