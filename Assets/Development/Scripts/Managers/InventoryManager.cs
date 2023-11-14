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

    public List<InventoryItem> boosterInventory;
    public List<InventoryItem> consumableInventory;
    public List<InventoryItem> keyItemInventory;

    //If the item is able to be used, it will be activated
    public void UseItem(InventoryItem item)
    {

    }
}
