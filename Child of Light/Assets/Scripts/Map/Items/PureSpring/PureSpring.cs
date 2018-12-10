using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureSpring : MonoBehaviour {
	public int AttackValue;
	public float OverLapRadius;
	public GameObject SpringInitializer;

	private void Start() {
		SpringInitializer = GameObject.FindGameObjectWithTag("SpringInitializer");
	}

	private void Update() {
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, OverLapRadius, 1 << LayerMask.NameToLayer("Player"));
		foreach (var item in cols) {
			PlayerUnit u = item.GetComponent<PlayerUnit>();
			if (u != null) {
				u.FirstFindSpring = true;
			}
		}
	}

	private void Purify(GameObject target) {
		BossUnit u = target.GetComponent<BossUnit>();
		if (u != null) {
			u.Damage(AttackValue);
		}
		target.GetComponent<Rigidbody2D>().AddForce((target.transform.position - transform.position), ForceMode2D.Impulse);
		SpringInitializer.GetComponent<SpringInitializer>().SpringExist = false;
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Boss")) {
			Purify(other.gameObject);
			return;
		}
		if (other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);
			return;
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color(0, 1, 0, 1);
		Gizmos.DrawWireSphere(transform.position, OverLapRadius);
	}
	
}
