using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BondsMenu : MonoBehaviour
{
    InventoryManager inventoryManager;
    public BondsCharacter currentBondsCharacter;

    public GameObject bondsMenuContainer;
    public GameObject previousPageButton;
    public GameObject nextPageButton;

    //Display Texts and images
    public List<Image> boosterDisplayIcons;
    public List<TMP_Text> boosterItemDisplayNames;
    public List<GameObject> boosterItemDisplayBgs;

    //Variables for the selected item
    public BoosterItem selectedItem;
    public GameObject selectedItemDisplay;
    public Image selectedItemIcon;
    public TMP_Text selectedItemName;
    public TMP_Text selectedItemDescription;

    public int currentPage = 1;
    public int pageAmount;


    bool isVisible = false;

    private void Awake()
    {
        inventoryManager = InventoryManager.Instance;
        Refresh();
        selectedItemDisplay.SetActive(false);
    }

    void Refresh()
    {
        ClearBoosterItemDisplay();
        UpdateItemDisplay();
    }


    public void UpdateItemDisplay()
    {
        //Based on the bondsMenu page index, use the proper formula to display the correct page of items

        //Names of the item buttons become the required
        for (int i = 0; i < boosterItemDisplayNames.Count; i++)
        {
            int targetIndex = boosterItemDisplayNames.Count * (currentPage - 1) + i;

            //specific case where theres not enough booster items to fill up a page, break from the loop
            if (targetIndex >= inventoryManager.boosterInventory.Count)
            {
                boosterItemDisplayNames[i].gameObject.SetActive(false);
                boosterItemDisplayBgs[i].SetActive(false);
                continue;
            }

            //Finds the Booster item name to show based on the page number and size of the list of names needed to display
            BoosterItem targetBoosterItem = inventoryManager.boosterInventory[boosterItemDisplayNames.Count * (currentPage - 1) + i];

            //If the target item is not null
            if (targetBoosterItem != null)
            {
                //Shows the gameobject
                boosterItemDisplayNames[i].gameObject.SetActive(true);
                boosterItemDisplayBgs[i].SetActive(true);

                //Sets the title
                boosterItemDisplayNames[i].text = targetBoosterItem.itemName;
            }
        }

        //Sets the page amount
        pageAmount = inventoryManager.boosterInventory.Count / boosterItemDisplayNames.Count;

        if (pageAmount == 0)
        {
            pageAmount++;
        }
        else if ((inventoryManager.boosterInventory.Count % boosterItemDisplayNames.Count) != 0)
        {
            pageAmount++;
        }

        //If the page number is 1, hide the previous page button
        if (currentPage == 1)
        {
            previousPageButton.SetActive(false);
        }

        //If theres only one page, hide the next page button
        if (pageAmount == 1)
        {
            nextPageButton.SetActive(false);
        }
        else
        {
            nextPageButton.SetActive(true);
        }

        //If the page number is greater than 1, show the previous page button
        if (currentPage > 1)
        {
            previousPageButton.SetActive(true);

            //now pageAmount serves as the max amount of pages
            if (currentPage == pageAmount)
            {
                nextPageButton.SetActive(false);
            }
            else
            {
                nextPageButton.SetActive(true);
            }
        }
    }

    public void DisplayItem(int itemIndex)
    {
        selectedItemDisplay.SetActive(true);

        //Searches for the item in the List
        for (int i = 0; i < inventoryManager.boosterInventory.Count; i++)
        {
            if (boosterItemDisplayNames[itemIndex].text == inventoryManager.boosterInventory[i].itemName)
            {
                selectedItem = inventoryManager.boosterInventory[i];

                //Displays the item name, description, and icon
                selectedItemName.text = inventoryManager.boosterInventory[i].itemName;
                selectedItemDescription.text = inventoryManager.boosterInventory[i].itemDescription;
                selectedItemIcon.sprite = inventoryManager.boosterInventory[i].icon;
                break;
            }
        }
        UpdateItemDisplay();
    }

    public void HideItem()
    {
        selectedItemDisplay.SetActive(false);
    }

    void ClearBoosterItemDisplay()
    {
        //Clears the quest title, description, rewards, and conditions
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        selectedItemDisplay.SetActive(false);
    }

    /// <summary>
    /// Gives the passed in gift to the bonds character
    /// </summary>
    /// <param name="gift"></param>
    public void GiveGift()
    {
        //Check to see if it's the right gift
        if(selectedItem.requiredBondsCharacter == currentBondsCharacter)
        {
            //Checks to see if it's the right party member giving the gift
            if(GameManager.Instance.partyLeader.name == selectedItem.requiredPartyMember)
            {
                //Play the gift success convo
                StartCoroutine(DialogueManager.Instance.ShowConversation(currentBondsCharacter.giftSuccess));
                GameObject bondsNPC = GameObject.Find(currentBondsCharacter.characterName);
                bondsNPC.GetComponentInChildren<BondsDialogueNPC>().GiveGift(selectedItem);
            }
            //If its not the right party member, but the right gift
            else
            {
                //Play the wrong person convo
                StartCoroutine(DialogueManager.Instance.ShowConversation(currentBondsCharacter.wrongPerson));
            }
        }
        //If its not the correct gift for the bonds character
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowConversation(currentBondsCharacter.wrongGift));
        }

        //Makes sure to go back to the menu after the dialogue
        ToggleVisibility();
        DialogueManager.Instance.OnCloseDialogue += ToggleVisibility;
        DialogueManager.Instance.OnCloseDialogue += ActivateBondsMenuCamera;
        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            GameManager.Instance.ChangeState(GameState.Freeze);
        };
    }

    /// <summary>
    /// Closes the Bonds Menu
    /// </summary>
    public void Exit()
    {
        GameManager.Instance.ChangeState(GameState.FreeRoam);
        GameManager.Instance.SetCurrentCamera(GameManager.Instance.LowCamera);
        UiManager.Instance.bondsMenu.ToggleVisibility();
    }

    /// <summary>
    /// Next Quests Page
    /// </summary>
    public void NextPage()
    {
        if (currentPage != pageAmount)
        {
            currentPage++;
        }
        UpdateItemDisplay();
    }

    /// <summary>
    /// Previous Quests Page
    /// </summary>
    public void PreviousPage()
    {
        if (currentPage != 1)
        {
            currentPage--;
        }
        UpdateItemDisplay();
    }

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        bondsMenuContainer.SetActive(isVisible);
        selectedItemDisplay.SetActive(false);
        Refresh();
        DialogueManager.Instance.OnCloseDialogue -= ToggleVisibility;
    }

    public void ActivateBondsMenuCamera()
    {
        GameManager.Instance.SetCurrentCamera(GameManager.Instance.BondsMenuCamera);
        DialogueManager.Instance.OnCloseDialogue -= ActivateBondsMenuCamera;
    }
}
