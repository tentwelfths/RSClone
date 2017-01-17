using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AligntoMap : MonoBehaviour {

    public LayerMask MapLayer;
    public bool Aligned = false;

	// Use this for initialization
	void Start () {
        if(!Aligned)
            alignToMap();
	}
	
    public void alignToMap()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position + Vector3.up * 1000, Vector3.down, out hit, 2000, MapLayer.value))
        {
            gameObject.transform.position = hit.point;
        }
        Aligned = true;
    }
}
