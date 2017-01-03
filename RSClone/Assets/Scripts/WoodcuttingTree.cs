using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodcuttingTree : MonoBehaviour {

    public float RespawnTime;
    public string item;
    public string SuccessMessage;
    public string FailureMessage;

    private float countdowntimer = 0.0f;
    private MultiStateObject states;

    private void Awake()
    {
        states = GetComponent<MultiStateObject>();
    }

    private void Update()
    {
        if(states.GetState() == "Cut")
        {
            countdowntimer -= Time.deltaTime;
            if (countdowntimer <= 0.0f)
                states.SetState("Uncut");
        }
    }

    public void Chop()
    {
        if (Inventory.CheckForItem("hatchet_steel"))
        {
            if (states.GetState() == "Uncut")
            {
                if (Inventory.addItem(item))
                {
                    countdowntimer = RespawnTime * 60.0f;
                    GamePlayLog.LogMessage(SuccessMessage);

                    states.SetState("Cut");
                }
            }
        }
        else
        {
            GamePlayLog.LogMessage(FailureMessage);
        }
        
    }
}
