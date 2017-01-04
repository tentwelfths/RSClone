using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
    public int Index = 0;
    private string item;
    private Action[] actions;
    private UnityEngine.UI.Image image;

    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void SetItem(string _item)
    {
        item = _item;

        Inventory.Item currItem = Inventory.lookupItem(item);
        image.sprite = currItem.sprite;

        actions = new Action[currItem.actions.Length];
        for(int i = 0; i < currItem.actions.Length; i++)
        {
            actions[i].text = currItem.actions[i] + " " + currItem.name;
        }
        
    }

    public void ListActions()
    {
        ActionLister.AddActions(actions);
    }
}
