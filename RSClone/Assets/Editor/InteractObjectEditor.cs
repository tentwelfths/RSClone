using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractObject))]
[CanEditMultipleObjects]
public class InteractObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Clear Null Actions"))
        {
            foreach (InteractObject interact in targets)
                interact.ClearNullActions();
        }
    }


}
