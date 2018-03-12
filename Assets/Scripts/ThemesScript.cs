using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemesScript : MonoBehaviour {

    public List<GameObject> themes = new List<GameObject>();
    public List<Transform> chosens = new List<Transform>();


	private void Start()
	{
        foreach (Transform child in transform)
        {
            child.Find("Chosen").gameObject.SetActive(false);
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
        Manager.selectedTheme = themeNum;
        PlayerPrefs.SetInt("Theme", themeNum);

        foreach (Transform child in chosens)
        {
            child.gameObject.SetActive(false);
        }

        transform.GetChild(themeNum - 1).Find("Chosen").gameObject.SetActive(true);

        //load scene
        string sceneToLoad = Manager.sceneOrder[Manager.sceneOrder.Count - 1];
        Manager.sceneOrder.RemoveAt(Manager.sceneOrder.Count - 1);
        SceneManager.LoadScene(sceneToLoad);
    }
}
