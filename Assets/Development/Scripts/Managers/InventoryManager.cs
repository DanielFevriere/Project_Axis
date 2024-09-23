using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Global static reference
    private static InventoryManager instance;
    public static InventoryManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<InventoryManager>();
                return instance;
            }
        }
    }
    #endregion

    public List<BoosterItem> boosterInventory;
    public List<ConsumableItem> consumableInventory;
    public List<KeyItem> keyItemInventory;

    //If the item is able to be used, it will be activated
    public void UseItem(InventoryItem item)
    {

    }

    public void RemoveItem(InventoryItem item)
    {
        //Looks through the inventories to find the item
        for (int i = 0; i < boosterInventory.Count; i++)
        {
            //If the item names match, remove the item
            if(item.itemName == boosterInventory[i].itemName)
            {
                boosterInventory.Remove(boosterInventory[i]);
            }
        }
    }
}


