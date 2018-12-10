using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitializer : MonoBehaviour {
	public List<Vector2> InitialPoints;
	public GameObject EnemyPrefab;

	private void Awake() {
		StartCoroutine(CreatEnemy());
	}

	IEnumerator CreatEnemy() {
		yield return new WaitForSeconds(10);
		Instantiate(EnemyPrefab, GetRandomPoint(), EnemyPrefab.transform.rotation).transform.parent = transform;
		StartCoroutine(CreatEnemy());
	}

	public void CreatEnemyForButton() {
		Instantiate(EnemyPrefab, GetRandomPoint(), EnemyPrefab.transform.rotation).transform.parent = transform;
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
