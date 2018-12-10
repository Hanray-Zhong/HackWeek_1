using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour {

	public int MAX_PlayerHealth;
	public int playerHealth;
	public GameObject PlayerDeathPrefab;
	public GameObject GameOverCG;
	public float xOffset;
	[Header("UI Text")]
	public bool FirstBeFound = false;
	public bool FirstEscape = false;
	public bool FirstFindSpring = false;
	public bool FirstFindBoss = false;
	public GameObject FirstBeFoundText;
	public GameObject FirstEscapeText;
	public GameObject FirstFindSpringText;
	public GameObject FirstFindBossText;
	public GameObject FinalText;

	private void Awake() {
		playerHealth = MAX_PlayerHealth;
	}

	private void Update() {
		if (playerHealth <= 0) {
			// death
			Debug.Log("You Die!");
			Instantiate(PlayerDeathPrefab, (Vector2)transform.position + new Vector2(xOffset, 0), new Quaternion());
			GameOverCG.SetActive(true);
			Destroy(this.gameObject);
		}
		if (FirstBeFoundText != null && FirstBeFound) {
			FirstBeFoundText.SetActive(true);
		}
		if (FirstEscapeText != null && FirstEscape) {
			FirstEscapeText.SetActive(true);
		}
		if (FirstFindSpringText != null && FirstFindSpring) {
			FirstFindSpringText.SetActive(true);
		}
		if (FirstFindBossText != null && FirstFindBoss) {
			FirstFindBossText.SetActive(true);
		}
	}

	public void Damage(int damage) {
		playerHealth -= damage;
	}
}
