using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingRange : MonoBehaviour
{

    public Transform Hotspot;

    public ItemIO[] Cookables;

    public string noItemsMessage = "You have nothing to cook!";

    

    // Use this for initialization
    void Start()
    {
        if (Hotspot == null)
            Debug.LogError("No Hotspot initialized for " + gameObject.name);
    }

    public void Cook()
    {
        // Move player to cooking range
        Player.character.SendMessage("SetDestination", Hotspot.position);

        // Iterate through list of cookable items
        for (int i = 0; i < Cookables.Length; i++)
        {
            if (Cookables[i].Execute())
                return;
        }
        GamePlayLog.LogMessage(noItemsMessage);
    }

    public void UseItem()
    {
        for (int i = 0; i < Cookables.Length; i++)
        {
            for (int j = 0; j < Cookables[i].inputItems.Length; j++)
            {
                if (Cookables[i].inputItems[j] == ActionLister.ins.useItem)
                {
                    Cookables[i].Execute();
                    return;
                }
            }
        }
    }
}