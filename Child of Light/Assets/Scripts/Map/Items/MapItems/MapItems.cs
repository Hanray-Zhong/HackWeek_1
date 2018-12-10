using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItems : MonoBehaviour {

	[Header("Check Collision")]
	public float CollisionRadius;
	public LayerMask CollisionLayerMask;
	private SpriteRenderer spriteRenderer;

	private void Start() {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

	void Update () {
		CheckCollision();
	}

	private void CheckCollision() {
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, CollisionRadius, CollisionLayerMask);
		
		foreach (var col in cols) {
			if (col != null && col != this.gameObject) {
				int sourtingOrder_other = 0;
				if (col.tag == "Map") {
					sourtingOrder_other = col.GetComponent<SpriteRenderer>().sortingOrder;
				}
				else if (col.tag == "Player") {
					sourtingOrder_other = col.GetComponent<PlayerAnim>().Anim.sortingOrder;
				}
				else if (col.tag == "Enemy") {
					sourtingOrder_other = col.GetComponent<NormalEnemyAnim>().Anim.sortingOrder;
				}
				if (col.transform.position.y > transform.position.y) {
					spriteRenderer.sortingOrder = sourtingOrder_other + 1;
					return;
				}
				else if (col.transform.position.y < transform.position.y) {
					spriteRenderer.sortingOrder = sourtingOrder_other - 1;
					return;
				}
			}
		}
		if (cols.Length == 0) {
			spriteRenderer.sortingOrder = 0;
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawWireSphere(transform.position, CollisionRadius);
	}
}
