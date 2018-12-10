using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class NormalEnemyAnim : MonoBehaviour {

	public UnityArmatureComponent Anim;
	[Header("Collison Check")]
	public float CollisionRadius;
	public LayerMask CollisionLayerMask;
	[Header("")]
	public bool isAttack = false;

	private string currentAnimName = "Idle";

	private void Awake() {
		Anim = GetComponent<UnityArmatureComponent>();
		Anim.animation.Play("Idle");
	}

	private void Update() {
		CheckCollision();
		// Debug.Log(Anim.animation.lastAnimationName);
		Vector2 velocity = GetComponent<PolyNavAgent>().GetNormalizedVelocity();
		if (velocity.x == 0 && velocity.y == 0) {
			if (currentAnimName == "Move" && !isAttack) {
				AnimPlay("Idle");
				return;
			}
		}
		else if (velocity.x != 0) {
			if (velocity.x > 0.1f) {
				Anim.armature.flipX = true;
			}
			if (velocity.x < -0.1f) {
				Anim.armature.flipX = false;
			}
			if (currentAnimName == "Idle" && !isAttack) {
				AnimPlay("Move");
				return;
			}
		}
		else {
			if (currentAnimName == "Idle" && !isAttack) {
				AnimPlay("Move");
				return;
			}
		}
	}

	private void CheckCollision() {
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, CollisionRadius, CollisionLayerMask);
		int sourtingOrder_other = 0;
		foreach (var col in cols) {
			if (col != null && col != this.gameObject) {
				if (col.tag == "Map") {
					sourtingOrder_other = col.GetComponent<SpriteRenderer>().sortingOrder;
				}
				else if (col.tag == "Player") {
					sourtingOrder_other = col.GetComponent<PlayerAnim>().Anim.sortingOrder;
				}
				else if (col.tag == "Enemy") {
					sourtingOrder_other = col.GetComponent<NormalEnemyAnim>().Anim.sortingOrder;
				}
				else if (col.tag == "Boss") {
					sourtingOrder_other = col.GetComponent<QueenAnim>().Anim.sortingOrder;
				}

				if (col.transform.position.y > transform.position.y) {
					Anim.sortingOrder = sourtingOrder_other + 1;
					return;
				}
			}
		}
		if (cols.Length == 0) {
			Anim.sortingOrder = 0;
		}
	}

	public void AnimPlay(string name) {
		Anim.animation.Stop(currentAnimName);
		Anim.animation.Play(name);
		currentAnimName = name;
	}

	void OnDrawGizmos(){
		Gizmos.color = new Color(0,1,0,1);
		Gizmos.DrawWireSphere(transform.position, CollisionRadius);
    }
}
