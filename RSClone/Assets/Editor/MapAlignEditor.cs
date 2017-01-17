using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AligntoMap))]
[CanEditMultipleObjects]
public class MapAlignEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Align To Map"))
        {
            foreach(AligntoMap toAlign in targets)
            toAlign.alignToMap();
        }
    }


}
