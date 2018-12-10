using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public GameObject BlackBackground;

	public void NaxtScene(string SceneName) {
		BlackBackground.SetActive(true);
		StartCoroutine(LoadScene(SceneName));
	}

	IEnumerator LoadScene(string name) {
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(name);
	}
}
