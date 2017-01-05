﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    public Transform hotspot;

    // Replace before online implementation!
    public GameObject Player;

    void Net()
    {
        if (Inventory.inv.CheckForItem("net"))
        {
            Player.SendMessage("SetDestination", hotspot.position);
            while (!Inventory.inv.CheckInventoryFull())
            {
                Inventory.inv.addItem("feesh_raw");
                GamePlayLog.LogMessage("You manage to catch a.. feesh.");
            }
        }
        else
        {
            GamePlayLog.LogMessage("You'd need a net to catch these feesh.");
        }
    }
	
}
