using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public GameObject shopMenuContainer;
    bool isVisible = false;

    public ShopKeeper currentShopKeeper;

    public List<ShopItem> shopItemsList;
    public List<GameObject> shopItemObjects;
    public List<Image> shopItemIcons;

    //Variables for the selected shop item
    public ShopItem selectedShopItem;
    public GameObject selectedItemDisplay;
    public TMP_Text selectedShopItemName;
    public TMP_Text selectedShopItemCost;
    public TMP_Text selectedShopItemDescription;
    public Image selectedShopItemIcon;

    void Awake()
    {
        
    }

    //When the menu is opened, this needs to be called so that it displays the proper keeper to buy from.
    public void SelectShopKeeper(ShopKeeper keeper)
    {
        currentShopKeeper = keeper;
        shopItemsList.Clear();
        for (int i = 0; i < keeper.shopItemList.Count; i++)
        {
            shopItemsList.Add(keeper.shopItemList[i]);
        }
        Refresh();
    }

    public void Refresh()
    {
        //Names of the item buttons become the required
        for (int i = 0; i < shopItemObjects.Count; i++)
        {
            //specific case where theres not enough booster items to fill up a page, break from the loop
            if (i >= shopItemsList.Count)
            {
                shopItemObjects[i].SetActive(false);
                continue;
            }

            //Finds the shop item name to show
            ShopItem targetShopItem = shopItemsList[i];

            //If the target item is not null
            if (targetShopItem != null)
            {
                //Shows the gameobject
                shopItemObjects[i].SetActive(true);

                //Sets the icon
                shopItemIcons[i].sprite = targetShopItem.item.icon;
            }
        }
    }

    public void DisplayItem(int itemIndex)
    {
        selectedItemDisplay.SetActive(true);

        //Searches for the item in the List
        for (int i = 0; i < shopItemsList.Count; i++)
        {
            if (shopItemsList[itemIndex] == currentShopKeeper.shopItemList[i])
            {
                //Makes the selected shop item the item clicked on via index
                selectedShopItem = shopItemsList[itemIndex];

                //Displays the item name, description, and icon
                selectedShopItemName.text = shopItemsList[itemIndex].item.itemName;
                selectedShopItemCost.text = shopItemsList[itemIndex].itemCost.ToString();
                selectedShopItemDescription.text = shopItemsList[itemIndex].item.itemDescription;
                selectedShopItemIcon.sprite = shopItemsList[itemIndex].item.icon;
                break;
            }
        }
        Refresh();
    }

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        shopMenuContainer.SetActive(isVisible);
        selectedItemDisplay.SetActive(false);
        Refresh();
    }
}
