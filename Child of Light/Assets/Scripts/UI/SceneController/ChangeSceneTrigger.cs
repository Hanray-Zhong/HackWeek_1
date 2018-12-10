using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour {

	public GameObject FinalText;
	public GameObject SceneController;
	public string NextSceneName;

	void Update () {
		if (FinalText == null) {
			SceneController.GetComponent<ChangeScene>().NaxtScene(NextSceneName);
		}
	}
}
