using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private static int gold;
    //private static List<Item> items = new List<Item>();
    //Probably a dictionary of items?
    public static ItemSlot[] items = new ItemSlot[19];
    public UnityEngine.UI.Image[] itemSlots;
    private static Dictionary<string, Item> itemList = new Dictionary<string, Item>();


    public void Awake()
    {
        if (itemSlots.Length != 19)
            Debug.LogError("ItemSlots must have 19 members!");

        
    }

    public void Start()
    {
        // Set gold to 0
        setGold(0);

        // Set all items to null to start
        for (int i = 0; i < items.Length; i++)
        {
            items[i].quantity = 0;
            items[i].item = itemList[""];
        }
    }

    public void Update()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        // Updates sprites in inventory
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].sprite = items[i].item.sprite;
        }
    }

    // Returns true if the item was successfully added to inventory.
    public static bool addItem(string _item)
    {
        // Checks if inventory is full
        if (CheckInventoryFull())
            return false;

        // Find first empty slot in inventory
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item.id == "")
            {
                items[i].item = lookupItem(_item);
                GamePlayLog.LogMessage("Picking up " + items[i].item.name);
                return true;
            }
        }
        return false;

        
    }

    public static bool addItem(string _item, int qty)
    {
        // Checks if inventory already has item
        // Updates item quantity

        // otherwise
        // Checks if inventory is full
        // Find first empty slot in inventory
        return false;


    }

    // Returns true if there are no empty item slots
    public static bool CheckInventoryFull()
    {
        // Iterates through item slots
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].item.id == "")
                return false;
        }
        // If quantity is 0
        return true;

    }

    // Checks if the player has an item in their inventory
    public static bool CheckForItem(string _id)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            // Don't bother with empty item slots
            // if player has the item(in any quantity)
            if (items[i].item.id == _id)
                return true;
        }
            return false;
    }

    // Removes first instance of an item from player's inventory
    public static bool RemoveItem(string _toRemove)
    {
        //Check that the player has it at all first
        if(!CheckForItem(_toRemove))
        return false;

        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].item.id == _toRemove)
            {
                items[i].item = lookupItem("");
                return true;
            }
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

    public static void importItem(Item _item)
    {
        itemList.Add(_item.id, _item);
    }

    public static Item lookupItem(string _name)
    {
        return itemList[_name];
    }

    public struct ItemSlot
    {
        public int quantity;
        public Item item;
    }

    [System.Serializable]
    public struct Item
    {
      // Name as it appears in-game
      public string name;
      // Unique name of this particular item
      // I don't see this being different from item name usually
      public string id;
      // Base price in gold pieces
      public int cost;
      // weight in kg
      public float weight;
      // whether or not the item is stackable
      public bool stackable;
      // The sprite to use for the item
      public Sprite sprite;
      //Some sort of list of options in addition to Use and Examine
      public string examine;
    }
	
}
