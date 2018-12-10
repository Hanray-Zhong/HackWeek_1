using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour {

	private UnityArmatureComponent Anim;

	private void Start() {
		Anim = GetComponent<UnityArmatureComponent>();
		Anim.animation.Play("Dead", 1);
		StartCoroutine(Reload());
	}

	IEnumerator Reload() {
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
