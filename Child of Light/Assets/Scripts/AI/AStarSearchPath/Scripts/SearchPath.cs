using UnityEngine;
using System.Collections;

public class SearchPath {

	// Use this for initialization
	private int ExploreType = 0; //0: Simple Explore, 1 Advance Explore, 2 All Direction Explore

	public ArrayList AStarSearch(Vector2 Start,Vector2 Target,bool isNoPathReturn = false,bool Smooth = false)
	{
		if(Start == Target)
			return new ArrayList();
		GameTerrain GB = GameTerrain.GroundMap;
		int[] OperMap = GB.GetMatrix(true); // clone the matrix status;
		Vector2 StartNode = GB.ConvertPointToMatrix(Start);
		Vector2 EndNode = GB.ConvertPointToMatrix(Target);
		int StartNodeX = (int)StartNode.x;
		int StartNodeY = (int)StartNode.y;
		int EndNodeX = (int)EndNode.x;
		int EndNodeY = (int)EndNode.y;
		bool isNotFind = false;
		Node CurrentNode = new Node(StartNodeX,StartNodeY,0,Dist(EndNodeX,EndNodeY,StartNodeX,StartNodeY),new Vector2(0,0));
		OperMap[GB.GetIndex(StartNodeX,StartNodeY)] = -1; // suppose the start has been explored
		ArrayList FindPath = new ArrayList();
		ArrayList ExploreList = new ArrayList();
		FindPath.Add(CurrentNode);
		for(int step = 0; step < GB.GetMatrix(false).Length * 3; step++)
		//while(true)
		{
			if(StartNodeX == EndNodeX && StartNodeY == EndNodeY)
				goto End;
			for(int Y = CurrentNode.getY()-1; Y <= CurrentNode.getY() + 1 ; Y++)
			{
				if(GB.IsOverHeight(Y))
					continue;
				for(int X = CurrentNode.getX()-1; X <= CurrentNode.getX() + 1 ; X++)
				{
					if(GB.IsOverWidth(X)) //Explored or can not walk or over, skip
					   continue;
					if(OperMap[GB.GetIndex(X,Y)] == -1)
						continue;
					//if is 8 direction , remove line 39~41, and re-comment line 55~62
					if((X == CurrentNode.getX()-1 && Y == CurrentNode.getY() -1) || (X == CurrentNode.getX()+1 && Y == CurrentNode.getY()-1)  // (Top of Left||Top of Right||Buttom of Left||Buttom of Right) G = 14
						|| (X == CurrentNode.getX()-1 && Y == CurrentNode.getY()+1) || (X == CurrentNode.getX()+1 && Y == CurrentNode.getY() +1))
						continue;
					if(X == EndNodeX && Y == EndNodeY)
					{
						FindPath.Add(new Node(X,Y,0,0,CurrentNode.GetVecotr2()));
						goto End;
					}
					if((X == CurrentNode.getX() && Y == CurrentNode.getY() -1) || (X == CurrentNode.getX()-1 && Y == CurrentNode.getY())  // (UP||LEFT||RIGTH||DOWN) G = 10
					   || (X == CurrentNode.getX()+1 && Y == CurrentNode.getY()) || (X == CurrentNode.getX() && Y == CurrentNode.getY() +1))
					{
						int G = CurrentNode.G + 10;
						int F = G + Dist(EndNodeX,EndNodeY,X,Y)+OperMap[GB.GetIndex(X,Y)];
						OperMap[GB.GetIndex(X,Y)] = -1;
						InsertSortNode(ref ExploreList,new Node(X,Y,G,F,CurrentNode.GetVecotr2()));
					}
                    // else if((X == CurrentNode.getX()-1 && Y == CurrentNode.getY() -1) || (X == CurrentNode.getX()+1 && Y == CurrentNode.getY()-1)  // (Top of Left||Top of Right||Buttom of Left||Buttom of Right) G = 14
 					// || (X == CurrentNode.getX()-1 && Y == CurrentNode.getY()+1) || (X == CurrentNode.getX()+1 && Y == CurrentNode.getY() +1))
					// {
					// 		int G = CurrentNode.G + 14;
					// 		int F = G + Dist(EndNodeX,EndNodeY,X,Y)+OperMap[GB.GetIndex(X,Y)];
					// 		OperMap[GB.GetIndex(X,Y)] = -1;
					// 		InsertSortNode( ref ExploreList,new Node(X,Y,G,F,CurrentNode.GetVecotr2()));
					// }

				} // End X loop
			}// End Y loop
			if(ExploreList.Count == 0) // Can not find any path
			{
				isNotFind = true;
				if(!isNoPathReturn)
					FindPath.Clear();
				break;
			}else{
				//ShowExploreList(ExploreList);
				Node MinP = (Node) ExploreList[0];
				ExploreList.RemoveAt(0);
				//ShowExploreList(ExploreList);
				FindPath.Add (MinP);
				CurrentNode = MinP;
				OperMap[GB.GetIndex(CurrentNode.getX(),CurrentNode.getY())] = -1;
				Vector2 CP = CurrentNode.GetVecotr2();
				if(CP.x == EndNodeX && CP.y == EndNodeY)
				{
					break;
				}
			}
		}// end while
		End: ExploreList.Clear();
		return GetSearchPath(FindPath,GB,Start,Target,Smooth,isNoPathReturn,isNotFind);
	}

	private int Dist(int Ex,int Ey,int Sx,int Sy )
	{
		//Manhattan Method
		return (Mathf.Abs(Ex-Sx) + Mathf.Abs(Ey-Sy)) * 10;
	}

	private ArrayList GetSearchPath(ArrayList FindPath,GameTerrain GB,Vector2 OrignalStart,Vector2 OrignalTarget,bool Smooth,bool isNoPathReturn,bool isNotFind)
	{
		ArrayList Path = new ArrayList();
		int minFindex = FindPath.Count-1;
		if(isNoPathReturn && isNotFind && FindPath.Count > 0)
		{
			int MinF = 0;
			for(int i = FindPath.Count -1; i >0  ; i--)
			{
				Node tmpP = (Node)FindPath[i];
				if(i == FindPath.Count -1)
				{
					MinF = tmpP.F;
					continue;
				}
				if(tmpP.F < MinF)
				{
					MinF = tmpP.F;
					minFindex =  i;
				}
			}
		}
		for(int i = minFindex; i >= 0 ; i--)
		{
			Node P = (Node) FindPath[i];
			if(i == minFindex) // first point
				Path.Add(P);
			else
			{
				Node tp = (Node)Path[Path.Count-1];
				if(tp.Parent == P.GetVecotr2())
					Path.Add(P);
			}
		}
		ArrayList SearchPath = new ArrayList();
		for(int i = Path.Count-1 ; i >= 0; i--)
		{
			Node P = (Node)Path[i];
			Vector2 AddPosition = GB.ConvertMatrixToPoint((float)P.getX(),(float)P.getY());
			if(Smooth)
			{
				if(i == Path.Count-1)
				{
					SearchPath.Add(OrignalStart);
				}
				if(i == 0 && GB.ConvertPointToMatrix(OrignalTarget)==P.GetVecotr2())
				{
					SearchPath.Add(OrignalTarget);
				}
			}
			SearchPath.Add(AddPosition);
		}
		return SearchPath;
	}

	private void InsertSortNode(ref ArrayList L , Node P)
	{
		int i=0;
		foreach(Node Lp in L)
		{
			if(P.F < Lp.F)
			{
				L.Insert(i,P);
				return;
			}
			i++;
		}
		L.Add(P);
	}
}

public class Node{
	private int X;
	private int Y;
	public int F;
	public int G;
	public Vector2 Parent;
	public Node (){}
	public Node(int x, int y,int g, int f , Vector2 p)
	{
		X = x;
		Y = y;
		F = f;
		G = g;
		Parent = p;
	}
	public Vector2 GetVecotr2()
	{
		return new Vector2((float) X, (float) Y);
	}
	public int getX()
	{
		return X;
	}
	public int getY()
	{
		return Y;
	}

}