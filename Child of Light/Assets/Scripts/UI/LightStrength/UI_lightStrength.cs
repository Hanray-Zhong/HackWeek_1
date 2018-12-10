using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_lightStrength : MonoBehaviour {

	public GameObject main_camera;
	public Slider lightStrength_Slider;
	private FogOfWar LightController;

	private void Awake() {
		LightController = main_camera.GetComponent<FogOfWar>();
	}

	private void Update() {
		lightStrength_Slider.value = (LightController.Lerp * 200);
	}
}
