using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SprintEnergy : MonoBehaviour {
	public Text SprintEnergy_txt;
	public GameObject Player;
	private void Update() {
		SprintEnergy_txt.text = Player.GetComponent<PlayerController>().SprintEnergy.ToString();
	}
}
