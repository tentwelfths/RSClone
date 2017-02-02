using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour {

    public bool permanent = false;
    public float despawnTime;
    public ItemIO[] Cookables;



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
        for (int i = 0; i < Cookables.Length; i++)
        {
            if (Cookables[i].Execute())
                return;
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
