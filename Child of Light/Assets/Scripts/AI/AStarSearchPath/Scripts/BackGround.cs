using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameTerrain.GroundMap.InitBackGound(gameObject);
		GameTerrain.GroundMap.RegisterObstaclesToMaTrix("Solid");
	}
	
	// Update is called once per frame
	void Update () {

	}
}
