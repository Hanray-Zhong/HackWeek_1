using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Health1 : MonoBehaviour {

	public GameObject Player;
	public GameObject[] HealthImages;

	private int playerHealth;
	private int max_playerHealth;

	private void Awake() {
		max_playerHealth = Player.GetComponent<PlayerUnit>().MAX_PlayerHealth;
	}

	private void Update() {
		if (Player == null) {
			return;
		}
		int i;
		playerHealth = Player.GetComponent<PlayerUnit>().playerHealth;
		for (i = 0; i <= playerHealth - 1; i ++) {
			HealthImages[i].GetComponent<CanvasGroup>().alpha = 1;
		}
		for (i = playerHealth; i <= HealthImages.Length - 1; i++) {
			HealthImages[i].GetComponent<CanvasGroup>().alpha = 0;
		}
	}
}
