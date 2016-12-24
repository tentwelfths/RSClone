using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
  public string name;
  public Sprite icon;
  public int id;
  public int cost;
  public float weight;
  public bool stackable = false;
  //Some sort of list of options in addition to Use and Examine
  public string examine;
  private void Collect()
  {
    Inventory.Item i = new Inventory.Item();
    i.name = name;
    i.sprite = icon;
    i.id = id;
    i.cost = cost;
    i.weight = weight;
    i.stackable = stackable;
    i.examine = examine;
    Debug.Log(name);
        Inventory.addItem(i);
  }
}
