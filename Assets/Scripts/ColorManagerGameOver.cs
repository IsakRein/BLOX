using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorManagerGameOver : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public List<GameObject> buttons1 = new List<GameObject>();

    private void Start()
    {
        LoadThemeColors();
    }

    public void LoadThemeColors()
    {
        gameObjects[0].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[44];
        gameObjects[1].GetComponent<Image>().color = Manager.staticTheme[45];
        gameObjects[2].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[44];
        gameObjects[3].GetComponent<Image>().color = Manager.staticTheme[46];
        gameObjects[4].GetComponent<Camera>().backgroundColor = Manager.staticTheme[34];


        foreach (GameObject item in buttons1)
        {
            item.GetComponent<Image>().color = Manager.staticTheme[47];
        }
    }
}