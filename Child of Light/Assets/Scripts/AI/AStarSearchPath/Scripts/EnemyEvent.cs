using UnityEngine;
using System.Collections;

public class EnemyEvent : GameDev {

	public GameObject TargetObject;
	private Vector2 Target;
	public float speed = 0.05f;
	public ArrayList Path = new ArrayList();
	public bool run= true;
	private SearchPath SP = new SearchPath();
	private Vector2 CurrentPosition;
	private float checkTime = 0.0f;

	public void Start()
	{
		CurrentPosition = (Vector2) transform.position;
	}
	// Use this for initialization
	public override void FixedUpdateGame()
	{
		float distance = Vector2.Distance(transform.position, TargetObject.transform.position);
		if(distance <= 0.45f)
			return;
		Vector2 Target = (Vector2)TargetObject.transform.position;
		if(run)
		{
			Path.Clear();
			Path = SP.AStarSearch(new Vector2(gameObject.transform.position.x,transform.position.y) ,Target,false,false);
			run = false;
		}
		if(Path.Count > 0)
		{
			Vector2 move = (Vector2)Path[0];
			gameObject.transform.position = Vector2.MoveTowards((Vector2)gameObject.transform.position,move,speed);
			if((Vector2)transform.position == move)
			{
				Path.RemoveAt(0);
			} 
		}
		else 
			gameObject.transform.position = Vector2.MoveTowards((Vector2)transform.position,Target,speed);
		if(!GameTerrain.GroundMap.IsSamePosition((Vector2)transform.position,CurrentPosition))
		{
			GameTerrain.GroundMap.SetMartix((Vector2)transform.position,-1);
			GameTerrain.GroundMap.SetMartix(CurrentPosition,0);
		}
		CurrentPosition = (Vector2)transform.position;
		checkTime += Time.deltaTime;
		if(checkTime > 2.0f)
		{
			run = true;
			checkTime = 0.0f;
		}
}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("Emeny Oh~~~~~");
	}
}
