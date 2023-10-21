using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> boosterInventory;
    public List<InventoryItem> consumableInventory;
    public List<InventoryItem> keyItemInventory;

    //If the item is able to be used, it will be activated
    public void UseItem(InventoryItem item)
    {

    }
}
