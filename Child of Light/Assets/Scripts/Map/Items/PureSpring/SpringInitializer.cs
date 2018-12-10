using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringInitializer : MonoBehaviour {

	public List<Vector2> InitialPoints;
	public GameObject[] SpringPrefabs;
	public GameObject Player;
	public bool SpringExist = false;
	private PlayerUnit u;

	private void Start() {
		u = Player.GetComponent<PlayerUnit>();
	}

	private void Update() {
		if (!SpringExist && u.FirstEscape) {
			StartCoroutine(CreatSpring());
			SpringExist = true;
		}
	}

	IEnumerator CreatSpring() {
		yield return new WaitForSeconds(30);
		GameObject SpringPrefab = SpringPrefabs[Random.Range(0, 1)];
		Instantiate(SpringPrefab, GetRandomPoint(), SpringPrefab.transform.rotation).transform.parent = transform;
	}


	private Vector2 GetRandomPoint() {
		Vector2 currentInitialPoint;
		currentInitialPoint = InitialPoints[Random.Range(0, InitialPoints.Count)];
		return currentInitialPoint;
	}

	private void OnDrawGizmos() {
		foreach (var item in InitialPoints)
		{
			Gizmos.color = new Color(1f, 0f, 0f, 1f);
			Gizmos.DrawSphere((Vector3)item, 0.2f);
		}
	}
}
