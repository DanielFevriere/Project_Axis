using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : ScriptableObject
{
    //Name of the item
    public string itemName;

    //Item Icon
    public SpriteRenderer icon;

    public bool usable;
    public bool disposable;

    //
    public void UseItem()
    {

    }
}
