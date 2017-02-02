using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.LogError("This object was instantiated. It is a placeholder for a null object. Please find its source and replace it");
        Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
