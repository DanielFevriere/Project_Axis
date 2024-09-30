using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Item")]

public class ShopItem : ScriptableObject
{
    public int itemCost;
    public InventoryItem item;
}
