using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDictionary))]
public class ItemDictionaryEditor : Editor
{
    private StreamWriter sw;
    public override void OnInspectorGUI()
    { 

        base.OnInspectorGUI();
        if (GUILayout.Button("Export Item List"))
        {
            sw = File.AppendText("ItemList.txt");
            ItemDictionary dict = ((ItemDictionary)target);
            for (int i = 1; i < dict.items.Length; i++)
            {
                ExportItem(dict.items[i]);
            }
            sw.Close();
        }
    }

    private void ExportItem(Item _export)
    {
        sw.WriteLine(_export.id);
        sw.WriteLine(_export.name);
        sw.WriteLine(_export.cost);
        sw.WriteLine(_export.weight);
        sw.WriteLine(_export.stackable);
        sw.WriteLine(_export.examine);
        sw.WriteLine(_export.sprite.ToString());
        sw.WriteLine(_export.model.ToString());
        sw.WriteLine(_export.actions.Length);
        for (int i = 0; i < _export.actions.Length; i++)
        {
            sw.WriteLine(_export.actions[i].funct);
            sw.WriteLine(_export.actions[i].text);
        }
            
        Debug.Log("Added " + _export.id);
    }

}
