using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public static Manager manager;

	void Start () {
		DontDestroyOnLoad(transform.gameObject);
	}	
}
