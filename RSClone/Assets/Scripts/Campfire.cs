using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour {

    public bool permanent = false;
    public float despawnTime;
    public CookingIO[] Cookables;



    private float despawnCountdown;

    private void Awake()
    {
        despawnCountdown = despawnTime;
    }

    private void Update()
    {
        if (!permanent)
        {
            despawnCountdown -= Time.deltaTime;
            if (despawnCountdown <= 0.0f)
                Destroy(gameObject);
        }
    }

    public void Cook()
    {
        // Move player to campfire

        // Iterate through list of cookable items
        //   if player has the cookable item
        //   remove it from inventory
        //   roll chance to burn the item
        //   add item(either burnt form or cooked form)
        for (int i = 0; i < Cookables.Length; i++)
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

    private void Feed()
    {
        if (Inventory.inv.CheckForItem("logs"))
        {
            // This is where the delay would go.
            Inventory.inv.RemoveItem("logs");
            despawnCountdown += 30.0f;
            GamePlayLog.LogMessage("You toss some logs into the fire.");
            return;
        }
        GamePlayLog.LogMessage("You have nothing to burn!");
    }
}
