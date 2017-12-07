using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	static SceneManager Instance;

	void Start () {
		if (Instance != null)
		{
			GameObject.Destroy (gameObject);
		}
		else
		{
			GameObject.DontDestroyOnLoad (gameObject);
			Instance = this;
		}
	}

    public void LoadScene (string scene) {
        Application.LoadLevel (scene);
	}
}
