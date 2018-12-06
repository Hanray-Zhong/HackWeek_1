using UnityEngine;
using System.Collections;

public class Player : GameDev {
	private SearchPath SP = new SearchPath();
	public float speed = 0.01f;
	private ArrayList Path = new ArrayList();
	private Vector2 Target;
	private int step = 0;
	private bool run = false;
	public GameObject wayPoint;
	private GameObject[] wayPointList;

	void Update() {
		CheckColliderPS();
		if(Input.GetMouseButtonDown(0))
		{
			if(Path.Count > 0 )
			{
				Path.Clear();
				DeleteAllWayPoint();
			}
			Vector3 cam = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 CamP = GameTerrain.GroundMap.ConvertPointToMatrix((Vector2)cam);
			if(GameTerrain.GroundMap.IsOverWidth((int) CamP.x) || GameTerrain.GroundMap.IsOverHeight((int)CamP.y))
			{
				Debug.Log ("Out of Range");
				return;
			}
			Target = new Vector2(cam.x,cam.y);
			if(run == false)
				run = true;

		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(GameDev.GameTime > 0.0f)
				GameDev.GameTime = 0.0f;
			else
				GameDev.GameTime = 0.1f;
		}
	}

	//public override void LateUpdateGame(){
	void LateUpdate(){
		
		if(run == false)
			return ;
		if(Path.Count == 0)
		{
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), Target, speed * Time.deltaTime);
		}
		else if(Path.Count > 0)
		{
			int i, j;
			for (i = Path.Count - 1; i >= 0; i--)
			{
				if (RaycastCheck((Vector2)transform.position, (Vector2)Path[i])) 
				{
					for (j = 0; j < i - 1; j++) 
					{
						Destroy(wayPointList[wayPointList.Length - Path.Count]);
						Path.RemoveAt(0);
					}
					break;
				}
			}
			Vector2 move = (Vector2)Path[0];
			transform.position = Vector2.MoveTowards((Vector2)transform.position, move, speed * Time.deltaTime);
			if((Vector2)transform.position == move)
			{
				Destroy(wayPointList[wayPointList.Length - Path.Count]);
				Path.RemoveAt(0);
			}
		}
		if(new Vector2(transform.position.x,transform.position.y) == Target)
		{
			Path.Clear();
			run = false;
		}
	}


	/// <summary>
	/// 每秒钟检测物体与终点之间有没有墙壁阻挡
	/// </summary>
	private void CheckColliderPS() {
		if (RaycastCheck((Vector2)(transform.position), Target)) {
			Path.Clear();
			DeleteAllWayPoint();
			return;
		}
		if (run == true && Path.Count == 0) {
			if(Path.Count > 0 )
			{
				Path.Clear();
				DeleteAllWayPoint();
			}
			Path = SP.AStarSearch(new Vector2(transform.position.x,transform.position.y) ,Target, false, false);
			wayPointList = new GameObject[Path.Count];
			for(int i = 0 ; i < Path.Count ; i++)
			{
				Vector2 P = (Vector2) Path[i];
				wayPointList[i] = (GameObject)Instantiate(wayPoint,new Vector3(P.x,P.y,0),Quaternion.identity);
			}
		}
	}

	private void DeleteAllWayPoint()
	{
		GameObject[] OldWayPoint = GameObject.FindGameObjectsWithTag("waypoint");
		foreach(GameObject OldWP in OldWayPoint)
			Destroy(OldWP);
	}

	/// <summary>
	/// 检测是否会碰到墙壁
	/// </summary>
	private bool RaycastCheck(Vector2 playerPos, Vector2 Target) {
		Vector2 moveDir = (Target - playerPos).normalized;
		RaycastHit2D hit_1 = Physics2D.Raycast(playerPos,  
									moveDir, Vector2.Distance(Target, playerPos), 1 << LayerMask.NameToLayer("Map"));
		RaycastHit2D hit_2 = Physics2D.Raycast(playerPos + new Vector2(0.25f, 0),  
									moveDir, Vector2.Distance(Target, playerPos + new Vector2(0.25f, 0)), 1 << LayerMask.NameToLayer("Map"));
		RaycastHit2D hit_3 = Physics2D.Raycast(playerPos + new Vector2(-0.25f, 0), 
									moveDir, Vector2.Distance(Target, playerPos + new Vector2(-0.25f, 0)), 1 << LayerMask.NameToLayer("Map"));
		RaycastHit2D hit_4 = Physics2D.Raycast(playerPos + new Vector2(0, 0.25f),  
									moveDir, Vector2.Distance(Target, playerPos + new Vector2(0, 0.25f)), 1 << LayerMask.NameToLayer("Map"));
		RaycastHit2D hit_5 = Physics2D.Raycast(playerPos + new Vector2(0, -0.25f),  
									moveDir, Vector2.Distance(Target, playerPos + new Vector2(0, -0.25f)), 1 << LayerMask.NameToLayer("Map"));
		return (hit_1.collider == null && hit_2.collider == null && hit_3.collider == null && hit_4.collider == null && hit_5.collider == null);
	}
}
