using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour {

	public float LightMaxStrength = 0.5f;
	public GameObject main_camera;

	public float decreaseSpeed;
	public float increaseSpeed;

	private FogOfWar LightController;
	private float button_a;

	private void Awake() {
		LightController = main_camera.GetComponent<FogOfWar>();
	}

	private void Update() {

		button_a = Input.GetAxis("Button_A");

		if (button_a < 1 && LightController != null) {
			if (LightController.Lerp > 0.15) {
				// 光衰弱
				LightController.Lerp -= decreaseSpeed * Time.deltaTime;
			}
		}
		if (button_a >= 1 && LightController != null) {
			if (LightController.Lerp < 0.5f) {
				// 光增强
				LightController.Lerp += increaseSpeed * Time.deltaTime;
			}
		}
	}
}
