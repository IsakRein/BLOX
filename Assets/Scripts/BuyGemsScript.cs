using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyGemsScript : MonoBehaviour {

    public TextMeshProUGUI gemText;

    public void BuyGems(int gems) {

        Manager.gemCount += gems;
        Manager.SaveGemCount();

        gemText.SetText(Manager.gemCount.ToString());
    }
}