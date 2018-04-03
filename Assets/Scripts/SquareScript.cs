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

    public int platformInt;

    public Game squareScript;
    public LineScript lineScript;

    public Color[] setColor;
    public int colorNum;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public int number;

    [Space]

    public bool hoverSwitch = false;

    [Space]

    public bool isHovering = false;
    public bool addSquareHasBeenCalled = false;
    public bool interactable = false;
    private int squareRows;
    public int fallCounter;
    public Vector3 targetPos;
    private bool fallInitialized = false;
    public bool isAnimating;
    public float animEditor;
    public bool takeColorFromTop = false;
    public int rowNum;

    public bool loadColors;

    public GameObject countDownPrefab;
    GameObject countDown;

    public int countDownStart = 10;
    public int countDownCounter;

    private bool firstFall;

    public bool hammerOn = false;
    public bool removeOn = false;

    public bool isGenerated;

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

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        Line = GameObject.Find("LineHolder");
        lineScript = Line.GetComponent<LineScript>();

        squares = GameObject.Find("Squares");
        squareScript = squares.GetComponent<Game>();
        squareRows = squareScript.Rows;

        setColor = lineScript.setColor;

        if (loadColors)
        {
            colorNum = Manager.colorList[number];
            spriteRenderer.color = setColor[colorNum];

            firstFall = false;
        }

        else if (takeColorFromTop)
        {
            GameObject square = GameObject.Find("Game/GameCanvas/BG1/BG2/Circles/" + rowNum.ToString());
            colorNum = square.transform.GetChild(0).GetComponent<NextSquareScript>().colorNum;
            square.transform.GetChild(0).SendMessage("NewColor");

            countDownCounter = countDownStart;

            spriteRenderer.color = setColor[colorNum];

            firstFall = true;
        }
        else
        {
            colorNum = Random.Range(0, setColor.Length - 1);
            spriteRenderer.color = setColor[colorNum];

            countDownCounter = countDownStart;

            firstFall = true;
        }

        if (colorNum == setColor.Length - 1)
        {
            spriteRenderer.sprite = lineScript.deadSquare;

            if (Manager.selectedTheme == 7)
            {
                spriteRenderer.sprite = Manager.deadCircle;
            }

            countDownPrefab = (GameObject)Resources.Load("Prefabs/CountDown", typeof(GameObject));

            countDown = Instantiate(countDownPrefab, transform);

            countDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            countDown.GetComponentInChildren<TextMeshProUGUI>().color = Manager.staticTheme[28];

            if (loadColors)
            {
                countDownCounter = Manager.deadSquareCounterList[number];
                GetComponentInChildren<TextMeshProUGUI>().SetText(countDownCounter.ToString());
            }
        }

        else
        {
            if (Manager.selectedTheme == 7)
            {
                spriteRenderer.sprite = Manager.circle;
            }

            if (Manager.selectedTheme == 8)
            {
                spriteRenderer.sprite = Manager.cats[colorNum];

                int tempColorValue = 255 - colorNum;
                spriteRenderer.color = new Color(tempColorValue, tempColorValue, tempColorValue);
            }
        }

        
        if (isGenerated) 
        {
            if (colorNum == setColor.Length - 1)
            {
                animator.Play("DeadSquareEntry");

                transform.eulerAngles = new Vector3(0, 0, -90);
            }

            else
            {
                animator.Play("Entry");

                transform.eulerAngles = new Vector3(0, 0, 0);
            }
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

                float step = 8f * Time.deltaTime;

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, step);

                if (targetPos.y == transform.localPosition.y)
                {
                    fallInitialized = false;

                    fallCounter = 0;

                    firstFall = false;

                    NameSquare();

                    lineScript.AddToFallCounter();

                    if (firstFall)
                    {
                        UpdateCountDown();
                    }
                }
            }
        }


        if (platformInt == 0)
        {
            if (!(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)))
            {
                addSquareHasBeenCalled = false;

                hoverSwitch = false;
            }
        }

        else if (platformInt > 0)
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

    public void LoadPreviousColor () 
    {       
        colorNum = Manager.previousColorList[number];
        spriteRenderer.color = setColor[colorNum];

        foreach (Transform child in transform)
        {
            if (child.name == "CountDown(Clone)")
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        if (Manager.selectedTheme == 7)
        {
            spriteRenderer.sprite = Manager.circle;
        }

        if (Manager.selectedTheme == 8)
        {
            spriteRenderer.sprite = Manager.cats[colorNum];
        }

        if (colorNum == setColor.Length - 1)
        {
            spriteRenderer.sprite = lineScript.deadSquare;
            countDownPrefab = (GameObject)Resources.Load("Prefabs/CountDown", typeof(GameObject));
            countDown = Instantiate(countDownPrefab, transform);
            countDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            countDownCounter = Manager.previousDeadSquareCounterList[number];

            countDown.GetComponentInChildren<TextMeshProUGUI>().SetText(countDownCounter.ToString());
        }

        else
        {
            spriteRenderer.sprite = lineScript.regularSquare;
        }

        if (colorNum == setColor.Length - 1)
        {
            lineScript.AddToDeadSquareCounterList(number, countDownCounter);
        }
        else
        {
            lineScript.AddToDeadSquareCounterList(number, 0);
        }

        UpdateColorlist();
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
   
    public void SetCountDown () {
        if (colorNum == setColor.Length - 1)
        {
            countDownCounter = countDownCounter - 1;

            if (countDownCounter == 0)
            {
                animator.SetTrigger("Trigger");
                lineScript.squareList.Add(number);
            }

            GetComponentInChildren<TextMeshProUGUI>().SetText(countDownCounter.ToString());
        }
    }

    public void UpdateCountDown() {
        if (colorNum == setColor.Length-1) {
            lineScript.AddToDeadSquareCounterList(number, countDownCounter); 
        }
        else {
            lineScript.AddToDeadSquareCounterList(number, 0); 
        }
    }

    void UpdateColorlist()
    {
        lineScript.AddToColorList(number, colorNum);
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
        OnTouchStay();
    }

    void OnTouchUp()
    {
        isHovering = false;
    }

    void OnTouchStay()
    {
        isHovering = true;

        if (!addSquareHasBeenCalled && interactable)
        {
            if (hammerOn)
            {
                animator.Play("Default");

                lineScript.FallOne(number);
            }

            else if (removeOn)
            {
                lineScript.EndRemove();
                lineScript.FallOneColor(colorNum);
            }

            else
            {
                if (!addSquareHasBeenCalled && interactable)
                {
                    lineScript.AddSquare(number, colorNum, hoverSwitch);
                }
            }

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

        animator.Play("Default");

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