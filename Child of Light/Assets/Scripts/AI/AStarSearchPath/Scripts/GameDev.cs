using UnityEngine;
using System.Collections;

public abstract class GameDev : GameController {
	private static bool isStopGame = false; // control object to pause

	public static bool IsStopGame
	{
		get {return GameDev.isStopGame;}
		set {GameDev.isStopGame = value;}
	}

	private static float gameTime = 0; //0 is update normally,x is update per x second (x>0).

	public static float GameTime
	{
		get {return GameDev.gameTime;}
		set {GameDev.gameTime = value;}
	}

	private static float runtime = 0; //Timer
	private bool IsOnTime = false;

	void Update() //update frame
	{
		if(IsOnTime = (IsRun()))
		{
			UpdateGame();
		}
	}

	void FixedUpdate() //FixedUpdate frame
	{
		if(IsOnTime = (IsRun()))
		{
			FixedUpdateGame ();
		}
	}

	void LateUpdate() //LateUpdate frame
	{
		if(IsOnTime)
		{
			LateUpdateGame();
		}
	}

	private bool LateTime() //Should update the frame?
	{
		if(GameTime <= 0 ) 
			return true;
		runtime += Time.fixedDeltaTime;
		if(runtime >= GameTime)
		{
			runtime = 0;
			return true;
		}
		return false;
	}

	private bool IsRun() // Is Object running?
	{
		if(!IsStopGame)
		{
			if(LateTime())//Is time to update when Object is not pause?
			{
				return true;
			}
		}
		return false;
	}

	public override void FixedUpdateGame(){}
	public override void UpdateGame(){}
	public override void LateUpdateGame(){}
}
