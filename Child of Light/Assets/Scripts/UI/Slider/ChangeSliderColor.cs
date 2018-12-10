using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSliderColor : MonoBehaviour {
	public Image slider_BG;
	public Image fill;
	public float constant;
	private Color changing_color = new Color(1, 1, 1, 1);
	private Slider slider;

	private void Awake() {
		slider = GetComponent<Slider>();
	}
	
	void Update () {
		changing_color.g = slider.value * constant;
		changing_color.b = slider.value * constant;
		slider_BG.color = changing_color;
		fill.color = changing_color;
	}
}
