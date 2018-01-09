using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateGameObject : MonoBehaviour {

    public void Disable() {
        gameObject.SetActive(false);
    }

}
