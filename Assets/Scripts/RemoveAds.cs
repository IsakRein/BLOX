using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAds : MonoBehaviour {

    public GameObject price;
    public GameObject purchased;

    private Button button;

	private void Start()
	{
        button = gameObject.GetComponent<Button>();

        if (PlayerPrefs.HasKey("boughtRemoveAds")) {
            if (PlayerPrefs.GetInt("boughtRemoveAds") == 1) 
            {
                price.gameObject.SetActive(false);
                purchased.gameObject.SetActive(true);
                button.interactable = false;
            }

            else {
                price.gameObject.SetActive(true);
                purchased.gameObject.SetActive(false); 
                button.interactable = true;
            }
        }
	}

	public void Purchase()
    {
        //purchase
        PlayerPrefs.SetInt("showAds", 0);
        PlayerPrefs.SetInt("boughtRemoveAds", 1);

        button.interactable = false;

        price.gameObject.SetActive(false);
        purchased.gameObject.SetActive(true);
    }
}
