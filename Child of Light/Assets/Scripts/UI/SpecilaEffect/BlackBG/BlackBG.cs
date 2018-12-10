using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBG : MonoBehaviour {

	private CanvasGroup canvasGroup;
	public float changeSpeed;
	public bool CanClose = false;
	public bool AlwaysExist;

	void Awake () {
		canvasGroup = gameObject.GetComponent<CanvasGroup>();
	}

	void Update () {
		if (canvasGroup.alpha < 1 && !CanClose) {
			Open();
		}
		if (canvasGroup.alpha >= 1 && !AlwaysExist) {
			CanClose = true;
		}
		if (canvasGroup.alpha > 0 && CanClose) {
			Close();
		}
		if (canvasGroup.alpha == 0 && CanClose) {
			Destroy(gameObject);
		}
	}

	private void Open() {
		canvasGroup.alpha += changeSpeed * Time.deltaTime;
	}

	private void Close() {
		canvasGroup.alpha -= changeSpeed * Time.deltaTime;
	}
}
