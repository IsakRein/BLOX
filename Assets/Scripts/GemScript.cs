using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemScript : MonoBehaviour {

    private TextMeshProUGUI text;
    public GameObject plus;
    public bool usePlus;

	void Start () {
        text = gameObject.GetComponent<TextMeshProUGUI>();

        UpdateValue();
	}

    public void UpdateValue() {
        text.SetText(Manager.gemCount.ToString());

        if (usePlus) {
            if (Manager.gemCount < 10)
            {
                plus.transform.localPosition = new Vector2(-470, 0);
            }
            else if (Manager.gemCount < 100)
            {
                plus.transform.localPosition = new Vector2(-630, 0);
            }
            else
            {
                plus.transform.localPosition = new Vector2(-740, 0);
            }
        }
    }
}
