using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour {
	
	public float RotateSpeed;
	public int Damage;

	void Update () {
		transform.Rotate(0, 0, RotateSpeed * Time.deltaTime, Space.Self);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Map") {
			// destroy
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "Player") {
			PlayerUnit u = other.gameObject.GetComponent<PlayerUnit>();
			if (u != null) {
				u.Damage(Damage);
				Destroy(gameObject);
			}
		}
		
		Destroy(gameObject, 2f);
	}
}
