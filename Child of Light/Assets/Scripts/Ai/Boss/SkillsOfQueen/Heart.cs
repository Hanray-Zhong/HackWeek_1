using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
	public int Damage;
	public float a;
	public float b;
	public float h;

	private void Update() {
		Destroy(gameObject, 1.5f);
	}


	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "HeartPart") {
			Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)transform.position + new Vector2(0, -h), new Vector2(a, b), 0, 1 << LayerMask.NameToLayer("Player"));
			if (cols.Length != 0) {
				foreach (var col in cols) {
					PlayerUnit u = col.GetComponent<PlayerUnit>();
					u.Damage(1);
				}
			}
			Destroy(other.gameObject);
			Destroy(gameObject);
			Debug.Log("get");
		}	
	}

	private void OnDrawGizmos() {
		Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(0, -h), new Vector3(a, b, 0));
	}
}
