using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingRange : MonoBehaviour
{

    public Transform Hotspot;

    public ItemIO[] Cookables;

    

    // Use this for initialization
    void Start()
    {
        if (Hotspot == null)
            Debug.LogError("No Hotspot initialized for " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {

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
        GamePlayLog.LogMessage("You have nothing to cook!");
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