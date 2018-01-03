using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScaler : MonoBehaviour {
    public Camera cam;
    public List<Transform> parents = new List<Transform>();

    public float scaleValue;
    public float yValue;

	private CanvasScaler canvas;

	void Start() {
		canvas = gameObject.GetComponent<CanvasScaler> ();
	}

	void Update () {
        if ((float)cam.pixelWidth / (float)cam.pixelHeight > 401f / 663f)
        {
            float scale = (scaleValue / ((float)cam.scaledPixelWidth / (float)cam.scaledPixelHeight)) * 1.1f;
            float yPos = yValue * ((float)cam.scaledPixelWidth / (float)cam.scaledPixelHeight);

			canvas.matchWidthOrHeight = 1;

            gameObject.transform.localScale = new Vector3(scale, scale, 1);

            foreach (Transform parent in parents)
            {
                parent.position = new Vector2(0, yPos);
            }
        }

        else if ((float)cam.pixelWidth / (float)cam.pixelHeight > 9f / 16f)
        {
            float yPos = yValue * ((float)cam.scaledPixelWidth / (float)cam.scaledPixelHeight);

            gameObject.transform.localScale = new Vector3(1, 1, 1);

            foreach (Transform parent in parents)
            {
                parent.position = new Vector2(0, yPos);
            }
        }

        else
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);

            foreach (Transform parent in parents)
            {
                parent.position = new Vector2(0, 0);
            }
        }
	}
}
