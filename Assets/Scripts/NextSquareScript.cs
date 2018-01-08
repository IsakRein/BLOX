using System.Linq;
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
    public int num;

    private SpriteRenderer spriteRenderer;

    public bool loadColors;

	void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Line = GameObject.Find("LineHolder");
        lineScript = Line.GetComponent<LineScript>();

        squares = GameObject.Find("Squares");
        squareScript = squares.GetComponent<Game>();

        setColor = lineScript.setColor;

        if (loadColors) {
            colorNum = Manager.circleList[num];

            lineScript.AddToCircleList(num, colorNum);

            spriteRenderer.color = setColor[colorNum];

            Color color = spriteRenderer.color;
            color.a = 0.5f;
            spriteRenderer.color = color;
        }

        else {
            NewColor();
        }
	}
	
    void NewColor() {
        colorNum = Random.Range(0, setColor.Length);

        lineScript.AddToCircleList(num, colorNum);

        spriteRenderer.color = setColor[colorNum];

        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
    }
}
