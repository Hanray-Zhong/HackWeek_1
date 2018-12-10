using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradualChange : MonoBehaviour {

	private CanvasGroup canvasGroup;
	private float _deltaTime;
	private float _timeAtLastFrame;
	public float changeSpeed;
	public bool CanClose = false;

	void Awake () {
		canvasGroup = gameObject.GetComponent<CanvasGroup>();
	}

	void Update () {
		if (canvasGroup.alpha < 1 && !CanClose) {
			Open();
		}
		if (CanClose) {
			Close();
		}
	}

	private void Open() {
		canvasGroup.alpha += changeSpeed / 50;
	}

	private void Close() {
		canvasGroup.alpha -= changeSpeed / 50;
	}
}
