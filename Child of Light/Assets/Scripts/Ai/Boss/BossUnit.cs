using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossName {
	QueenOfHeart,
}

public class BossUnit : MonoBehaviour {
	[Header("Boss Name")]
	public BossName name;
	[Header("Health")]
	public int MAX_BossHealth;
	public int BossHealth;
	public GameObject Player;

	private void Awake() {
		BossHealth = MAX_BossHealth;
		Player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update() {
		if (BossHealth <= 0 && Player.GetComponent<PlayerUnit>().FinalText != null) {
			// death
			Debug.Log("Boss Die!");
			Player.GetComponent<PlayerUnit>().FinalText.SetActive(true);
		}
	}

	public void Damage(int damage) {
		BossHealth -= damage;
	}

}
