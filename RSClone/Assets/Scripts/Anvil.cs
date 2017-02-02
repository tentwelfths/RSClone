using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour {

    public ItemIO[] Smithables;

	void Smith()
    {
        if(!Inventory.inv.CheckForItem("hammer"))
        {
            GamePlayLog.LogMessage("You don't have a hammer to smith any metal!");
            return;
        }

        for (int i = 0; i < Smithables.Length; i++)
        {
            if (Smithables[i].Execute())
                return;
        }
        GamePlayLog.LogMessage("You have no metal to smith!");
    }

    void UseItem()
    {
        if (!Inventory.inv.CheckForItem("hammer"))
        {
            GamePlayLog.LogMessage("You don't have a hammer to smith any metal!");
            return;
        }

        for (int i = 0; i < Smithables.Length; i++)
        {
            for (int j = 0; j < Smithables[i].inputItems.Length; j++)
            {
                if (Smithables[i].inputItems[j] == ActionLister.ins.useItem)
                {
                    Smithables[i].Execute();
                    return;
                }
            }
        }
    }
}
