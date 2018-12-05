using UnityEngine;
using System.Collections;

public class GameTerrain : MonoBehaviour {
	
	private int TerrainW;
	private int TerrainH;
	private Vector2 TerrainCenterPoint;
	private Vector2 TerrainSreenOrignalPoint;
	public int GridW; // your GameObject base on which size(unit: pixel)
	public int GridH;
	private int[] GroundMatrix; //Background Matrix
	private int MatrixW; //Matrix width
	private int MatrixH; //Matrix height
	private int ScaleValue = 100; //Unity pixel = Image pixel / 100

	public static GameTerrain GroundMap; // Defiend to Global 

	public void InitBackGound(GameObject BG){ //initialize the ground to get the Matrix
		SpriteRenderer GroundRender = BG.GetComponent<SpriteRenderer>();
		TerrainW = (int)Mathf.Floor (GroundRender.sprite.bounds.size.x * BG.transform.localScale.x * ScaleValue); // convert unit pixel to image pixel
		TerrainH = (int)Mathf.Floor (GroundRender.sprite.bounds.size.y * BG.transform.localScale.y * ScaleValue);
		MatrixW = (int)Mathf.Round(TerrainW/GridW);
		MatrixH = (int)Mathf.Round(TerrainH/GridH);
		GroundMatrix = new int[MatrixW*MatrixH];
		TerrainCenterPoint = new Vector2(transform.position.x * ScaleValue , transform.position.y * ScaleValue);
		TerrainSreenOrignalPoint = new Vector2(TerrainCenterPoint.x-TerrainW/2 , Mathf.Round(((TerrainCenterPoint.y - TerrainH/2))/GridH)*GridH);
		//Debug.Log ("SOP: "+TerrainSreenOrignalPoint.x.ToString() +","+TerrainSreenOrignalPoint.y.ToString());
	}

	public void RegisterObstaclesToMaTrix(string ObstaclesTag){ //Register where can not to move
		GameObject[] ObjstaclesArray = GameObject.FindGameObjectsWithTag(ObstaclesTag);
		foreach(GameObject Obs in ObjstaclesArray){
			SpriteRenderer ObsRender = Obs.GetComponent<SpriteRenderer>();
			int ObsW = (int) Mathf.Floor (ObsRender.sprite.bounds.size.x * Obs.transform.localScale.x * ScaleValue); // convert unit pixel to image pixel
			int ObsH = (int) Mathf.Floor (ObsRender.sprite.bounds.size.y * Obs.transform.localScale.y * ScaleValue);
			int ObsUnityW = (int)Mathf.Round(ObsW/GridW); //Get the obstacle width in unity world
			int ObsUnityH = (int)Mathf.Round(ObsH/GridH);
			int ObsOrgPointX = (int)Mathf.Abs(Mathf.Round(TerrainSreenOrignalPoint.x-(Obs.transform.position.x * ScaleValue-ObsW/2)) / (int)GridW); // get the obstacle orignal point
			int ObsOrgPointY = (int)Mathf.Abs(Mathf.Round(TerrainSreenOrignalPoint.y-(Obs.transform.position.y * ScaleValue-ObsH/2)) / (int)GridH);
			for(int Y=0; Y < ObsUnityH ; Y++)
			{
				for(int X=0; X < ObsUnityW ; X++)
				{
					//Debug.Log("X:"+(ObsOrgPointX+X).ToString() +",Y:"+(ObsOrgPointY+Y).ToString());
					GroundMatrix[GetIndex(ObsOrgPointX+X,ObsOrgPointY+Y)] = -1; 
				}
			}
		}
	}

	public Vector2 ConvertPointToMatrix(Vector2 Point)
	{
		float X = Mathf.Abs((int)(TerrainSreenOrignalPoint.x-Point.x * ScaleValue)/(int)GridW);
		float Y = Mathf.Abs((int)(TerrainSreenOrignalPoint.y-Point.y * ScaleValue)/(int)GridH);
		return new Vector2(X,Y);
	}

	public Vector2 ConvertMatrixToPoint(float X, float Y)
	{
		float PX = ((TerrainSreenOrignalPoint.x+X*GridW)+GridW/2)/ScaleValue;
		float PY = ((TerrainSreenOrignalPoint.y+Y*GridH)+GridH/2)/ScaleValue;
		return new Vector2(PX,PY);
	}

	public Vector2 ConvertMatrixToPoint(Vector2 Point)
	{
		float PX = ((TerrainSreenOrignalPoint.x+Point.x*GridW)+GridW/2)/ScaleValue;
		float PY = ((TerrainSreenOrignalPoint.y+Point.y*GridH)+GridH/2)/ScaleValue;
		return new Vector2(PX,PY);
	}

	public int GetMatrixValue(int X , int Y)
	{
		int idx = GetIndex(X,Y);
		return GroundMatrix[idx];
	}

	public int GetIndex(int X , int Y) // convert binary index to unitary index
	{
		return (Y*MatrixW)+(X+Y)-Y;
	}

	public bool IsOverWidth(int W)
	{
		if(W < 0 || W >=MatrixW)
			return true;
		return false;
	}

	public bool IsOverHeight(int H)
	{
		if(H < 0 || H >= MatrixH )
			return true;
		return false;
	}

	public void UpdateMatrix(int[] NewMatrix)
	{
		GroundMatrix = NewMatrix;
	}

	public int[] GetMatrix(bool clone){
		if(clone)
			return (int[])GroundMatrix.Clone();
		return (int[])GroundMatrix;
	}

	public void SetMartix(Vector2 Point, int value)
	{
		Vector2 MP = ConvertPointToMatrix(Point);
		GroundMatrix[GetIndex((int)MP.x,(int)MP.y)] = value;
	}

	public bool IsSamePosition(Vector2 Point1, Vector2 Point2)
	{
		if(ConvertPointToMatrix(Point1) == ConvertPointToMatrix(Point2))
			return true;
		return false;
	}

	void Awake()
	{
		GroundMap = this;
	}

}

