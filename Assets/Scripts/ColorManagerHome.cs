using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorManagerHome : MonoBehaviour {
    public List<GameObject> gameObjects = new List<GameObject>();

    public List<GameObject> buttons = new List<GameObject>();

	private void Start()
	{
        LoadThemeColors();
    }

    public void LoadThemeColors() {
        gameObjects[0].GetComponent<Camera>().backgroundColor = Manager.staticTheme[0];
        gameObjects[1].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[1];
        gameObjects[2].GetComponent<Image>().color = Manager.staticTheme[2];
        gameObjects[3].GetComponent<Image>().color = Manager.staticTheme[3];

        foreach (GameObject button in buttons) {
            button.GetComponent<Image>().color = Manager.staticTheme[4];
            button.GetComponentInChildren<SpriteRenderer>().color = Manager.staticTheme[5];
        }
    }
}
