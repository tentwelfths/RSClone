using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
  public string Item;

    private void Awake()
    {
        Item thisItem = Inventory.inv.lookupItem(Item);
        gameObject.name = thisItem.name;
        gameObject.GetComponent<ExamineObject>().ExamineText = thisItem.examine;
    }

  private void Collect()
  {
        Inventory.inv.addItem(Item);
        gameObject.SendMessageUpwards("Collected", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
  }
}
