using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bosses : MonoBehaviour {

	[Header("Search State Paramaters")]
	public float MIN_SearchRadius;
	public float searchRadius;
	public float SearchRadiusChangeConstant;
	[Header("Attack")]
	public float PushForce;
	public int AttackValue;
	public BossSkill bossSkill;
	[Header("State")]
	public States CurrentState = States.PatrolState;
	[Header("Others")]
	public GameObject Player;
	public GameObject Main_Camera;
	public PolyNavAgent NavAgent;

	private QueenAnim Anim;
	private LayerMask playerLayer;
	private GameObject target;
	private FogOfWar lightController;

	private void Awake() {
		bossSkill = GetComponent<BossSkill>();
		NavAgent = GetComponent<PolyNavAgent>();
		Player = GameObject.FindGameObjectWithTag("Player");
		Anim = GetComponent<QueenAnim>();
		Main_Camera = GameObject.FindGameObjectWithTag("MainCamera");
		playerLayer = LayerMask.NameToLayer("Player");
		lightController = Main_Camera.GetComponent<FogOfWar>();
		StartCoroutine(NormalAttack());
		StartCoroutine(UseSkill());
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
			NavAgent.maxSpeed = 6f;
			ChangeState(States.AttackState);
		}
		if (CurrentState == States.AttackState && col == null) {
			// Debug.Log("丢失玩家");
			target = null;
			StopCoroutine(NormalAttack());
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
		if (CurrentState == States.AttackState) {
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

	private void Attack(GameObject target) {
		PlayerUnit u = target.GetComponent<PlayerUnit>();
		if (u != null) {
			u.Damage(AttackValue);
		}
		target.GetComponent<Rigidbody2D>().AddForce((target.transform.position - transform.position) * PushForce, ForceMode2D.Impulse);
	}

	/******************** 技能系统 ******************/
	IEnumerator NormalAttack() {
		yield return new WaitForSeconds(bossSkill.NormalAttackCD);
		// Debug.Log("Normal");
		if (CurrentState == States.AttackState && !Anim.isAttack) {
			bossSkill.UseSkills("NormalAttack");
			Anim.AnimPlay("Skill2");
			Anim.isAttack = true;
			NavAgent.maxSpeed = 0.1f;
			StartCoroutine(AttackColdDown());
		}
		StartCoroutine(NormalAttack());
	}
	IEnumerator UseSkill() {
		yield return new WaitForSeconds(bossSkill.SpecialSkillCD);
		// Debug.Log("Special!");
		if (CurrentState == States.AttackState && !Anim.isAttack) {
			bossSkill.UseSkills("HeartFallDown");
			Anim.AnimPlay("Skill1");
			Anim.isAttack = true;
			NavAgent.maxSpeed = 0.1f;
			StartCoroutine(AttackColdDown());
		}
		StartCoroutine(UseSkill());
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			Attack(other.gameObject);
		}
	}

	void OnDrawGizmos(){
    	Gizmos.color = new Color(1,0,0,1);
    	Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

	IEnumerator AttackColdDown() {
		yield return new WaitForSeconds(1f);
		Anim.isAttack = false;
		NavAgent.maxSpeed = 6f;
		Anim.AnimPlay("Move");
	}
}
