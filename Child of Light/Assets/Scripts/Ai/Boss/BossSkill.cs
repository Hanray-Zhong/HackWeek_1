using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour {
	[Header("Target")]
	public GameObject Target;
	[Header("SkillsCD")]
	public float NormalAttackCD;
	public float SpecialSkillCD;
	[Header("QueenOfHeart")]
	public GameObject cardPrefab;
	public float Q_NormalAttackForce;
	public float HeartHeight;
	public GameObject HeartPrefab;
	public GameObject HeartPartPrefab;
	public float Q_SpecialSkillForce;
	[Header("Other")]
	private BossUnit unit;

	private void Awake() {
		unit = GetComponent<BossUnit>();
		Target = GameObject.FindGameObjectWithTag("Player");
	}

	public void UseSkills(string name) {
		if (name == "NormalAttack" && unit.name == BossName.QueenOfHeart && Target != null && cardPrefab != null) {
			// Debug.Log("normal attack");
			GameObject Card_1 = Instantiate(cardPrefab, transform.position, new Quaternion(0, 0, 0, 0));
			GameObject Card_2 = Instantiate(cardPrefab, transform.position, new Quaternion(0, 0, 0, 0));
			GameObject Card_3 = Instantiate(cardPrefab, transform.position, new Quaternion(0, 0, 0, 0));
			Vector2 normal_Dir = (Target.transform.position - transform.position).normalized;
			Vector2 left_Dir = RotateVector2(normal_Dir, Mathf.PI / 6);
			Vector2 right_Dir = RotateVector2(normal_Dir, -Mathf.PI / 6);
			Card_1.GetComponent<Rigidbody2D>().AddForce(normal_Dir * Q_NormalAttackForce, ForceMode2D.Impulse);
			Card_2.GetComponent<Rigidbody2D>().AddForce(left_Dir * Q_NormalAttackForce, ForceMode2D.Impulse);
			Card_3.GetComponent<Rigidbody2D>().AddForce(right_Dir * Q_NormalAttackForce, ForceMode2D.Impulse);
		}
		if (name == "HeartFallDown" && Target != null && HeartPrefab != null) {
			Debug.Log("Heart Fall Down");
			GameObject HeartPart = Instantiate(HeartPartPrefab, (Vector2)Target.transform.position + new Vector2(0, -2), new Quaternion(0, 0, 0, 0));
			StartCoroutine(HeartFallDown(HeartPart));
		}
	}

	private Vector2 RotateVector2(Vector2 dir, float theta) {
		float omega = Mathf.Atan2(dir.y, dir.x);
		float angle = omega + theta;
		Vector2 new_Dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		return new_Dir;
	}

	IEnumerator HeartFallDown(GameObject heartPart) {
		yield return new WaitForSeconds(1f);
		GameObject Heart = Instantiate(HeartPrefab, (Vector2)heartPart.transform.position + new Vector2(0, HeartHeight), new Quaternion(0, 0, 0, 0));
		Vector2 Dir = new Vector2(0, -1);
		Heart.GetComponent<Rigidbody2D>().AddForce(Dir * Q_SpecialSkillForce, ForceMode2D.Impulse);
	}
}
