using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseColorAnim : MonoBehaviour {

    public bool animate;
    public   bool start;

    public Image crossLine1;
    public Image crossLine2;

    public TextMeshProUGUI gemCount;
    public Text plusSign;

    public Color colorRegular;
    public Color colorPause;

    public Color textRegular;
    public Color textPause;

    private float duration = 0.6666f;
    public float t;

	void Start () {
        colorRegular = Manager.staticTheme[9];
        colorPause = Manager.staticTheme[10];

        textRegular = Manager.staticTheme[11];
        textPause = Manager.staticTheme[12];

        start = true;
	}
	
    void Update()
    {
        if (animate) {
            if (start) {
                crossLine1.color = Color.Lerp(colorRegular, colorPause, t);
                crossLine2.color = Color.Lerp(colorRegular, colorPause, t);

                gemCount.color = Color.Lerp(textRegular, textPause, t);
                plusSign.color = Color.Lerp(textRegular, textPause, t);
            }
            else
            {
                crossLine1.color = Color.Lerp(colorPause, colorRegular, t);
                crossLine2.color = Color.Lerp(colorPause, colorRegular, t);

                gemCount.color = Color.Lerp(textPause, textRegular, t);
                plusSign.color = Color.Lerp(textPause, textRegular, t);
            }

            if (t < 1)
            { 
                t += Time.deltaTime / duration;
            }
        } 
    }

    public void ResetT () {
        t = 0;
    }
}
