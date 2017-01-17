using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningOre : MonoBehaviour {

    public string item;
    public string SuccessMessage;
    public string FailureMessage;

    private MultiStateObject states;

    private void Start()
    {
        states = transform.parent.GetComponent<MultiStateObject>();
    }

    public void Mine()
    {
        if (Inventory.inv.CheckForItem("pickaxe_steel"))
        {
            if (Inventory.inv.addItem(item))
            {
                GamePlayLog.LogMessage(SuccessMessage);
                states.SetState("Mined");
            }
        }
        else
        {
            GamePlayLog.LogMessage(FailureMessage);
        }

    }
}
