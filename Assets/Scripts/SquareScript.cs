using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class SquareScript : MonoBehaviour
{
    public GameObject squares;
    public GameObject Line;

    public Game squareScript;
    public LineScript lineScript;

    public Color[] setColor;
    public int colorNum;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isOnMobile = true;

    public int number;

    [Space]

    public bool hoverSwitch = false;

    [Space]

    public bool isHovering = false;
    public bool addSquareHasBeenCalled = false;
    public bool interactable = false;
    private int squareRows;
    public int fallCounter;
    public float speed = 1f;
    public Vector3 targetPos;
    private bool fallInitialized = false;
    public bool isAnimating;
    public float animEditor;
    public bool takeColorFromTop = false;
    public int rowNum;

    public bool loadColors;

    public GameObject countDownPrefab;
    GameObject countDown;

    public int countDownStart = 5;
    public int countDownCounter;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        Line = GameObject.Find("LineHolder");
        lineScript = Line.GetComponent<LineScript>();

        squares = GameObject.Find("Squares");
        squareScript = squares.GetComponent<Game>();
        squareRows = squareScript.Rows;

        setColor = lineScript.setColor;

        countDownCounter = countDownStart;

        if (loadColors) {
            colorNum = Manager.colorList[number];
            spriteRenderer.color = setColor[colorNum];
        }

        else if (takeColorFromTop)
        {
			GameObject square = GameObject.Find ("Game/GameCanvas/BG1/BG2/Circles/" + rowNum.ToString ());
			colorNum = square.transform.GetChild(0).GetComponent<NextSquareScript>().colorNum;
			square.transform.GetChild(0).SendMessage("NewColor");

            spriteRenderer.color = setColor[colorNum];
        }
        else
        {
            colorNum = Random.Range(0, setColor.Length-1);
            spriteRenderer.color = setColor[colorNum];
        }

        if (colorNum == setColor.Length-1)
        {
            spriteRenderer.sprite = lineScript.deadSquare;

            countDownPrefab = (GameObject)Resources.Load("Prefabs/CountDown", typeof(GameObject));

            countDown = Instantiate(countDownPrefab, transform);

            countDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        addSquareHasBeenCalled = false;

        isHovering = false;

        hoverSwitch = false;

        squareRows = squareScript.Rows;
    }

    void Update()
    {
        if (isAnimating)
        {
            transform.localPosition = new Vector2(transform.localPosition.x + animEditor, transform.localPosition.y);

        }

        if (!isHovering)
        {
            addSquareHasBeenCalled = false;
        }

        if (fallInitialized)
        {
            if (lineScript.fallDown == true && fallCounter > 0)
            {
                //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref velocity, 0.1f);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed / 50);

                if (targetPos == transform.localPosition)
                {
                    fallCounter = 0;

                    lineScript.AddToFallCounter();

                    NameSquare();

                    fallInitialized = false;
                }
            }
        }


#if UNITY_EDITOR
        isOnMobile = false;

        if (!(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)))
        {
            addSquareHasBeenCalled = false;

            hoverSwitch = false;
        }

#endif

        if (isOnMobile)
        {
            if (Input.touchCount == 0)
            {
                addSquareHasBeenCalled = false;

                hoverSwitch = false;
            }
        }
    }

    public void LoadColor(int newColorNum) 
    {
        colorNum = newColorNum;

        Animate();

        spriteRenderer.color = setColor[colorNum];
    }

    public void NameSquare()
    {
        float x = (transform.localPosition.x - (0.5f - (((float)squareRows) / 2))) + 1;
        float y = (((((float)squareRows) / 2) - 0.5f) - transform.localPosition.y);

        number = (int)(Mathf.CeilToInt(squareRows * y) + Mathf.Round(x));

        gameObject.name = "" + number;

        x = Mathf.Round(transform.localPosition.x - 0.5f) + 0.5f;
        y = Mathf.Round(transform.localPosition.y - 0.5f) + 0.5f;

        transform.localPosition = new Vector2(x, y);

        UpdateColorlist();
    }

    public void UpdateCountDown() {
        if (colorNum == setColor.Length-1) {
            countDownCounter = countDownCounter - 1;

            if (countDownCounter == 0) {
                animator.SetTrigger("Trigger");
                lineScript.squareList.Add(number);
            }

            GetComponentInChildren<TextMeshProUGUI>().SetText(countDownCounter.ToString());
        }
    }

    void UpdateColorlist()
    {
        lineScript.AddToColorList(System.Convert.ToInt32(gameObject.name), colorNum);
    }

    void StartErrorAnimation()
    {
        interactable = false;

        isAnimating = true;
    }

    void EndErrorAnimation()
    {
        interactable = true;

        isAnimating = false;

        NameSquare();
    }

    void OnTouchDown()
    {
        isHovering = true;

        if (addSquareHasBeenCalled == false && interactable)
        {
            int num = System.Convert.ToInt32(gameObject.name);
            lineScript.AddSquare(num, colorNum, hoverSwitch);

            addSquareHasBeenCalled = true;
        }
    }

    void OnTouchUp()
    {
        isHovering = false;
    }

    void OnTouchStay()
    {
        isHovering = true;


        if (addSquareHasBeenCalled == false && interactable)
        {
            int num = System.Convert.ToInt32(gameObject.name);
            lineScript.AddSquare(num, colorNum, hoverSwitch);

            addSquareHasBeenCalled = true;
        }
    }

    void InitializeFall()
    {
        if (fallCounter == 0)
        {
            lineScript.AddToFallCounter();
            fallInitialized = true;
        }
        else
        {
            targetPos = transform.localPosition;
            targetPos.y = transform.localPosition.y - fallCounter;
            interactable = true;
            fallInitialized = true;
        }
    }

    void Animate()
    {
        animator.SetTrigger("Trigger");
    }

    void AnimateError()
    {
        animator.SetTrigger("Error");
    }

    void OnTouchExit()
    {
        isHovering = false;
    }

    void EnableFall()
    {
        lineScript.InitializeFall();
    }

    void PlaySound()
    {
        lineScript.PlaySound();
    }

    void MakeInteractable()
    {
        NameSquare();

        interactable = true;
    }

    void DisableGameObject()
    {
		int iValue;

		int.TryParse(gameObject.name, out iValue);

		for (int i = iValue; i > 0; i = i - squareRows)
        {
            GameObject.Find("Game/GameCanvas/BG1/BG2/Squares/" + i.ToString()).SendMessage("AddToFallCounter", 1);
        }

        GameObject.Destroy(gameObject);
    }

    void AddToFallCounter(int howMuch)
    {
        fallCounter = fallCounter + howMuch;
    }
}