using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour {

    public Item[] items;
   
    // Use this for initialization
    void Awake () {
		for(int i = 0; i < items.Length; i++)
        {
            Inventory.inv.importItem(items[i]);
        }
	}

    void CreateItem(Item _item)
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
