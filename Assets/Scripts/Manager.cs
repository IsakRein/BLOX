using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager instance = null;

    public static int highScore;
    public static int gemCount = 10;
    public static List<string> sceneOrder = new List<string>();

    public static bool soundEnabled;
    public static bool vibEnabled;

    public static List<int> colorList = new List<int>(); 
    public static List<int> deadSquareCounterList = new List<int>(); 
    public static List<int> circleList = new List<int>();

    public static bool thereIsPrevious;
    public static List<int> previousColorList = new List<int>();
    public static List<int> previousDeadSquareCounterList = new List<int>(); 
    public static List<int> previousCircleList = new List<int>();

    public static bool loadColors;
    public static int previousScore;
    public static int previousPreviousScore;

    public static int selectedTheme = 1;

    public GameObject colorManagerHome;

    public static List<Color> staticTheme = new List<Color>(); 

    public List<Color> theme1 = new List<Color>(); 
    public List<Color> theme2 = new List<Color>(); 
    public List<Color> theme3 = new List<Color>();
    public List<Color> theme4 = new List<Color>();
    public List<Color> theme5 = new List<Color>();
    public List<Color> theme6 = new List<Color>();
    public List<Color> theme7 = new List<Color>();
    public List<Color> theme8 = new List<Color>();

    public static List<Color> staticTheme1 = new List<Color>();
    public static List<Color> staticTheme2 = new List<Color>();
    public static List<Color> staticTheme3 = new List<Color>();
    public static List<Color> staticTheme4 = new List<Color>();
    public static List<Color> staticTheme5 = new List<Color>();
    public static List<Color> staticTheme6 = new List<Color>();
    public static List<Color> staticTheme7 = new List<Color>();
    public static List<Color> staticTheme8 = new List<Color>();

    public static List<int> boughtThemes = new List<int>();

    public static List<Sprite> cats = new List<Sprite>();
    public List<Sprite> setCats = new List<Sprite>();

    public static Sprite circle = new Sprite();
    public Sprite setCircle;

    public static Sprite deadCircle = new Sprite();
    public Sprite setDeadCircle;

    public bool loadColorsCheck;

    void Awake() {
        Application.targetFrameRate = 60;

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        staticTheme1 = theme1;
        staticTheme2 = theme2;
        staticTheme3 = theme3;
        staticTheme4 = theme4;
        staticTheme5 = theme5;
        staticTheme6 = theme6;
        staticTheme7 = theme7;
        staticTheme8 = theme8;

        cats = setCats;
        circle = setCircle;
        deadCircle = setDeadCircle;

        if (PlayerPrefs.HasKey("Theme")) {
            selectedTheme = PlayerPrefs.GetInt("Theme");
        }
        else {
            selectedTheme = 1;
        }

        UpdateTheme();

        colorManagerHome.GetComponent<ColorManagerHome>().LoadThemeColors();


        //load bought themes

        if (PlayerPrefs.HasKey("BoughtTheme_1")) {
            for (int i = 0; i <= 8; i++) {
                boughtThemes.Add(0);
            }

            boughtThemes[0] = -1;

            for (int i = 0; i <= 8; i++)
            {
                boughtThemes[i] = PlayerPrefs.GetInt("BoughtTheme_" + i);
            }
        }
        else {
            for (int i = 0; i <= 8; i++)
            {
                boughtThemes.Add(0);
            }

            boughtThemes[0] = -1;
            boughtThemes[1] = 1;
            boughtThemes[2] = 1;
        }


        //temporary code, change if more squares are needed

        colorList.Clear();
        for (int i = 0; i <= 36; i++)
        {
            colorList.Add(-1);
        }

        deadSquareCounterList.Clear();
        for (int i = 0; i <= 36; i++)
        {
            deadSquareCounterList.Add(0);
        }

        circleList.Clear();
        for (int i = 0; i <= 6; i++)
        {
            circleList.Add(-1);
        }

        previousColorList.Clear();
        for (int i = 0; i <= 36; i++)
        {
            previousColorList.Add(-1);
        }

        previousDeadSquareCounterList.Clear();
        for (int i = 0; i <= 36; i++)
        {
            previousDeadSquareCounterList.Add(-1);
        }

        previousCircleList.Clear();
        for (int i = 0; i <= 6; i++)
        {
            previousCircleList.Add(-1);
        }

        //end of temporary code

        if (PlayerPrefs.HasKey ("Gems")) {
            gemCount = PlayerPrefs.GetInt("Gems");
        }

        else {
            gemCount = 0;
        }

        if (PlayerPrefs.HasKey ("LoadColors")) {			
            if (PlayerPrefs.GetInt ("LoadColors") == 1) {
                if (PlayerPrefs.HasKey ("colorList_1")) {
                    loadColors = true;

                    colorList.Clear();
                    for (int i = 0; i <= 36; i++)
                    {
                        colorList.Add(-1);
                    }

                    deadSquareCounterList.Clear();
                    for (int i = 0; i <= 36; i++)
                    {
                        deadSquareCounterList.Add(0);
                    }

                    circleList.Clear();
                    for (int i = 0; i <= 6; i++)
                    {
                        circleList.Add(-1);
                    }

                    for (int i = 1; i <= 36; i++) {
						colorList [i] = PlayerPrefs.GetInt ("colorList_" + i);
					}

                    for (int i = 1; i <= 36; i++) {
						deadSquareCounterList [i] = PlayerPrefs.GetInt ("deadSquareCounter_" + i);
					}
                    	
                    for (int i = 1; i <= 6; i++)
                    {
                        circleList[i] = PlayerPrefs.GetInt("circleList_" + i);
                    }

				} 

                else {
                    loadColors = false;

                    colorList.Clear();
                    for (int i = 0; i <= 36; i++)
                    {
                        colorList.Add(-1);
                    }

                    deadSquareCounterList.Clear();
                    for (int i = 0; i <= 36; i++)
                    {
                        deadSquareCounterList.Add(0);
                    }

                    circleList.Clear();
                    for (int i = 0; i <= 6; i++)
                    {
                        circleList.Add(-1);
                    }
				}

                if (PlayerPrefs.HasKey("previousColorList_1")) {

                    previousColorList.Clear();
                    for (int i = 0; i <= 36; i++)
                    {
                        previousColorList.Add(-1);
                    }

                    previousCircleList.Clear();
                    for (int i = 0; i <= 6; i++)
                    {
                        previousCircleList.Add(-1);
                    }

                    previousDeadSquareCounterList.Clear();
                    for (int i = 0; i <= 36; i++)
                    {
                        previousDeadSquareCounterList.Add(-1);
                    }

                    for (int i = 1; i <= 36; i++)
                    {
                        previousColorList[i] = PlayerPrefs.GetInt("previousColorList_" + i);
                    }

                    for (int i = 1; i <= 6; i++)
                    {
                        previousCircleList[i] = PlayerPrefs.GetInt("previousCircleList_" + i);
                    }

                    for (int i = 1; i <= 36; i++)
                    {
                        previousDeadSquareCounterList[i] = PlayerPrefs.GetInt("previousDeadSquareCounterList_" + i);
                    }


                    previousPreviousScore = PlayerPrefs.GetInt("previousPreviousScore");
                }

                else {
                    previousColorList = new List<int>(colorList);
                    previousCircleList = new List<int>(circleList);
                    previousDeadSquareCounterList = new List<int>(deadSquareCounterList);
                }
            }

            else {
                loadColors = false;

                colorList.Clear();
                for (int i = 0; i <= 36; i++)
                {
                    colorList.Add(-1);
                }
                for (int i = 1; i <= 36; i++)
                {
                    colorList[i] = PlayerPrefs.GetInt("colorList_" + i);
                }

                deadSquareCounterList.Clear();
                for (int i = 0; i <= 36; i++)
                {
                    deadSquareCounterList.Add(0);
                }
                for (int i = 1; i <= 36; i++)
                {
                    deadSquareCounterList[i] = PlayerPrefs.GetInt("deadSquareCounter_" + i);
                }

                circleList.Clear();
                for (int i = 0; i <= 6; i++)
                {
                    circleList.Add(-1);
                }
                for (int i = 1; i <= 6; i++)
                {
                    circleList[i] = PlayerPrefs.GetInt("circleList_" + i);
                }
            }
		}

        else 
        {
            loadColors = false;
        }

        if (PlayerPrefs.HasKey("thereIsPrevious")) {
            if (PlayerPrefs.GetInt("thereIsPrevious") == 1) {
                thereIsPrevious = true;
            }
            else {
                thereIsPrevious = false;
            }
        }
        else {
            thereIsPrevious = false;
        }

        NextTimeLoadLevel();

        if (PlayerPrefs.HasKey("previousScore")) {
            previousScore = PlayerPrefs.GetInt("previousScore");
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("soundEnabled")) {
            if (PlayerPrefs.GetInt("soundEnabled") == 1)
            {
                soundEnabled = true;
            }
            else
            {
                soundEnabled = false;
            }
        }

        if (PlayerPrefs.HasKey("vibEnabled"))
        {
            if (PlayerPrefs.GetInt("vibEnabled") == 1)
            {
                vibEnabled = true;
            }
            else
            {
                vibEnabled = false;
            }
        }

        if (PlayerPrefs.HasKey("highScore")) {
            highScore = PlayerPrefs.GetInt("highScore");
        }
        else {
            highScore = 0;
        }
    }

	void Update()
	{
        loadColorsCheck = loadColors;
	}

	public static void UpdateTheme()
    {
        if (selectedTheme == 1)
        {
            staticTheme = staticTheme1;
        }

        else if (selectedTheme == 2)
        {
            staticTheme = staticTheme2;
        }

        else if (selectedTheme == 3)
        {
            staticTheme = staticTheme3;
        }

        else if (selectedTheme == 4)
        {
            staticTheme = staticTheme4;
        }

        else if (selectedTheme == 5)
        {
            staticTheme = staticTheme5;
        }

        else if (selectedTheme == 6)
        {
            staticTheme = staticTheme6;
        }

        else if (selectedTheme == 7)
        {
            staticTheme = staticTheme7;
        }

        else if (selectedTheme == 8)
        {
            staticTheme = staticTheme8;
        }

        if (!PlayerPrefs.HasKey("showAds")) {
            PlayerPrefs.SetInt("showAds", 1);
        }
    }

    public static void SaveScore()
    {
        PlayerPrefs.SetInt("highScore", highScore);
    }

    public static void SaveGemCount()
    {
        PlayerPrefs.SetInt("Gems", gemCount);
    }

    public static void SaveBoughtThemes() 
    {
        for (int i = 0; i <= 8; i++)
        {
            PlayerPrefs.SetInt("BoughtTheme_" + i, boughtThemes[i]);
        }
    }

    public static void SaveScene() 
    {
        for (int i = 1; i <= 36; i++)
        {
            PlayerPrefs.SetInt("colorList_" + i, colorList[i]);
            PlayerPrefs.SetInt("deadSquareCounter_" + i, deadSquareCounterList[i]);
           
        }

        for (int i = 1; i <= 6; i++)
        {
            PlayerPrefs.SetInt("circleList_" + i, circleList[i]);
        }

        PlayerPrefs.SetInt("previousScore", previousScore);


        //previous
        for (int i = 1; i <= 36; i++)
        {
            PlayerPrefs.SetInt("previousColorList_" + i, previousColorList[i]);
        }

        for (int i = 1; i <= 6; i++)
        {
            PlayerPrefs.SetInt("previousCircleList_" + i, previousCircleList[i]);
        }

        for (int i = 1; i <= 36; i++)
        {
            PlayerPrefs.SetInt("previousDeadSquareCounterList_" + i, previousDeadSquareCounterList[i]);
        }

        PlayerPrefs.SetInt("previousPreviousScore", previousPreviousScore);
    }

    public static void SaveSoundSettings()
    {
        if (soundEnabled) {
            PlayerPrefs.SetInt("soundEnabled", 1);
        }
        else {
            PlayerPrefs.SetInt("soundEnabled", 0);
        }
    }

    public static void SaveVibSettings()
    {
        if (vibEnabled)
        {
            PlayerPrefs.SetInt("vibEnabled", 1);
        }
        else
        {
            PlayerPrefs.SetInt("vibEnabled", 0);
        }
    }

	public static void NextTimeLoadLevel() {
		PlayerPrefs.SetInt ("LoadColors", 1);
        PlayerPrefs.SetInt ("thereIsPrevious", 1);
	}

    public static void NextTimeDontLoadLevel()
    {
        PlayerPrefs.SetInt("LoadColors", 0);
        PlayerPrefs.SetInt("thereIsPrevious", 0);
    }
}