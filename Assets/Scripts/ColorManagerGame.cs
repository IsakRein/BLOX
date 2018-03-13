using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorManagerGame : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();

    private void Start()
    {
        LoadThemeColors();
    }

    public void LoadThemeColors()
    {
        gameObjects[0].GetComponent<Camera>().backgroundColor = Manager.staticTheme[6];
        gameObjects[1].GetComponent<Image>().color = Manager.staticTheme[7];
        gameObjects[2].GetComponent<Image>().color = Manager.staticTheme[8];
    }
}
