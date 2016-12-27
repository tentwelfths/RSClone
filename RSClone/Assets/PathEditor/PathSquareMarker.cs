using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class PathSquareMarker : MonoBehaviour {
	public bool north;
	public bool east;
	public Material redColor;
	public Material greenColor;
	Renderer rend1;
	Renderer rend2;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if (rend1 == null) {
			rend1 = transform.GetChild(0).GetComponent<Renderer> ();
		}
		if (rend2 == null) {
			rend2 = transform.GetChild (1).GetComponent<Renderer> ();
		
		}
		if (north) {
			rend1.material = greenColor;
		} else {
			rend1.material = redColor;
		}
		if (east) {
			rend2.material = greenColor;
		} else {
			rend2.material = redColor;
		}
		
	}
}
