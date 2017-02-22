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
            StartCoroutine(ChopTree());
        }
        else
        {
            GamePlayLog.LogMessage(FailureMessage);
        }
        
    }

    private IEnumerator ChopTree()
    {
        Player.character.SendMessage("SetDestination", transform.position);
        while (CollisionMap.Map.Distance(Player.character.transform.position, transform.position) > 0)
        {
            yield return null;
        }

        GamePlayLog.LogMessage("You begin to hack at the tree..");
        yield return new WaitForSeconds(0.75f);

        if (Inventory.inv.addItem(item))
        {
            GamePlayLog.LogMessage(SuccessMessage);
            states.SetState("Cut");
        }
    }
}
