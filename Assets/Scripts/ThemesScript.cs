using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemesScript : MonoBehaviour {

    public List<GameObject> themes = new List<GameObject>();
	
	public void ChangeTheme(int themeNum)
    {
        Manager.selectedTheme = themeNum;
    }
}
