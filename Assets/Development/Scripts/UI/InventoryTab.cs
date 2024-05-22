using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTab : GameTab
{
    public int invPage = 0;
    public TMP_Text veniText;
    public TMP_Text displayItemName;
    public TMP_Text displayItemDescription;
    public InventoryManager InventoryManager;
    public List<Image> inventoryIcons;

    public override void Refresh()
    {
        veniText.text = "Veni: " + GameManager.Instance.veni.ToString();

        //Clears icons
        for (int i = 0; i < inventoryIcons.Count; i++)
        {
            inventoryIcons[i].sprite = null;
        }

        //Puts new icons
        switch (invPage)
        {
            case 0:
                if(InventoryManager.boosterInventory.Count == 0)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < InventoryManager.boosterInventory.Count; i++)
                    {
                        inventoryIcons[i].sprite = InventoryManager.boosterInventory[i].icon;
                    }
                }
                break;

            case 1:
                if(InventoryManager.consumableInventory.Count == 0)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < InventoryManager.consumableInventory.Count; i++)
                    {
                        inventoryIcons[i].sprite = InventoryManager.consumableInventory[i].icon;
                    }
                }
                break;

            case 2:
                if (InventoryManager.keyItemInventory.Count == 0)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < InventoryManager.keyItemInventory.Count; i++)
                    {
                        inventoryIcons[i].sprite = InventoryManager.keyItemInventory[i].icon;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Page 0 = Boosters Tab
    /// Page 1 = Consumables Tab
    /// Page 2 = Key Items Tab
    /// </summary>
    /// <param name="page"></param>
    public void SwitchPage(int page)
    {
        invPage = page;
        Refresh();
    }

    public void Activate()
    {
        invPage = 0;
        Refresh();
    }

    public void DisplayItem(int itemIndex)
    {
        switch (invPage)
        {
            case 0:
                displayItemName.text = InventoryManager.boosterInventory[itemIndex - 1].itemName;
                displayItemDescription.text = InventoryManager.boosterInventory[itemIndex - 1].itemDescription;
                break;

            case 1:
                displayItemName.text = InventoryManager.consumableInventory[itemIndex - 1].itemName;
                displayItemDescription.text = InventoryManager.consumableInventory[itemIndex - 1].itemDescription;
                break;

            case 2:
                displayItemName.text = InventoryManager.keyItemInventory[itemIndex - 1].itemName;
                displayItemDescription.text = InventoryManager.keyItemInventory[itemIndex - 1].itemDescription;
                break;
        }
    }
}
