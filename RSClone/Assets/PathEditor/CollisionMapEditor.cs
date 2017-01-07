using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollisionMapEditor : MonoBehaviour {
	public float squareSize = 0.5f;
	public static float globalSquareSize = 0.5f;
	public int width= 10;
	public int height = 10;
	public Vector3 offset;
	public bool[][][] map;
	[SerializeField]
	bool[] mapHolder;
	public GameObject marker;
	PathSquareMarker[,] markers;
	public Color[] excluded;
	public float tolerance;
	public float maxHeightDif = 0.5f;
	public LayerMask collisionLayers;
	public LayerMask killOnSight;
	static List<CollisionMapEditor> maps = new List<CollisionMapEditor>();
	// Use this for initialization
	public void CommitMap(){
		List<bool> boolholder = new List<bool> ();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				boolholder.Add(markers[x,y].north);
				boolholder.Add (markers[x,y].east);
			}
		}
		mapHolder = boolholder.ToArray ();
	}
	public void LoadMap(){
		int index = 0;
		map = new bool[width][][];
		for (int x = 0; x < width; x++) {
			map [x] = new bool[height][];
			for (int y = 0; y < height; y++) {
				map [x] [y] = new bool[2];
				map [x] [y] [0] = mapHolder [index];
				index++;
				map [x] [y] [1] = mapHolder [index];
				index++;
			}
		}
	}
	public void DefaultMap(){
		map = new bool[width][][];
		for (int i = 0; i < width; i++) {
			map [i] = new bool[height][];
			for (int j = 0; j < height; j++) {
				map [i] [j] = new bool[2];
			}
		}
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				map [x] [y] [0] = true;
				map [x] [y] [1] = true;
			}
		}
	
	}
	public void DisplayMap(){
		while (transform.childCount > 0) {
			DestroyImmediate (transform.GetChild (0).gameObject);
		}
        if (mapHolder != null && mapHolder.Length == height*width*2)
        {
            LoadMap();
        }
		markers = new PathSquareMarker[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject newObj = (GameObject)Instantiate (marker, offset + squareSize * x * Vector3.right + squareSize * y * Vector3.forward, Quaternion.identity, transform);
				markers [x, y] = newObj.GetComponent<PathSquareMarker> ();
				markers [x, y].north = map [x][y][0];
				markers [x, y].east = map [x] [y] [1];
				newObj.transform.localScale = Vector3.one * squareSize / 3;
				RaycastHit hit;
				if (Physics.Raycast (newObj.transform.position + Vector3.up * 1000, Vector3.down, out hit)) {
					newObj.transform.position = hit.point;
				}
			}
		}
	}
	void Start () {
		LoadMap ();
		if (maps.Contains (this) == false) {
			maps.Add (this);
		}
		globalSquareSize = squareSize;
	}
	public static bool[] Get(Vector3 position){
		position.x = (Mathf.RoundToInt(position.x - 0.5f)) + 0.5f;
		position.z = (Mathf.RoundToInt(position.z - 0.5f)) + 0.5f;
		bool[] toreturn = new bool[4];
		bool[] gotit = InternalGet (position);
		toreturn [0] = gotit [0];
		toreturn [1] = gotit [1];
		toreturn [2] = InternalGet (position - Vector3.forward * globalSquareSize) [0];
		toreturn [3] = InternalGet (position - Vector3.right * globalSquareSize) [1];
		return toreturn;
	}
	static bool[] InternalGet(Vector3 postion){
		for (int i = 0; i < maps.Count; i++) {
			if (maps [i].offset.x <= postion.x && maps [i].offset.z <= postion.z) {
				if (maps [i].offset.x + maps [i].width * globalSquareSize >= postion.x && maps [i].offset.z + maps [i].height * globalSquareSize >= postion.z) {
					if (i > 0) {
						CollisionMapEditor holder = maps [i - 1];
						maps [i - 1] = maps [i];
						maps [i] = holder;
						i--;
					}
					return maps [i].GetLocal (postion);
				
				}
			
			}
		}
		return new bool[]{ false, false };
	}
	public bool[] GetLocal(Vector3 postition){
		int x = (int)((postition.x - offset.x) / squareSize);
		int y = (int)((postition.z - offset.z) / squareSize);
		if (x < width && y < height) {
			return map [x] [y];
		}
		return new bool[]{ false, false };
	}


	/*public void CommitMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				map [x] [y] [0] = markers [x, y].north;
				map [x] [y] [1] = markers [x, y].east;
			
			}
		}
	}*/
	public void AutoMap(){
		/*
		width = 1;
		height = 1;

		int count = Physics.OverlapBox (offset + Vector3.right * width * squareSize / 2 + Vector3.forward * height * squareSize * 2, Vector3.right * width * squareSize + Vector3.forward * squareSize * height + Vector3.up * squareSize * (height + width) / 2f).Length;
		width = 16;
		height = 16;
		while (count!=Physics.OverlapBox (offset + Vector3.right * width * squareSize / 2 + Vector3.forward * height * squareSize * 2, Vector3.right * width * squareSize + Vector3.forward * squareSize * height + Vector3.up * squareSize * (height + width) / 2f).Length) {
			count = Physics.OverlapBox (offset + Vector3.right * width * squareSize / 2 + Vector3.forward * height * squareSize * 2, Vector3.right * width * squareSize + Vector3.forward * squareSize * height + Vector3.up * squareSize * (height + width) / 2f).Length;
			width *= 2;
			height *= 2;
		}
		*/

		map = new bool[width][][];
		for (int i = 0; i < width; i++) {
			map [i] = new bool[height][];
			for (int j = 0; j < height; j++) {
				map [i] [j] = new bool[]{ true, true };
			}
		}
					Color[,] colors = new Color[width, height];
					float[,] heights = new float[width,height];

					for(int x=0;x<width;x++){
			for (int y = 0; y < height; y++) {
				heights [x, y] = -1000;
				colors [x, y] = Color.white;
				RaycastHit hit;
				if (Physics.Raycast (offset + Vector3.right * squareSize * x + Vector3.forward * squareSize * y + Vector3.up * 1000, Vector3.down,2000, killOnSight.value)) {
					map [x] [y] [0] = false;
					map [x] [y] [1] = false;
					//Debug.Log ("killing");
				
				} else {
					if (Physics.Raycast (offset + Vector3.right * squareSize * x + Vector3.forward * squareSize * y + Vector3.up * 1000, Vector3.down, out hit, 2000, collisionLayers.value)) {
						//Debug.Log ("rayHit");
						heights [x, y] = hit.point.y;
						Texture2D texture = (Texture2D) hit.collider.GetComponent<Renderer> ().sharedMaterial.mainTexture;
						colors [x, y] = texture.GetPixel ( (int) (hit.textureCoord.x * texture.width), (int) (hit.textureCoord.y * texture.height));
						//Debug.Log (colors [x, y]);
					}
				}
			}
		}
		//Debug.Log ("HERE");
		for (int x = 0; x < width-1; x++) {
			for (int y = 0; y < height-1; y++) {
				if(map[x][y][0]||map[x][y][1]){
				//Debug.Log (heights [x, y]);
				float h1 = heights [x, y];
				float h2 = heights [x, y + 1];
				float h3 = heights [x + 1, y];
				if (Mathf.Abs (h2 - h1) > maxHeightDif) {
					map [x] [y] [0] = false;
				} else {
					if (Mathf.Abs (h3 - h1) > maxHeightDif) {
						map [x] [y] [1] = false;
					} else {
							for (int i = 0; i < excluded.Length; i++) {
								Color toCompare = colors [x, y];
								if (true) {
									//Debug.Log ("got color");
									Vector3 toCheck = Vector3.zero;
									toCheck.x = toCompare.r - excluded [i].r;
									toCheck.y = toCompare.g - excluded [i].g;
									toCheck.z = toCompare.b - excluded [i].b;
									float difference = toCheck.magnitude / Vector3.one.magnitude;
									//Debug.Log (difference);
									if (difference < tolerance) {
										map [x] [y] [0] = false;
										map [x] [y] [1] = false;
										break;
									}
								
								}
							}
						}
					}
				}
			}
		}


		for (int x = 0; x < width; x++) {
			map [x] [height - 1] [0] = false;
			map [x] [height - 1] [1] = false;
		}
		for (int y = 0; y < height; y++) {
			map [width - 1] [y] [1] = false;
			map [width - 1] [y] [0] = false;
		}

        //DisplayMap();
        //CommitMap();



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
