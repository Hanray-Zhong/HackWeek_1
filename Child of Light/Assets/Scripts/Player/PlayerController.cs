﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private float 			hl;
	private float 			vl;
	private float 			Button_A;
	private float 			Button_X;
	private Vector2 		MoveDir;
	public GameObject 		SpreadPS;
	public GameObject 		SprintPS;
	public float Speed;
	public float Drag;
	void FixedUpdate () {
		HandleConfiguration ();
		GetMoveDir ();
		StartMove ();
		Lighting ();
		Sprinting ();
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
			Debug.Log( "发光" );
		}
	}
	/// <summary>
	///	发光
	/// </summary>
	void Sprinting () {
		if ( Button_X >= 1 ) {
			Debug.Log( "冲刺" );
			Speed = 4500;
			var main = SpreadPS.GetComponent< ParticleSystem >().main;
			main.loop = false;
			main = SprintPS.GetComponent< ParticleSystem >().main;
			main.loop = true;
			SprintPS.GetComponent< ParticleSystem >().Play();
		} else {
			Speed = 3000;
			var main = SprintPS.GetComponent< ParticleSystem >().main;
			main.loop = false;
			main = SpreadPS.GetComponent< ParticleSystem >().main;
			main.loop = true;
			SpreadPS.GetComponent< ParticleSystem >().Play();
		}
	}
}
