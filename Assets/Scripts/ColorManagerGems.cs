using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorManagerGems : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public List<GameObject> buttons1 = new List<GameObject>();
    public List<GameObject> buttons2 = new List<GameObject>();
    public List<GameObject> buttons3 = new List<GameObject>();

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
        gameObjects[4].GetComponent<Image>().color = Manager.staticTheme[38];

        foreach (GameObject item in buttons1)
        {
            item.GetComponent<Image>().color = Manager.staticTheme[39];
        }

        foreach (GameObject item in buttons2)
        {
            item.GetComponent<Image>().color = Manager.staticTheme[40];
        }

        foreach (GameObject item in buttons3)
        {
            item.GetComponent<Image>().color = Manager.staticTheme[41];
        }

        gameObjects[5].GetComponent<Image>().color = Manager.staticTheme[42];
        gameObjects[6].GetComponent<Image>().color = Manager.staticTheme[43];
    }
}