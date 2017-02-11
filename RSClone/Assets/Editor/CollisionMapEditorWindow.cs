using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CollisionMapEditor))]
public class CollisionMapEditorWindow : Editor
{
    private StreamWriter sw;

    void HideMap(CollisionMapEditor edit)
    {
        while (edit.transform.childCount > 0)
        {
            DestroyImmediate(edit.transform.GetChild(0).gameObject);
        }

    }
    public override void OnInspectorGUI()
    {
        CollisionMapEditor active = (CollisionMapEditor)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Create new map"))
        {
            active.AutoMap();
        }
        if (GUILayout.Button("Display Map"))
        {
            active.DisplayMap();
        }
        if (GUILayout.Button("Hide map"))
        {
            HideMap(active);

        }
        if (GUILayout.Button("Commit Map"))
        {
            active.CommitMap();
        }

        if (GUILayout.Button("Export Map"))
        {
            active.LoadMap();
            sw = File.AppendText(active.mapFileName);
            for (int y = 0; y < active.height; y++)
            {
                for (int x = 0; x < active.width; x++)
                {
                    sw.Write(active.map[x][y][0]);
                    sw.Write(active.map[x][y][1]);
                }
                sw.WriteLine("\n");
            }
            sw.Close();
            Debug.Log("Completed map export");
        }
    }
}