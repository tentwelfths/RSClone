using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory inv;

    public GameObject bonfire;

    private int gold;
    public InventoryItem[] itemSlots;
    private Dictionary<string, Item> itemList = new Dictionary<string, Item>();

    // Initialize inv and make sure the right number of itemslots are present.
    public void Awake()
    {
        if (inv == null)
            inv = this;
        else
            Debug.LogError("Multiple instances of inventory exist! Delete one");

        if (itemSlots.Length != 19)
            Debug.LogError("ItemSlots must have 19 members!");

    }

    // Set inventory to empty and gold to 0
    public void Start()
    {

        // Set all items to null to start
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].quantity = 0;
            itemSlots[i].SetItem("");
        }
    }

    // Updates sprites in inventory
    // Called each time an item is added or removed from the inventory
    public void UpdateInventory()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].UpdateItem();
        }
    }

    // Returns true if the item was successfully added to inventory.
    public bool addItem(string _item)
    {
        // Checks if inventory is full
        if (CheckInventoryFull())
            return false;

        // Find first empty slot in inventory
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].ItemName() == "")
            {
                itemSlots[i].SetItem(_item);
                GamePlayLog.LogMessage("Picking up " + itemList[_item].name);
                UpdateInventory();
                return true;
            }
        }
        return false;


    }

    public bool addItem(string _item, int qty)
    {
        // Checks if inventory already has item
        // Updates item quantity

        // otherwise
        // Checks if inventory is full
        // Find first empty slot in inventory
        return false;


    }

    public bool addItem(string[] _item)
    {
        if (ItemSlotsFree() < _item.Length)
            return false;

        for (int i = 0; i < _item.Length; i++)
            if (!addItem(_item[i]))
                return false;

        return true;
    }

    // Returns true if there are no empty item slots
    public bool CheckInventoryFull()
    {
        // Iterates through item slots
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].ItemName() == "")
                return false;
        }

        return true;

    }

    public int ItemSlotsFree()
    {
        int count = 0;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].ItemName() == "")
                count++;
        }

        return count;
    }

    // Checks if the player has an item in their inventory
    public bool CheckForItem(string _id)
    {
        for (int i = 0; i < itemSlots.Length; ++i)
        {
            // Don't bother with empty item slots
            // if player has the item(in any quantity)
            if (itemSlots[i].ItemName() == _id)
                return true;
        }
        return false;
    }

    // Check for an array of items
    public bool CheckForItem(string[] _id)
    {
        for (int i = 0; i < _id.Length; i++)
            if (!CheckForItem(_id[i]))
                return false;

        return true;
    }

    // Removes array of items. If the player doesn't have the given item, it stops midway.
    public bool RemoveItem(string[] _toRemove)
    {
        for (int i = 0; i < _toRemove.Length; i++)
            if (!RemoveItem(_toRemove[i]))
                return false;

        return true;
    }

    // Removes first instance of an item from player's inventory
    public bool RemoveItem(string _toRemove)
    {
        //Check that the player has it at all first
        if (!CheckForItem(_toRemove))
            return false;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].ItemName() == _toRemove)
            {
                itemSlots[i].SetItem("");
                UpdateInventory();
                return true;
            }
        }

        return false;
    }

    // Item Dictionary gettors and settors
    public void importItem(Item _item)
    {
        itemList.Add(_item.id, _item);
    }

    public Item lookupItem(string _name)
    {
        return itemList[_name];
    }





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
    //Prefab of the item if it's on the ground
    public GameObject model;
    // Actions you can do with the item
    public Action[] actions;
}

[System.Serializable]
public class ItemIO
{
    public SkillExp reward;
    public string[] inputItems;
    public string successMessage;
    public string[] outputItems;
    [Range(0f, 1f)]
    public float failureChance = 0.0f;
    public string failureMessage;
    public string[] failureOutputItems;
    [Range(0f, 1f)]
    public float time = 0.0f;

    public bool Execute()
    {
        // Do you have the items?
        if (!Inventory.inv.CheckForItem(inputItems))
            return false;

        // Do you have the space when it's all gonna be finished?
        if (Inventory.inv.ItemSlotsFree() + inputItems.Length < outputItems.Length
         || Inventory.inv.ItemSlotsFree() + inputItems.Length < failureOutputItems.Length)
            return false;

        Inventory.inv.RemoveItem(inputItems);
        if (Random.Range(0.0f, 1.0f) > failureChance)
        {
            // Success
            GamePlayLog.LogMessage(successMessage);
            Inventory.inv.addItem(outputItems);
        }
        else
        {
            // Failure
            GamePlayLog.LogMessage(failureMessage);
            Inventory.inv.addItem(failureOutputItems);
        }

        return true;
    }
}