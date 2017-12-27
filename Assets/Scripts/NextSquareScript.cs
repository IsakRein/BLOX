using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSquareScript : MonoBehaviour {
    public GameObject squares;
    public GameObject Line;

    public Game squareScript;
    public LineScript lineScript;

    public Color[] setColor;
    public int colorNum;

    private SpriteRenderer spriteRenderer;

	void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Line = GameObject.Find("LineHolder");
        lineScript = Line.GetComponent<LineScript>();

        squares = GameObject.Find("Squares");
        squareScript = squares.GetComponent<Game>();

        setColor = lineScript.setColor;

        NewColor();
	}
	
    void NewColor() {
        colorNum = Random.Range(0, setColor.Length);
        spriteRenderer.color = setColor[colorNum];

        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;


    }
}
