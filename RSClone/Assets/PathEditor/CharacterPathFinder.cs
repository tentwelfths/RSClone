using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Point on the collision map
public class MapNode
{

    public MapNode parent;

    public bool[,] passability;

    private int _gridX, _gridY;

    public int x
    {
        get
        {
            return _gridX;
        }
        set
        {
            _gridX = value;
        }
    }

    public int y
    {
        get
        {
            return _gridY;
        }
        set
        {
            _gridY = value;
        }
    }

    private Vector3 _pos { get; set; }
    public Vector3 pos
    {
        get { return _pos; }
        set { _pos = value; }
    }

    public int fScore
    {
        get { return gScore + hScore; }
    }

    private int _gScore;
    public int gScore
    {
        get { return _gScore; }
        set { _gScore = value; }
    }

    private int _hScore;
    public int hScore
    {
        get { return _hScore; }
        set { _hScore = value; }
    }

    //Constructs a MapNode based on a Vector3
    public MapNode(Vector3 _position, int _x, int _y)
    {
        pos = _position;

        passability = new bool[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                passability[i, j] = true;

        _gridX = _x;
        _gridY = _y;

        gScore = -1;
        hScore = -1;

    }

}

public class CharacterPathFinder : MonoBehaviour
{

    public bool viewDebug;
    public GameObject CurrentMarker;
    private List<GameObject> OpenMarkers = new List<GameObject>();
    public GameObject NeighborMarker;
    private List<GameObject> ClosedMarkers = new List<GameObject>();
    public GameObject DestinationMarker;

    private CollisionMap grid;
     
    private List<MapNode> path = new List<MapNode>();
    private Vector3 _target;
    private Coroutine CalcPath;

    private void Awake()
    {
        grid = CollisionMap.Map;
    }

    public Vector3 Target
    {
        get { return _target; }
        set {
            _target = value;
            StopCoroutine(WalkPath());
            if(CalcPath != null)
                StopCoroutine(CalcPath);
            StartCoroutine(SetDestination(_target));
        }
    }

    IEnumerator WalkPath()
    {
        Coroutine CurrStep;
        foreach (MapNode node in path)
        {
            //Debug.Log("Moving to " + node.x + "," + node.y);
            CurrStep = StartCoroutine(MoveTo(node));
            
            yield return CurrStep;
        }
    }

    private IEnumerator MoveTo(MapNode _dest)
    {
        float time = Time.deltaTime*5;
        Vector2 startPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 destPos = new Vector2(_dest.pos.x, _dest.pos.z);
        while (Vector2.Distance(startPos, destPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(new Vector3(startPos.x, 0, startPos.y), _dest.pos, time);
            startPos = new Vector2(transform.position.x, transform.position.z);
            time += Time.deltaTime * 5;
            GetComponent<AligntoMap>().alignToMap();
            yield return null;
        }

        transform.position = _dest.pos;
        GetComponent<AligntoMap>().alignToMap();
    }

    private IEnumerator SetDestination(Vector3 _dest)
    {

        MapNode start = grid.NodefromWorldPoint(transform.position);
        MapNode end = grid.NodefromWorldPoint(_dest);

        float time = Time.time;

        Debug.Log("Traveling from " + start.x + "," + start.y + " to " + end.x + "," + end.y + "(" + grid.GetDistance(start,end) + " squares)");
        CalcPath = StartCoroutine(CalculatePath(transform.position, _dest));
        Debug.Log("Beginning travel at " + Time.time);
        yield return CalcPath;
        Debug.Log("Path calculated at " + Time.time);
        yield return (WalkPath());
        time = Time.time - time;
        Debug.Log("Destination reached in " + time + " seconds.");
    }

    private IEnumerator CalculatePath(Vector3 _start, Vector3 _end)
    {
        if(viewDebug)
            DestinationMarker.transform.position = _end + Vector3.up * 2;
        MapNode startNode = grid.NodefromWorldPoint(_start);
        MapNode endNode = grid.NodefromWorldPoint(_end);

        List<MapNode> OpenSet = new List<MapNode>();
        HashSet<MapNode> ClosedSet = new HashSet<MapNode>();

        OpenSet.Add(startNode);
        while (OpenSet.Count > 0)
        {
            if (viewDebug)
            {
                foreach (GameObject o in OpenMarkers)
                {
                    Destroy(o);
                }
                foreach (GameObject o in ClosedMarkers)
                {
                    Destroy(o);
                }
                OpenMarkers.Clear();
                ClosedMarkers.Clear();
            }
            MapNode Current = OpenSet[0];

            for (int i = 1; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].fScore < Current.fScore
                 || OpenSet[i].fScore == Current.fScore && OpenSet[i].hScore < Current.hScore)
                    Current = OpenSet[i];
            }

            OpenSet.Remove(Current);
            ClosedSet.Add(Current);

            if (Current == endNode)
            {
                RetracePath(startNode, endNode);
                break;
            }

                foreach (MapNode neighbor in grid.GetNeighbors(Current))
                {
                    if (ClosedSet.Contains(neighbor))
                        continue;

                int newCost = Current.gScore + grid.GetDistance(Current,neighbor);
                if (newCost < neighbor.gScore || !OpenSet.Contains(neighbor))
                {
                    neighbor.gScore = newCost;
                    neighbor.hScore = grid.GetDistance(neighbor, endNode);
                    neighbor.parent = Current;

                    if (!OpenSet.Contains(neighbor))
                    {
                        OpenSet.Add(neighbor);
                    }
                }
                }
                
            if(viewDebug)
            {
                foreach (MapNode node in OpenSet)
                {
                    OpenMarkers.Add(GameObject.Instantiate(CurrentMarker, node.pos + Vector3.up, Quaternion.identity));
                }
                foreach (MapNode node in ClosedSet)
                {
                    ClosedMarkers.Add(GameObject.Instantiate(NeighborMarker, node.pos + Vector3.up, Quaternion.identity));
                }
            }

            yield return null;
            }


    }

    void ClearDuplicatePositions(List<MapNode> _list)
    {
        for(int i = 0; i < _list.Count;i++)
        {
            for (int j = i + 1; j < _list.Count; j++)
                if ((_list[i].x == _list[j].x) && (_list[i].y == _list[j].y))
                    _list.RemoveAt(j);
        }
    }

    void RetracePath(MapNode _start, MapNode _end)
    {
        List<MapNode> _path = new List<MapNode>();
        MapNode _current = _end;
            while(_current != _start)
        {
            _path.Add(_current);
            _current = _current.parent;
        }

        _path.Reverse();
        path = _path;
        Debug.Log(path.Count);
    }
}
