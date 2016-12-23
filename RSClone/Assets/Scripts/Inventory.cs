using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private static int gold;
    //private static List<Item> items = new List<Item>();
    //Probably a dictionary of items?
    public static Item[] items = new Item[19];


    // Returns true if the item was successfully added to inventory.
    public static bool addItem(Item item)
    {
      for (int i = 0; i < items.Length; ++i)
      {
        Debug.Log("Item " + i + " " + items[i].empty);
        if (items[i].empty)
        {
          GamePlayLog.LogMessage("Filling empty slot with " + item.name);
          items[i].name = item.name;
          items[i].sprite = item.sprite;
          items[i].id = item.id;
          items[i].cost = item.cost;
          items[i].weight = item.weight;
          items[i].stackable = item.stackable;
          items[i].examine = item.examine;
          items[i].empty = false;
                return true;
        }
      }

      //Listing the inventory?
      for (int i = 0; i < items.Length; ++i)
      {
        if (!items[i].empty)
        {
          Debug.Log("Item " + i + " is " + items[i].name);
        }
      }
      
        return false;
    }

    // Checks if the player has an item in their inventory
    // Will probably adjust to use more readable parameters than id
    public static bool CheckForItem(int id)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            if (items[i].id == id)
                return true;
        }
            return false;
    }


    static int addGold(int _Amt)
    {
        gold += _Amt;

        return gold;
    }

    static int spendGold(int _Amt)
    {
        if(gold < _Amt)
        {
            GamePlayLog.LogMessage("You don't have enough gold for that!");
            return -1;
        }

        gold -= _Amt;

        return gold;
    }

    static int setGold(int _Amt)
    {
        gold = _Amt;
        return _Amt;
    }

    void Start () {
        setGold(0);
        for (int i = 0; i < items.Length; ++i)
        {
          items[i].empty = true;
        }
	}


    public struct Item
    {
      public bool empty;
      public string name;
      public int id;
      public int cost;
      public float weight;
      public bool stackable;
      public Sprite sprite;
      //Some sort of list of options in addition to Use and Examine
      public string examine;
    }
	
}
