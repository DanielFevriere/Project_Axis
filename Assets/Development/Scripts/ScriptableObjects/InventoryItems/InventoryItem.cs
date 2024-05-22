using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : ScriptableObject
{
    //Name of the item
    public string itemName;

    //Description
    public string itemDescription;

    //Item Icon
    public Sprite icon;

    public bool usable;
    public bool disposable;

    //
    public void UseItem()
    {

    }
}