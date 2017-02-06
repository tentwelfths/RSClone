using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CollisionMapEditor))]
public class CollisionMapEditorWindow : Editor {
	void HideMap(CollisionMapEditor edit){
		while (edit.transform.childCount > 0) {
			DestroyImmediate (edit.transform.GetChild (0).gameObject);
		}
	
	}
	public override void OnInspectorGUI(){
		CollisionMapEditor active = (CollisionMapEditor)target;
		base.OnInspectorGUI ();
		if (GUILayout.Button ("Create new map")) {
			active.AutoMap();
		}
		if (GUILayout.Button ("Display Map")) {
			active.DisplayMap ();
		}
		if (GUILayout.Button ("Hide map")) {
			HideMap (active);
		
		}
		if (GUILayout.Button ("Commit Map")) {
			active.CommitMap ();
		}


	}
}