using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWalk : MonoBehaviour {
    public Transform Player;
    public Camera miniMap;

    //private Collider col;

    private void Start()
    {
        //col = GetComponent<Collider>();
    }

    private void OnMouseEnter()
    {
        //ActionLister.ListAction(new global::ActionItem("Walk Here", gameObject, true));
    }

    public void Walk()
    {
        RaycastHit Ray;
        if (Input.mousePosition.x / Screen.width < 0.75f)
        {
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, out Ray))
            {
                Player.SendMessage("SetDestination", Ray.point + new Vector3(0, 0.75f, 0));
            }
        }
        else
        {
            if (Physics.Raycast(miniMap.ScreenToWorldPoint(Input.mousePosition), miniMap.transform.forward, out Ray))
            {
                Player.SendMessage("SetDestination", Ray.point + new Vector3(0, 0.75f, 0));
            }
        }

    }
}
