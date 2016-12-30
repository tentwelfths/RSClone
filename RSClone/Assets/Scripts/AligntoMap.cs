using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AligntoMap : MonoBehaviour {

    public LayerMask MapLayer;

	// Use this for initialization
	void Start () {
        alignToMap();
	}
	
    void alignToMap()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position + Vector3.up * 1000, Vector3.down, out hit, 2000, MapLayer.value))
        {
            gameObject.transform.position = hit.point;
        }
    }
}
