using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
class CollisionPoint{
	public bool active = true;
	public int x;
	public int y;
	public Vector3 position;
	public bool[] rules;
	public CollisionPoint(Vector3 pos,bool[] rules,int x, int y){
		this.position = pos;
		this.rules = rules;
		this.x = x;
		this.y = y;
	}
	public static int GetDist(CollisionPoint a, CollisionPoint b){
		return Mathf.Abs (a.x - b.x) + Mathf.Abs (a.y - b.y);

	}

}
public class PathFinder : MonoBehaviour {
	Vector3 target;
	Vector3 position;
	public bool started = false;
	List<Vector3> path = new List<Vector3>();
	public int maxGenerations = 9;
	public int margin = 10;
	public GameObject debugMarker;
	public bool debug = false;
	List<GameObject> debugMarkers = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	public Vector3 getPoint(float distance){
		if (path.Count > 0) {
			Vector3 toreturn = path [0];
			if (path.Count > 1) {
				for (int i = 1; distance > 0; i++) {
					if (i >= path.Count) {
						return path [path.Count - 1];
					
					}


					distance -= (path [i] - path [i - 1]).magnitude;
					if (distance <= 0) {
						toreturn = path [i];
					}
				}
			}
			return toreturn;
		}
		return Vector3.up*20000;

	}
	public void FindPath(Vector3 position, Vector3 target){
		if (!started) {
			target.x = Mathf.RoundToInt (target.x - 0.5f) + 0.5f;
			target.z = Mathf.RoundToInt (target.z - 0.5f) + 0.5f;
			position.x = Mathf.RoundToInt (position.x - 0.5f) + 0.5f;
			position.z = Mathf.RoundToInt (position.z - 0.5f) + 0.5f;
			started = true;
			this.target = target;
			this.position = position;
			if (debug) {
				FindPathDebug ();
			} else {
				Thread t = new Thread (FindPathThread);
				t.Start ();
			}
		}
	}
	public void FindPathDebug(){
		while (debugMarkers.Count > 0) {
			GameObject obj = debugMarkers [0];
			debugMarkers.RemoveAt (0);
			Destroy (obj);
		}
		float xDist = Mathf.Abs (target.x - position.x);
		float yDist = Mathf.Abs (target.z - position.z);
		int squaresX = (int)(xDist / CollisionMapEditor.globalSquareSize) + 1;
		int squaresY = (int)(yDist / CollisionMapEditor.globalSquareSize) + 1;
		Vector3 lowerLeft = new Vector3 (Mathf.Min (position.x, target.x), 0, Mathf.Min (position.z, target.z));
		//Vector3 upperRight = new Vector3 (Mathf.Max (position.x, target.x), 0, Mathf.Max (position.z, target.z));
		Vector3 actualLow = lowerLeft + Vector3.left * CollisionMapEditor.globalSquareSize * margin + Vector3.back * CollisionMapEditor.globalSquareSize * margin;
		List<CollisionPoint> points = new List<CollisionPoint> ();
		CollisionPoint[,] pointGrid = new CollisionPoint[squaresX + margin * 2, squaresY + margin * 2];
		for (int x = 0; x < squaresX + margin*2; x++) {
			for (int y = 0; y < squaresY + margin*2; y++) {
				Vector3 newPos = (actualLow + Vector3.right * CollisionMapEditor.globalSquareSize * x + Vector3.forward * CollisionMapEditor.globalSquareSize * y);
				pointGrid[x,y] = (new CollisionPoint (newPos, CollisionMapEditor.Get (newPos),x,y));
				points.Add (pointGrid [x, y]);
			}
		}
		CollisionPoint closestToPlayer = points [0];
		float closestPlayDist = 1000;
		float closestTargDist = 1000;
		CollisionPoint closestToTarg = points [0];
		for (int i = 0; i < points.Count; i++) {
			points [i].active = true;
			if (!points [i].rules [0] && !points [i].rules [1] && !points [i].rules [2] && !points [i].rules [3]) {
				//points.RemoveAt (i);
				//i--;
				//points[i].active = false;
			} else {
				float playDist = (points [i].position - position).sqrMagnitude;
				float targDist = (points [i].position - target).sqrMagnitude;
				if (playDist < closestPlayDist) {
					closestToPlayer = points [i];
					closestPlayDist = playDist;
				}
				if (targDist < closestTargDist) {
					closestToTarg = points [i];
					closestTargDist = targDist;
				}
			}
		}
		List<List<CollisionPoint>> pathClusterA = new List<List<CollisionPoint>>();
		List<List<CollisionPoint>> pathClusterB = new List<List<CollisionPoint>>();
		List<CollisionPoint> pathA = new List<CollisionPoint> ();
		pathA.Add (closestToPlayer);
		closestToPlayer.active = false;
		List<CollisionPoint> pathB = new List<CollisionPoint> ();
		pathB.Add (closestToTarg);
		closestToTarg.active = false;
		pathClusterA.Add (pathA);
		pathClusterB.Add (pathB);
		List<CollisionPoint> result = new List<CollisionPoint> ();
		Debug.Log ("starting");
		for (int i = 0; true; i++) {
			Debug.Log ("Wave " + i);
			Debug.Log ("Cluster A " + pathClusterA.Count);
			if (CheckAdj (pathClusterA, pathClusterB, out result)) {
				Debug.Log ("Found path after " + i + " waves");
				break;
			}
			xpandAllPaths (pathClusterA,pointGrid);
			if (CheckAdj (pathClusterA, pathClusterB, out result)) {
				Debug.Log ("Found path after " + i + " waves");
				break;
			}
			xpandAllPaths (pathClusterB, pointGrid);
			Debug.Log ("Cluster b " + pathClusterB.Count);
			//Debug.Log ("A:" + pathClusterA == null);
			//Debug.Log ("B:" + pathClusterB == null);
			//Debug.Log ("A:"+pathClusterA.Count+" B:"+pathClusterB.Count);

			if (pathClusterA.Count == 0 || pathClusterB.Count == 0) {
				Debug.Log ("dead end");
				PlaceDebugMarkers (pathClusterA);
				PlaceDebugMarkers (pathClusterB);
				path.Clear ();
				started = false;
				return;
			}
			if (i == maxGenerations) {
				Debug.Log ("found no path within time");
				PlaceDebugMarkers (pathClusterA);
				PlaceDebugMarkers (pathClusterB);
				path.Clear ();
				started = false;
				return;
			}
		}
		path.Clear ();
		for (int i = 0; i < result.Count; i++) {
			path.Add (result [i].position);
		}
		started = false;
	}
	void PlaceDebugMarkers(List<List<CollisionPoint>> cluster){
		for (int i = 0; i < cluster.Count; i++) {
			for(int j=0;j<cluster[i].Count;j++){
				GameObject obj = (GameObject)Instantiate (debugMarker, new Vector3 (cluster [i] [j].position.x, 10, cluster [i] [j].position.z), Quaternion.identity);
				debugMarkers.Add (obj);
			}
		}
	
	}
	public void FindPathThread(){
		float xDist = Mathf.Abs (target.x - position.x);
		float yDist = Mathf.Abs (target.z - position.z);
		int squaresX = (int)(xDist / CollisionMapEditor.globalSquareSize) + 1;
		int squaresY = (int)(yDist / CollisionMapEditor.globalSquareSize) + 1;
		Vector3 lowerLeft = new Vector3 (Mathf.Min (position.x, target.x), 0, Mathf.Min (position.z, target.z));
		//Vector3 upperRight = new Vector3 (Mathf.Max (position.x, target.x), 0, Mathf.Max (position.z, target.z));
		Vector3 actualLow = lowerLeft + Vector3.left * CollisionMapEditor.globalSquareSize * margin + Vector3.back * CollisionMapEditor.globalSquareSize * margin;
		List<CollisionPoint> points = new List<CollisionPoint> ();
		CollisionPoint[,] pointGrid = new CollisionPoint[squaresX + margin * 2, squaresY + margin * 2];
		for (int x = 0; x < squaresX + margin*2; x++) {
			for (int y = 0; y < squaresY + margin*2; y++) {
				Vector3 newPos = (actualLow + Vector3.right * CollisionMapEditor.globalSquareSize * x + Vector3.forward * CollisionMapEditor.globalSquareSize * y);
				pointGrid[x,y] = (new CollisionPoint (newPos, CollisionMapEditor.Get (newPos),x,y));
				points.Add (pointGrid [x, y]);
			}
		}
		CollisionPoint closestToPlayer = points [0];
		float closestPlayDist = 1000;
		float closestTargDist = 1000;
		CollisionPoint closestToTarg = points [0];
		for (int i = 0; i < points.Count; i++) {
			if (!points [i].rules [0] && !points [i].rules [1] && !points [i].rules [2] && !points [i].rules [3]) {
				//points.RemoveAt (i);
				//i--;
			//	points[i].active = false;
			} else {
				float playDist = (points [i].position - position).sqrMagnitude;
				float targDist = (points [i].position - target).sqrMagnitude;
				if (playDist < closestPlayDist) {
					closestToPlayer = points [i];
					closestPlayDist = playDist;
				}
				if (targDist < closestTargDist) {
					closestToTarg = points [i];
					closestTargDist = targDist;
				}
			}
		}
		List<List<CollisionPoint>> pathClusterA = new List<List<CollisionPoint>>();
		List<List<CollisionPoint>> pathClusterB = new List<List<CollisionPoint>>();
		List<CollisionPoint> pathA = new List<CollisionPoint> ();
		pathA.Add (closestToPlayer);
		closestToPlayer.active = false;
		List<CollisionPoint> pathB = new List<CollisionPoint> ();
		pathB.Add (closestToTarg);
		closestToTarg.active = false;
		pathClusterA.Add (pathA);
		pathClusterB.Add (pathB);
		List<CollisionPoint> result = new List<CollisionPoint> ();
		Debug.Log ("starting");
		for (int i = 0; true; i++) {
			Debug.Log ("Wave " + i);
			Debug.Log ("Cluster A " + pathClusterA.Count);
			if (CheckAdj (pathClusterA, pathClusterB, out result)) {
				Debug.Log ("Found path after " + i + " waves");
				break;
			}
			xpandAllPaths (pathClusterA,pointGrid);
			if (CheckAdj (pathClusterA, pathClusterB, out result)) {
				Debug.Log ("Found path after " + i + " waves");
				break;
			}
			xpandAllPaths (pathClusterB, pointGrid);
			Debug.Log ("Cluster b " + pathClusterB.Count);
			//Debug.Log ("A:" + pathClusterA == null);
			//Debug.Log ("B:" + pathClusterB == null);
			//Debug.Log ("A:"+pathClusterA.Count+" B:"+pathClusterB.Count);

			if (pathClusterA.Count == 0 || pathClusterB.Count == 0) {
				Debug.Log ("dead end");
				path.Clear ();
				started = false;
				return;
			}
			if (i == maxGenerations) {
				Debug.Log ("found no path within time");
				path.Clear ();
				started = false;
				return;
			}
		}
		path.Clear ();
		for (int i = 0; i < result.Count; i++) {
			path.Add (result [i].position);
		}
		started = false;

	}
	void xpandAllPaths (List<List<CollisionPoint>> seed, CollisionPoint[,] grid){
		List<List<CollisionPoint>> newList = new List<List<CollisionPoint>> ();
		for (int i = 0; i < seed.Count; i++) {
			newList.AddRange (xpandPath (seed [i], grid));
		}
		seed.Clear ();
		seed.AddRange (newList);


	
	
	}
	List<List<CollisionPoint>> xpandPath(List<CollisionPoint> seed, CollisionPoint[,] grid){
		if (seed.Count == 0) {
			return new List<List<CollisionPoint>> ();
		}
		List<List<CollisionPoint>> newPaths = new List<List<CollisionPoint>> ();
		CollisionPoint last = seed [seed.Count - 1];
		if (last.rules [0] && last.y < grid.GetLength (1)-1&&grid[last.x,last.y+1].active) {
			List<CollisionPoint> newPath = new List<CollisionPoint> (seed);
			newPath.Add (grid [last.x, last.y+1]);
			grid [last.x, last.y+1].active = false;
			newPaths.Add (newPath);
			//Debug.Log ("north");
		}
		if (last.rules [1] && last.x < grid.GetLength (0)-1&&grid[last.x+1,last.y].active) {
			List<CollisionPoint> newPath = new List<CollisionPoint> (seed);
			newPath.Add (grid [last.x+1, last.y]);
			grid [last.x+1, last.y].active = false;
			newPaths.Add (newPath);
			//Debug.Log ("east");
		}
		if (last.rules [2] && last.y >0&&grid[last.x,last.y-1].active) {
			List<CollisionPoint> newPath = new List<CollisionPoint> (seed);
			newPath.Add (grid [last.x, last.y-1]);
			grid [last.x, last.y-1].active = false;
			newPaths.Add (newPath);
			//Debug.Log ("south");
		}
		if (last.rules [3] && last.x >0&&grid[last.x-1,last.y].active) {
			List<CollisionPoint> newPath = new List<CollisionPoint> (seed);
			newPath.Add (grid [last.x-1, last.y]);
			grid [last.x-1, last.y].active = false;
			newPaths.Add (newPath);
			//Debug.Log ("west");
		}
		return newPaths;
	}
	bool CheckAdj(List<List<CollisionPoint>> clusterA, List<List<CollisionPoint>> clusterB,out List<CollisionPoint> result){
		for (int i = 0; i < clusterA.Count; i++) {
			CollisionPoint clusterAchoice = clusterA [i] [clusterA [i].Count - 1];
			for (int j = 0; j < clusterB.Count; j++) {
				CollisionPoint clusterBChoice = clusterB [j] [clusterB [j].Count - 1];
				int dist = CollisionPoint.GetDist (clusterAchoice, clusterBChoice);
				if (dist == 1) {
					int xOffset = clusterAchoice.x - clusterBChoice.x;
					int yOffset = clusterAchoice.y - clusterBChoice.y;
					if (xOffset == 1) {
						List<CollisionPoint> newPath = new List<CollisionPoint> (clusterA [i]);
						for (int k = clusterB [j].Count - 1; k >= 0; k--) {
							newPath.Add(clusterB[j][k]);
						}
						result = newPath;
						return true;
					}
					if (yOffset == 1) {
						List<CollisionPoint> newPath = new List<CollisionPoint> (clusterA [i]);
						for (int k = clusterB [j].Count - 1; k >= 0; k--) {
							newPath.Add(clusterB[j][k]);
						}
						result = newPath;
						return true;
					}
					if (xOffset == -1) {
						List<CollisionPoint> newPath = new List<CollisionPoint> (clusterA [i]);
						for (int k = clusterB [j].Count - 1; k >= 0; k--) {
							newPath.Add(clusterB[j][k]);
						}
						result = newPath;
						return true;
					}
					if (yOffset == -1) {
						List<CollisionPoint> newPath = new List<CollisionPoint> (clusterA [i]);
						for (int k = clusterB [j].Count - 1; k >= 0; k--) {
							newPath.Add(clusterB[j][k]);
						}
						result = newPath;
						return true;
					}
				
				}
			}
		}
		result = null;
		return false;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
