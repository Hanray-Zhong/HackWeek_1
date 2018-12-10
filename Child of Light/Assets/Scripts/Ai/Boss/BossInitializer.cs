using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInitializer : MonoBehaviour {
	public List<Vector2> InitialPoints;
	public GameObject BossPrefab;
	public GameObject Player;
	public float CreatBossTime;
	private bool haveBoss = false;


	private void Update() {
		if (Player != null && Player.GetComponent<PlayerUnit>().FirstFindSpring && !haveBoss) {
			StartCoroutine(CreatEnemy());
			haveBoss = true;
		}
	}

	IEnumerator CreatEnemy() {
		yield return new WaitForSeconds(CreatBossTime);
		Instantiate(BossPrefab, GetRandomPoint(), BossPrefab.transform.rotation).transform.parent = transform;
	}

	public void CreatBossForButton() {
		Instantiate(BossPrefab, GetRandomPoint(), BossPrefab.transform.rotation).transform.parent = transform;
	}

	private Vector2 GetRandomPoint() {
		Vector2 currentInitialPoint;
		currentInitialPoint = InitialPoints[Random.Range(0, InitialPoints.Count)];
		return currentInitialPoint;
	}

	private void OnDrawGizmos() {
		foreach (var item in InitialPoints)
		{
			Gizmos.color = new Color(0f, 0f, 1f, 1f);
			Gizmos.DrawSphere((Vector3)item, 0.2f);
		}
	}
}
