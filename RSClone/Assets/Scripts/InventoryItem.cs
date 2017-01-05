using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
    public int Index = 0;
    public int quantity = 0;
    private string item;
    private Action[] actions;
    private UnityEngine.UI.Image image;

    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
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
        
    }

    public void ListActions()
    {
        ActionLister.AddActions(actions);
    }

    public void RemoveActions()
    {
        ActionLister.RemoveActions(gameObject);
    }

    public void Examine()
    {
        GamePlayLog.LogMessage(Inventory.inv.lookupItem(item).examine);
    }

    public void Drop()
    {
        Inventory.inv.RemoveItem(item);
        ActionLister.RemoveActions(gameObject);

        //Instantiate a despawning item
    }

    //If the item isn't supposed to be tradable, destroy it instead of dropping it.
    public void Destroy()
    {
        Inventory.inv.RemoveItem(item);
        ActionLister.RemoveActions(gameObject);

        //Instantiate a despawning item
    }

    public void Use()
    {
        //Set ActionLister's state to useItem
        //Set outline to white
    }

    public void Light()
    {

    }
}
