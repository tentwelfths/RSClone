using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour {

    public Inventory.Item[] items;
   
    // Use this for initialization
    void Awake () {
		for(int i = 0; i < items.Length; i++)
        {
            Inventory.importItem(items[i]);
        }
	}

    void CreateItem(Inventory.Item _item)
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
