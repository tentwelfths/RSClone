using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
    public string Item;
  private void Collect()
  {
        Inventory.addItem(Item);
  }
}
