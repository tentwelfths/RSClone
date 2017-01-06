using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PathSquareMarker))]
public class PathSquareMarkerEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if (GUILayout.Button ("Get")) {
			bool[] got = CollisionMapEditor.Get (((PathSquareMarker)target).transform.position);
			string s = (got [0].ToString());
			for (int i = 1; i < got.Length; i++) {
				s += ","+got [i];
			}
			Debug.Log (s);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
