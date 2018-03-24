using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorManagerThemes : MonoBehaviour {

    public List<GameObject> gameObjects = new List<GameObject>();

    private void Start()
    {
        LoadThemeColors();
    }

    public void LoadThemeColors()
    {
        gameObjects[0].GetComponent<Camera>().backgroundColor = Manager.staticTheme[34];
        gameObjects[1].GetComponent<Image>().color = Manager.staticTheme[35];
        gameObjects[2].GetComponent<Image>().color = Manager.staticTheme[36];
        gameObjects[3].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[37];
        gameObjects[4].GetComponent<Text>().color = Manager.staticTheme[37];
        gameObjects[5].GetComponent<Image>().color = Manager.staticTheme[38];
    }
}
