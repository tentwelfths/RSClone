using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour {

    public string[] fillableItems;
    public string[] filledItems;
    
    void Fill()
    {
        for(int i = 0; i < fillableItems.Length; i++)
            if (Inventory.inv.RemoveItem(fillableItems[i]))
            {
                Inventory.inv.addItem(filledItems[i]);
                return;
            }
    }
}
