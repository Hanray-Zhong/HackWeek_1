using UnityEngine;
using System.Collections;

public abstract class GameController : MonoBehaviour {
	public abstract void FixedUpdateGame();//Like FixedUpdate function
	public abstract void UpdateGame();//Like Update function
	public abstract void LateUpdateGame();//Like LateUpdate function

}
