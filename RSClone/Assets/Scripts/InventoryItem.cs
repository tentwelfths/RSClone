using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
    public int Index = 0;
    public int quantity = 0;

    private string item;
    private Action[] actions;
    private UnityEngine.UI.Image image;
    private InteractObject obj;

    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        obj = GetComponent<InteractObject>();
    }

    public void UpdateItem()
    {
        Item currItem = Inventory.inv.lookupItem(item);
        image.sprite = currItem.sprite;
    }

    public void ClearItem()
    {
        SetItem("");
        quantity = 0;
    }

    public string ItemName()
    {
        return item;
    }

    public void SetItem(string _item)
    {
        item = _item;

        Item currItem = Inventory.inv.lookupItem(item);
        image.sprite = currItem.sprite;

        actions = new Action[currItem.actions.Length];
        for(int i = 0; i < currItem.actions.Length; i++)
        {
            actions[i] = currItem.actions[i];
            actions[i].obj = gameObject;
        }

        obj.Options = actions;
    }

    private void OnMouseOver()
    {
        obj.SendActions();

        
    }

    public void ListActions()
    {
        ActionLister.ins.AddActions(actions);

        if (ActionLister.ins.useItem != "")
        {
            Action use = new Action();
            use.funct = "UseItem";
            use.obj = gameObject;
            use.priority = 1;
            use.text = "Use " + Inventory.inv.lookupItem(ActionLister.ins.useItem).name + " -> " + Inventory.inv.lookupItem(item);
            ActionLister.ins.AddAction(use);

        }
    }

    public void RemoveActions()
    {
        ActionLister.ins.RemoveActions(gameObject);
    }

    public void Examine()
    {
        GamePlayLog.LogMessage(Inventory.inv.lookupItem(item).examine);
    }

    public void Drop()
    {
        Inventory.inv.RemoveItem(item);
        ActionLister.ins.RemoveActions(gameObject);

        //Instantiate a despawning item
    }

    //If the item isn't supposed to be tradable, destroy it instead of dropping it.
    public void Destroy()
    {
        Inventory.inv.RemoveItem(item);
        ActionLister.ins.RemoveActions(gameObject);

        //Instantiate a despawning item
    }

    public void Use()
    {
        //Set ActionLister's state to useItem
        ActionLister.ins.Use(item);
        //Set outline to white
    }

    public void UseItem()
    {
        if (item == "")
            return;
        string useItem = ActionLister.ins.useItem;

        if(CheckPair(useItem, item, "seashell_dust", "vial_water"))
        {
            Inventory.inv.RemoveItem("seashell_dust");
            Inventory.inv.RemoveItem("vial_water");

            GamePlayLog.LogMessage("You sprinkle the seashell dust into the vial of water.");
            Inventory.inv.addItem("potion_seashell_unf");
            // Add unfinished seashell potion
            return;
        }

        if(CheckPair(useItem, item, "potion_seashell_unf", "leaf_mint"))
        {
            Inventory.inv.RemoveItem("potion_seashell_unf");
            Inventory.inv.RemoveItem("leaf_mint");

            GamePlayLog.LogMessage("You add the mint leaf to the potion.");
            Inventory.inv.addItem("potion_def_unf");
            return;
        }

        if(CheckPair(useItem, item, "tinderbox", "logs"))
        {
            Light();
            return;
        }

        GamePlayLog.LogMessage("Nothing interesting happens.");
    }

    private bool CheckPair(string _checkA, string _checkB, string _pairA, string _pairB)
    {
        // If Item 1 doesn't belong to the pair
        if (_checkA != _pairA && _checkA != _pairB)
            return false;

        // If Item 2 doesn't belong to the pair
        if (_checkB != _pairA && _checkB != _pairB)
            return false;

        return true;
    }

    public void Crush()
    {
        if (!Inventory.inv.CheckForItem("pestle"))
        {
            GamePlayLog.LogMessage("You don't have a pestle and mortar to crush this item.");
            return;
        }

        //Check what item you're crushing
        if(item == "seashell")
        {
            //Remove it
            Inventory.inv.RemoveItem(item);
            GamePlayLog.LogMessage("You crush the seashell to dust.");
            Inventory.inv.addItem("seashell_dust");
            //Add crushed version
        }

    }

    public void Light()
    {
        // Check for tinderbox
        if (Inventory.inv.CheckForItem("tinderbox"))
        {
            Instantiate(Inventory.inv.bonfire, Player.character.transform.position, Quaternion.identity);
            Inventory.inv.RemoveItem("logs");
            GamePlayLog.LogMessage("You light a bonfire");
        }
        else
        {
            GamePlayLog.LogMessage("You don't have a tinderbox to light these logs!");
        }
    }

    public void Carve()
    {
        // Check for knife
        if(Inventory.inv.CheckForItem("knife") || Inventory.inv.CheckForItem("knife_bronze"))
        {
            Inventory.inv.RemoveItem("logs");
            GamePlayLog.LogMessage("You carve the logs into a crude arrow");
            Inventory.inv.addItem("arrow_plain");
        }
        else
        {
            GamePlayLog.LogMessage("You don't have a knife to carve these logs!");
        }
    }
}
