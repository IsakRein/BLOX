using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemesScript : MonoBehaviour {

    public List<GameObject> themes = new List<GameObject>();
    public List<int> prices = new List<int>();
    public List<Transform> chosens = new List<Transform>();

    public GemScript gemScript;


	private void Start()
	{
        foreach (Transform child in transform)
        {
            child.Find("Chosen").gameObject.SetActive(false);
            child.Find("Notchosen").gameObject.SetActive(false);
            child.Find("Price").gameObject.SetActive(false);
        }

        for (int i = 1; i <= 8; i++)
        {
            if (Manager.boughtThemes[i] == 0) {
                transform.GetChild(i - 1).Find("Price").gameObject.SetActive(true);
                transform.GetChild(i - 1).Find("Notchosen").gameObject.SetActive(false);
            }
            else {
                transform.GetChild(i - 1).Find("Price").gameObject.SetActive(false);
                transform.GetChild(i - 1).Find("Notchosen").gameObject.SetActive(true);
            }
        }

        transform.GetChild(Manager.selectedTheme - 1).Find("Chosen").gameObject.SetActive(true);
    }

    void IdentifyChosen()
    {
        foreach (Transform child in transform)
        {
            chosens.Add(child.Find("Chosen"));
        }
    }

	public void ChangeTheme(int themeNum)
    {
        if (prices[themeNum] < Manager.gemCount) {
            Manager.boughtThemes[themeNum] = 1;
            Manager.SaveBoughtThemes();

            ChargeGems(prices[themeNum]);

            Manager.selectedTheme = themeNum;
            PlayerPrefs.SetInt("Theme", themeNum);
            Manager.UpdateTheme();

            //load scene
            string sceneToLoad = Manager.sceneOrder[Manager.sceneOrder.Count - 1];
            Manager.sceneOrder.RemoveAt(Manager.sceneOrder.Count - 1);
            SceneManager.LoadScene(sceneToLoad);
        }

        else {
            Manager.sceneOrder.Add(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Gems");
        }
    }

    public void ChargeGems(int gems)
    {
        Manager.gemCount -= gems;
        Manager.SaveGemCount();

        gemScript.UpdateValue(); 
    }
}