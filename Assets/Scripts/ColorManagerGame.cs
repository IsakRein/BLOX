using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorManagerGame : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public List<GameObject> pausedGameObjects = new List<GameObject>();
    public List<GameObject> pausedGameObjectsChildren = new List<GameObject>();

    public List<Sprite> powerUps = new List<Sprite>();

    public List<GameObject> continueButtons = new List<GameObject>();

    private void Start()
    {
        LoadThemeColors();
    }

    public void LoadThemeColors()
    {
        gameObjects[0].GetComponent<Camera>().backgroundColor = Manager.staticTheme[6];
        gameObjects[1].GetComponent<Image>().color = Manager.staticTheme[7];
        gameObjects[2].GetComponent<Image>().color = Manager.staticTheme[8];
        gameObjects[3].GetComponent<Image>().color = Manager.staticTheme[9];
        gameObjects[4].GetComponent<Image>().color = Manager.staticTheme[9];
        gameObjects[5].GetComponent<Image>().color = Manager.staticTheme[9];
        gameObjects[6].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[11];
        gameObjects[7].GetComponent<Text>().color = Manager.staticTheme[11];
        gameObjects[8].GetComponent<Image>().color = Manager.staticTheme[14];

        if (Manager.staticTheme[16].r == 0)
        {
            gameObjects[9].GetComponent<Image>().sprite = powerUps[0];
            gameObjects[10].GetComponent<Image>().sprite = powerUps[0];
            gameObjects[11].GetComponent<Image>().sprite = powerUps[0];
            gameObjects[12].GetComponent<Image>().sprite = powerUps[0];
        }
        else
        {
            gameObjects[9].GetComponent<Image>().sprite = powerUps[1];
            gameObjects[10].GetComponent<Image>().sprite = powerUps[1];
            gameObjects[11].GetComponent<Image>().sprite = powerUps[1];
            gameObjects[12].GetComponent<Image>().sprite = powerUps[1];
        }

        gameObjects[13].GetComponent<SpriteRenderer>().color = Manager.staticTheme[17];
        gameObjects[14].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[18];
        gameObjects[15].GetComponent<SpriteRenderer>().color = Manager.staticTheme[19];
        gameObjects[16].GetComponent<Image>().color = Manager.staticTheme[20];
        gameObjects[17].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[21];
        gameObjects[18].GetComponent<Text>().color = Manager.staticTheme[22];

        gameObjects[19].GetComponent<Image>().color = Manager.staticTheme[29];
        gameObjects[20].GetComponent<Image>().color = Manager.staticTheme[29];
        gameObjects[21].GetComponent<Image>().color = Manager.staticTheme[29];

        gameObjects[22].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[30];
        gameObjects[23].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[30];
        gameObjects[24].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[30];
        gameObjects[25].GetComponent<SpriteRenderer>().color = Manager.staticTheme[30];
        gameObjects[26].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[30];

        gameObjects[27].GetComponent<Image>().color = Manager.staticTheme[31];
        gameObjects[28].GetComponent<Image>().color = Manager.staticTheme[31];

        gameObjects[29].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[32];
        gameObjects[30].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[32];

        gameObjects[31].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[33];

        gameObjects[32].GetComponent<SpriteRenderer>().color = Manager.staticTheme[48];

        gameObjects[33].GetComponent<Image>().color = Manager.staticTheme[9];
        gameObjects[34].GetComponent<TextMeshProUGUI>().color = Manager.staticTheme[30];

        foreach (GameObject obj in pausedGameObjects) {
            obj.GetComponent<Image>().color = Manager.staticTheme[13];
        }

        foreach (GameObject obj in pausedGameObjectsChildren)
        {
            if (obj.GetComponent<SpriteRenderer>()) 
            {
                obj.GetComponent<SpriteRenderer>().color = Manager.staticTheme[15];
            }

            else if (obj.GetComponent<Image>())
            {
                obj.GetComponent<Image>().color = Manager.staticTheme[15];
            }

            else {
                obj.GetComponent<Text>().color = Manager.staticTheme[15];
            }
        }
    }
}
