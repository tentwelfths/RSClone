using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingRange : MonoBehaviour {

    public Transform Hotspot;

    public CookingIO[] Cookables;
    
    // Replace this prior to online implementation!!
    public GameObject Player;

	// Use this for initialization
	void Start ()
    {
        if (Hotspot == null)
            Debug.LogError("No Hotspot initialized for " + gameObject.name);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Cook()
    {
        // Move player to cooking range
        Player.SendMessage("SetDestination", Hotspot.position);

        // Iterate through list of cookable items
        //   if player has the cookable item
        //   remove it from inventory
        //   roll chance to burn the item
        //   add item(either burnt form or cooked form)
        for(int i = 0; i < Cookables.Length; i++)
        {
            if (Inventory.inv.CheckForItem(Cookables[i].input))
            {
                // This is where the delay would go.
                Inventory.inv.RemoveItem(Cookables[i].input);
                if (Random.Range(0.0f, 1.0f) > Cookables[i].FailChance)
                {
                    Inventory.inv.addItem(Cookables[i].output);
                    GamePlayLog.LogMessage(Cookables[i].SuccessMessage);
                }
                else
                {
                    Inventory.inv.addItem(Cookables[i].failOutput);
                    GamePlayLog.LogMessage(Cookables[i].FailureMessage);
                }
                return;
            }
        }
        GamePlayLog.LogMessage("You have nothing to cook!");
    }

    
}

[System.Serializable]
public struct CookingIO
{
    public string input;
    public string output;
    public string failOutput;
    public float FailChance;
    public string SuccessMessage;
    public string FailureMessage;
}