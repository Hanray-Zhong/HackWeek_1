using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	private float hl;
	private float vl;
	private float button_a;
	private Vector2 MoveDir;
	public float Speed;
	public float Drag;
	void FixedUpdate () {
		HandleConfiguration ();
		GetMoveDir ();
		StartMove ();
		Lighting ();
	}
	/// <summary>
	/// 手柄配置
	/// </summary>
	void HandleConfiguration () {
		hl = Input.GetAxis("Horizontal");
		vl = Input.GetAxis("Vertical");
		button_a = Input.GetAxis("Button_A");
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
		if ( button_a >= 1 ) {
			Debug.Log( "发光" );
		}
	}
	private void OnCollisionStay2D ( Collision2D other ) {
		if ( other.gameObject.tag == "Map" ) {
			Debug.Log( "Collision" );
		}
	}
}
