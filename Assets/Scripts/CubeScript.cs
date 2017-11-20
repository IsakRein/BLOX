using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {

	private Transform parent;

	void Start () {
		parent = transform.parent;
	}


	void OnTouchDown() {
		parent.SendMessage("OnTouchDown", SendMessageOptions.DontRequireReceiver);
	}

	void OnTouchUp() {
		parent.SendMessage("OnTouchUp", SendMessageOptions.DontRequireReceiver);
	}

	void OnTouchStay() {
		parent.SendMessage("OnTouchStay", SendMessageOptions.DontRequireReceiver);
	}

	void OnTouchExit() {
		parent.SendMessage("OnTouchExit", SendMessageOptions.DontRequireReceiver);
	}
}
