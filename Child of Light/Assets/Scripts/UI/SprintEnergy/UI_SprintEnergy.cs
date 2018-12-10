using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SprintEnergy : MonoBehaviour {
	public Slider SprintEnergy_slider;
	public GameObject Player;
	private void Update() {
		if (Player != null)
			SprintEnergy_slider.value = Player.GetComponent<PlayerController>().SprintEnergy;
	}
}
