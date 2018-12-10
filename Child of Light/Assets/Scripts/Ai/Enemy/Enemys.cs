using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public enum States {
	PatrolState,
	ChaseState,
	ReturnState,
	AttackState,
}

public class Enemys : MonoBehaviour {
	
	[Header("Search State Paramaters")]
	public float MIN_SearchRadius;
	public float searchRadius;
	public float SearchRadiusChangeConstant;
	[Header("Attack")]
	public float PushForce;
	public int AttackValue;
	[Header("State")]
	public States CurrentState = States.PatrolState;
	[Header("Others")]
	public GameObject Player;
	public GameObject Main_Camera;
	public PolyNavAgent NavAgent;

	private NormalEnemyAnim Anim;
	private LayerMask playerLayer;
	private GameObject target;
	private FogOfWar lightController;

	private void Awake() {
		NavAgent = GetComponent<PolyNavAgent>();
		Player = GameObject.FindGameObjectWithTag("Player");
		Main_Camera = GameObject.FindGameObjectWithTag("MainCamera");
		Anim = GetComponent<NormalEnemyAnim>();
		playerLayer = LayerMask.NameToLayer("Player");
		lightController = Main_Camera.GetComponent<FogOfWar>();
		GetComponent<NormalEnemyAnim>().enabled = true;
	}

	private void Update() {
		ChangeSearchRadius();
		CheckStateChange();
		RunFSM();
	}

	void ChangeSearchRadius() {
		if (lightController.Lerp * SearchRadiusChangeConstant >= MIN_SearchRadius)
			searchRadius = lightController.Lerp * SearchRadiusChangeConstant;
		else 
			searchRadius = MIN_SearchRadius;
	}


	/**************** Simple FSM ****************/
	void CheckStateChange() {
		Collider2D col = Physics2D.OverlapCircle((Vector2)transform.position, searchRadius, 1 << playerLayer);
		if (CurrentState == States.PatrolState && col != null) {
			// Debug.Log("发现玩家");
			GetComponent<MoveBetween>().StopAllCoroutines();
			target = col.gameObject;
			target.GetComponent<PlayerUnit>().FirstBeFound = true;
			if (gameObject.layer == LayerMask.NameToLayer("Boss"))
				target.GetComponent<PlayerUnit>().FirstFindBoss = true;
			NavAgent.maxSpeed = 6f;
			ChangeState(States.ChaseState);
		}
		if (CurrentState == States.ChaseState && col == null) {
			// Debug.Log("丢失玩家");
			target.GetComponent<PlayerUnit>().FirstEscape = true;
			target = null;
			ChangeState(States.PatrolState);
			NavAgent.maxSpeed = 3.5f;
			GetComponent<MoveBetween>().StartCoroutine(GetComponent<MoveBetween>().OnDestinationReached());
		}
	}
	void ChangeState(States nextState) {
		CurrentState = nextState;
	}
	void RunFSM() {
		if (CurrentState == States.PatrolState) {
			GetComponent<MoveBetween>().enabled = true;
		}
		if (CurrentState == States.ChaseState) {
			GetComponent<MoveBetween>().enabled = false;
			if (target != null) {
				NavAgent.SetDestination(target.transform.position);
			}
			else {
				Debug.Log("Error : The target is miss.");
			}
		}
	}
	/**************** Simple FSM ****************/

	public void Attack(GameObject target) {
		PlayerUnit u = target.GetComponent<PlayerUnit>();
		if (u != null) {
			u.Damage(AttackValue);
		}
		target.GetComponent<Rigidbody2D>().AddForce((target.transform.position - transform.position) * PushForce, ForceMode2D.Impulse);
	}

	/// <summary>
	/// 调整物体显示层级，从纵坐标考虑
	/// </summary>
	// private void CheckCollision() {
	// 	GameObject other = Physics2D.OverlapCircle(transform.position, CollisionRadius, 1 << LayerMask.NameToLayer("Enemy")).gameObject;
	// 	SpriteRenderer spriteRenderer_self;
	// 	SpriteRenderer spriteRenderer_other;
	// 	spriteRenderer_self = gameObject.GetComponent<SpriteRenderer>();
	// 	spriteRenderer_other = other.GetComponent<SpriteRenderer>();
	// 	if (other != null && other != this.gameObject) {
	// 		if (other.transform.position.y > transform.position.y) {
	// 			spriteRenderer_self.sortingOrder = spriteRenderer_other.sortingOrder + 1;
	// 		}
	// 		else {
	// 			spriteRenderer_other.sortingOrder = spriteRenderer_self.sortingOrder + 1;
	// 		}
	// 	}
	// 	else {
	// 		spriteRenderer_self.sortingOrder = 0;
	// 	}
	// }

	// Attack
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log("Attack");
			Anim.AnimPlay("Attack");
			Attack(other.gameObject);
			Anim.isAttack = true;
			NavAgent.maxSpeed = 0.5f;
			StartCoroutine(AttackColdDown());
		}
	}
	// private void OnCollisionExit2D(Collision2D other) {
	// 	if (other.gameObject.tag == "Player")
	// 		Anim.AnimPlay("Move");
	// }
	

	void OnDrawGizmos(){
    	Gizmos.color = new Color(1,0,0,1);
    	Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

	IEnumerator AttackColdDown() {
		yield return new WaitForSeconds(0.5f);
		Anim.isAttack = false;
		NavAgent.maxSpeed = 6f;
		Anim.AnimPlay("Move");
	}
}
