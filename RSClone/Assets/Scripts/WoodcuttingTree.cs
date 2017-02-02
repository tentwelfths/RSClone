using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodcuttingTree : MonoBehaviour {

    public string item;
    public string SuccessMessage;
    public string FailureMessage;

    private MultiStateObject states;

    private void Start()
    {
        states = transform.parent.GetComponent<MultiStateObject>();
    }

    public void Chop()
    {
        if (Inventory.inv.CheckForItem("hatchet_steel"))
        {
                if (Inventory.inv.addItem(item))
                {
                    GamePlayLog.LogMessage(SuccessMessage);
                    states.SetState("Cut");
                }
        }
        else
        {
            GamePlayLog.LogMessage(FailureMessage);
        }
        
    }
}
