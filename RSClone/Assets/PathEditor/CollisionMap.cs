using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMap : MonoBehaviour
{
    public static CollisionMap Map;

    public LayerMask Unwalkable;

    public bool DebugDraw = true;

    public Vector2 MapSize;
    public float nodeRadius = 0.5f;

    public Color[] blockedColors;
    public float colorTolerance;

    private MapNode[,] _Grid;

    public MapNode[,] Grid
    {
        get
        {
            return _Grid;
        }
    }

    float[,] heightMap;

    private float nodeDiameter;
    private int GridSizeX, GridSizeY;

    // Initializing

    void Awake ()
	{
        if (Map != null)
            Debug.LogError("Cannot have more than one instance of CollisionMap!");
        Map = this;

        nodeDiameter = nodeRadius * 2.0f;
        GridSizeX = Mathf.RoundToInt(MapSize.x / nodeDiameter);
        GridSizeY = Mathf.RoundToInt(MapSize.y / nodeDiameter);
        CreateGrid();
        WallBorders();
        BlockObstacles();
        BlockColors();
    }

    private void CreateGrid()
    {
        Vector3 WorldBottomLeft = transform.position;
        _Grid = new MapNode[GridSizeX, GridSizeY];
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 WorldPoint = WorldBottomLeft + (Vector3.right * ((x * nodeDiameter) + nodeRadius)) + (Vector3.forward * ((y * nodeDiameter) + nodeRadius));
                _Grid[x, y] = new MapNode(WorldPoint,x,y);
            }
        }
    }

    // Automatic bocking specific aspects

    private void BlockObstacles()
    {
        RaycastHit hit;
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                            if (Physics.SphereCast(_Grid[x,y].pos + Vector3.up * 100, nodeRadius * 0.5f, -Vector3.up, out hit,Mathf.Infinity, Unwalkable))
                            {
                                MakeImpassable(x, y);
                            }

                        Vector3 dir = new Vector3(i, 0, j);

                        if (Physics.Raycast(_Grid[x, y].pos + Vector3.up, dir, nodeDiameter * dir.magnitude, Unwalkable))
                            _Grid[x, y].passability[i + 1, j + 1] = false;
                    }
                }
            }
        }
            //Raycast in 8 directions
            //If the raycast hit Unwalkable Layer, set that direction's passability to false
    }

    private void BlockColors()
    {
        // Create array of color elements on grid based on node positions
        Texture2D tex;

        tex = (Texture2D)GetComponent<Renderer>().sharedMaterial.mainTexture;

        for (int _x = 0; _x < GridSizeX; _x++)
        {
            for (int _y = 0; _y < GridSizeY; _y++)
            {
                Color toComp = Color.white;
                toComp = tex.GetPixel(_x, _y);

                foreach (Color c in blockedColors)
                {
                    Vector4 comparison = (Vector4)(c - toComp);
                    comparison.w = 0;
                    if (comparison.magnitude < colorTolerance)
                        MakeImpassable(_x,_y);
                }
            }
        }
        
        // Check array against each color in block list

        // if the color matches (give or take tolerance)
        // set node impassable using MarkImpassable()
    }

    private void WallBorders()
    {
        for(int x = 0; x <GridSizeX; x++)
        {
            _Grid[x, 0].passability[0, 0] = false;
            _Grid[x, 0].passability[1, 0] = false;
            _Grid[x, 0].passability[2, 0] = false;

            _Grid[x, GridSizeY-1].passability[0, 2] = false;
            _Grid[x, GridSizeY-1].passability[1, 2] = false;
            _Grid[x, GridSizeY-1].passability[2, 2] = false;
        }

        for (int y = 0; y < GridSizeY; y++)
        {
            _Grid[0, y].passability[0, 0] = false;
            _Grid[0, y].passability[0, 1] = false;
            _Grid[0, y].passability[0, 2] = false;

            _Grid[GridSizeX-1, y].passability[2, 0] = false;
            _Grid[GridSizeX-1, y].passability[2, 1] = false;
            _Grid[GridSizeX-1, y].passability[2, 2] = false;
        }
    }

    private void MakeImpassable(int _x, int _y)
    {
        if (_x >= GridSizeX || _x < 0
         || _y >= GridSizeY || _y < 0)
        {
            Debug.LogError("Error: " + _x + "," + _y + " is out of range");
            return;
        }

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if(_x+i >= 0 && _x+i < GridSizeX
                && _y+j >= 0 && _y+j < GridSizeY)
                    _Grid[_x + i, _y + j].passability[-i + 1, -j + 1] = false;

                _Grid[_x, _y].passability[i + 1, j + 1] = false;
            }
        }
           
    }

    // Access for pathfinding

    public Vector2 GetMapPos(Vector3 _position)
    {
        Vector3 WorldBottomLeft = transform.position;

        Vector3 RelativePos = _position - WorldBottomLeft;
        RelativePos.x = Mathf.RoundToInt(RelativePos.x);
        RelativePos.z = Mathf.RoundToInt(RelativePos.z);

        Vector2 MapPos = new Vector2(RelativePos.x, RelativePos.z);
        return MapPos;
    }

    public MapNode NodefromWorldPoint(Vector3 _pos)
    {
        Vector3 WorldBottomLeft = transform.position;
        Vector3 RelativePos = _pos - WorldBottomLeft;

        RelativePos.x = Mathf.Clamp01(RelativePos.x / MapSize.x);
        RelativePos.z = Mathf.Clamp01(RelativePos.z / MapSize.y);

        int _x = Mathf.RoundToInt((GridSizeX - 1) * RelativePos.x);
        int _y = Mathf.RoundToInt((GridSizeY - 1) * RelativePos.z);

        return Grid[_x, _y];
    }

    public List<MapNode> GetNeighbors(MapNode _node)
    {
        List<MapNode> neighbors = new List<MapNode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int CheckX = x + _node.x;
                int CheckY = y + _node.y;

                if (!_node.passability[x + 1, y + 1])
                    continue;

                if (CheckX >= 0 && CheckX < GridSizeX
                 && CheckY >= 0 && CheckY < GridSizeY)
                    neighbors.Add(Grid[CheckX, CheckY]);

            }
        }

        return neighbors;
    }

    public int GetDistance(MapNode _a, MapNode _b)
    {
        int AbsX = Mathf.Abs(_a.x - _b.x);
        int AbsY = Mathf.Abs(_a.y - _b.y);

        if(AbsX > AbsY)
            return (14 * AbsY) + (10 * (AbsX - AbsY));

        return (14 * AbsX) + (10 * (AbsY - AbsX));
    }

    public int Distance(Vector3 _a, Vector3 _b)
    {
        return (GetDistance(NodefromWorldPoint(_a), NodefromWorldPoint(_b)));
    }

    // Drawing in editor

    private void OnDrawGizmos()
    {
        if (!DebugDraw)
            return;
        Gizmos.DrawWireCube(new Vector3(MapSize.x, 0, MapSize.y)/2 + transform.position, new Vector3(MapSize.x, 2, MapSize.y));
    }

    private void OnDrawGizmosSelected()
    {
        if (!DebugDraw || _Grid == null)
            return;

        foreach (MapNode node in _Grid)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Gizmos.color = (node.passability[i, j]) ? Color.white : Color.red;
                    if (i != 1 || j != 1)
                        Gizmos.DrawLine(node.pos + Vector3.up, node.pos + new Vector3(i-1,0,j-1)*0.5f*nodeRadius + Vector3.up);
                    //Maybe a larger handle here?
                }
            }
        }
    }

}
