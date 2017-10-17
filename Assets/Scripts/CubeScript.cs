using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {

	void OnTouchDown() {
		transform.parent.SendMessage("OnTouchDown", SendMessageOptions.DontRequireReceiver);
	}

	void OnTouchUp() {
		transform.parent.SendMessage("OnTouchUp", SendMessageOptions.DontRequireReceiver);
	}

	void OnTouchStay() {
		transform.parent.SendMessage("OnTouchStay", SendMessageOptions.DontRequireReceiver);
	}

	void OnTouchExit() {
		transform.parent.SendMessage("OnTouchExit", SendMessageOptions.DontRequireReceiver);
	}
}
