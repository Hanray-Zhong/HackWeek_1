using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerController : MonoBehaviour {
	private float 			hl;
	private float 			vl;
	private float 			Button_A;
	private float 			Button_X;
	private float 			MAX_SprintEnergy = 100;
	private bool 			SERecoverEnabled = true;
	private Vector2 		MoveDir;
	[Header("Animation")]
	public UnityArmatureComponent Anim;
	[Header("Particle System")]
	public GameObject 		SpreadPS;
	public GameObject 		SprintPS;
	[Header("Move")]
	public float Speed;
	public float Drag;
	[Header("Sprint")]
	public float SprintEnergy = 100;
	public float SprintEnergyDec;
	public float SprintEnergyInc;
	void FixedUpdate () {
		HandleConfiguration ();
		GetMoveDir ();
		StartMove ();
		Lighting ();
		Sprinting ();
		SprintEnergyRevover();
	}
	/// <summary>
	/// 手柄配置
	/// </summary>
	void HandleConfiguration () {
		hl = Input.GetAxis( "Horizontal" );
		vl = Input.GetAxis( "Vertical" );
		Button_A = Input.GetAxis( "Button_A" );
		Button_X = Input.GetAxis( "Button_X" );
	}
	/// <summary>
	/// 获取移动方向
	/// </summary>
	void GetMoveDir () {
		MoveDir = new Vector2( 0 , 0 );
		if ( Mathf.Abs( hl ) > 0.05f || Mathf.Abs( vl ) > 0.05f) {
			MoveDir = new Vector2( hl , vl ).normalized;
		}
	}
	/// <summary>
	/// 开始移动
	/// </summary>
	void StartMove () {
		this.gameObject.GetComponent< Rigidbody2D >().drag = Drag;
		this.gameObject.GetComponent< Rigidbody2D >().AddForce( MoveDir * Speed * Time.deltaTime );
	}
	/// <summary>
	///	发光
	/// </summary>
	void Lighting () {
		if ( Button_A >= 1 ) {

		}
	}
	/// <summary>
	///	冲刺
	/// </summary>
	void Sprinting () {
		if ( Button_X >= 1 && SprintEnergy > 0 ) {
			Speed = 4500;
			Anim.animation.timeScale = 2.25f;
			var main = SpreadPS.GetComponent< ParticleSystem >().main;
			main.loop = false;
			main = SprintPS.GetComponent< ParticleSystem >().main;
			main.loop = true;
			SprintPS.GetComponent< ParticleSystem >().Play();
			SprintEnergy -= SprintEnergyDec * Time.deltaTime;
			if (SprintEnergy <= 0) {
				SERecoverEnabled = false;
			}
		} else {
			Speed = 3000;
			Anim.animation.timeScale = 1.5f;
			var main = SprintPS.GetComponent< ParticleSystem >().main;
			main.loop = false;
			main = SpreadPS.GetComponent< ParticleSystem >().main;
			main.loop = true;
			SpreadPS.GetComponent< ParticleSystem >().Play();
		}
	}
	/// <summary>
	/// 回复冲刺条
	/// </summary>
	void SprintEnergyRevover() {
		if (Button_X <= 0.1 && SERecoverEnabled && SprintEnergy <= MAX_SprintEnergy) {
			SprintEnergy += SprintEnergyInc * Time.deltaTime;
		}
		if (!SERecoverEnabled && Button_X <= 0.1) {
			StartCoroutine(StartRecover());
		}
	}

	IEnumerator StartRecover() {
		yield return new WaitForSeconds(3f);
		SERecoverEnabled = true;
		StopAllCoroutines();
	}
}
