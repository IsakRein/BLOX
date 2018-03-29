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

        SetSprite();
	}

    void NewColor() {
        colorNum = Random.Range(0, setColor.Length-1);

        float rolledDie = Random.Range(0.00f, 1.00f);

        if (rolledDie <= lineScript.deadOdds) {
            colorNum = setColor.Length-1;
        }

        lineScript.AddToCircleList(num, colorNum);

        spriteRenderer.color = setColor[colorNum];

        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;

        SetSprite();
    }

    void SetColornum() {
        spriteRenderer.color = setColor[colorNum];

        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;

        SetSprite();
    }

    public void LoadPreviousColor()
    {
        colorNum = Manager.previousCircleList[num];
        spriteRenderer.color = setColor[colorNum];

        SetSprite();

        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;

        UpdateCirclelist();
    }

    void UpdateCirclelist()
    {
        lineScript.AddToCircleList(num, colorNum);
    }

    void SetSprite() {
        if (colorNum == setColor.Length - 1)
        {
            spriteRenderer.sprite = lineScript.deadSquare;
        }

        else
        {
            if (Manager.selectedTheme == 8) {
                spriteRenderer.sprite = Manager.cats[colorNum];
            }

            else {
                spriteRenderer.sprite = lineScript.regularSquare;
            }
        }
    }
}