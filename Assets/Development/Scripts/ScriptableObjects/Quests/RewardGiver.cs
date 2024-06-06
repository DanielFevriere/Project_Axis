using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardGiver
{
    public int veniReward;
    public List<BoosterItem> boosterItemRewards;
    public List<ConsumableItem> consumableItemRewards;
    public List<KeyItem> keyItemRewards;
    public List<Node> nodeRewards;

    public void GiveReward()
    {
        GiveVeni();

        GiveBoosters();
        GiveConsumables();
        GiveKeyItems();

        GiveNodes();
    }

    void GiveVeni()
    {
        GameManager.Instance.veni += veniReward;
    }

    void GiveBoosters()
    {
        //Cycles through items needed to be given
        for (int i = 0; i < boosterItemRewards.Count; i++)
        {
            //If theres initially nothing in the inventory
            if(InventoryManager.Instance.boosterInventory.Count == 0)
            {
                //Add the item to the inventory, and move to the next item
                InventoryManager.Instance.boosterInventory.Add(boosterItemRewards[i]);
                continue;
            }

            //Searches to see if the item already exists
            for (int j = 0; j < InventoryManager.Instance.boosterInventory.Count; j++)
            {
                //if it already exists
                if (InventoryManager.Instance.boosterInventory[i] == boosterItemRewards[i])
                {
                    //This shouldn't happen. Each booster item is unique and there should only be one of each, so skip to the next item
                    //BUTTTT comment out the break if you want to see duplicates (test reasons)
                    //break;
                }

                //If it's checked the last item and theres no duplicates
                if (j == InventoryManager.Instance.boosterInventory.Count - 1)
                {
                    //Add the item to the inventory
                    InventoryManager.Instance.boosterInventory.Add(boosterItemRewards[i]);
                    break;
                }
            }
        }
    }
    void GiveConsumables()
    {
        //Cycles through items needed to be given
        for (int i = 0; i < consumableItemRewards.Count; i++)
        {
            //If theres initially nothing in the inventory
            if (InventoryManager.Instance.consumableInventory.Count == 0)
            {

                //Add the item to the inventory, and move to the next item
                InventoryManager.Instance.consumableInventory.Add(consumableItemRewards[i]);
                InventoryManager.Instance.consumableInventory[i].amount += 1;
                continue;
            }

            //Searches to see if the item already exists
            for (int j = 0; j < InventoryManager.Instance.consumableInventory.Count; j++)
            {
                //if it already exists
                if (InventoryManager.Instance.consumableInventory[i] == consumableItemRewards[i])
                {
                    //Add it to the inventory
                    InventoryManager.Instance.consumableInventory[i].amount += 1;
                    break;
                }

                //If it's checked the last item and theres no duplicates
                if (j == InventoryManager.Instance.consumableInventory.Count - 1)
                {
                    //Add the item to the inventory
                    InventoryManager.Instance.consumableInventory.Add(consumableItemRewards[i]);
                    break;
                }
            }
        }
    }
    void GiveKeyItems()
    {
        //Cycles through items needed to be given
        for (int i = 0; i < keyItemRewards.Count; i++)
        {
            //If theres initially nothing in the inventory
            if (InventoryManager.Instance.keyItemInventory.Count == 0)
            {
                //Add the item to the inventory
                InventoryManager.Instance.keyItemInventory.Add(keyItemRewards[i]);
                continue;
            }

            //Searches to see if the item already exists
            for (int j = 0; j < InventoryManager.Instance.keyItemInventory.Count; j++)
            {
                //if it already exists
                if (InventoryManager.Instance.keyItemInventory[i] == keyItemRewards[i])
                {
                    //This shouldn't happen. Each key item is unique and there should only be one of each, so skip to the next item
                    //BUTTTT comment out the break if you want to see duplicates (test reasons)
                    //break;
                }

                //If it's checked the last item and theres no duplicates
                if (j == InventoryManager.Instance.keyItemInventory.Count - 1)
                {
                    //Add the item to the inventory
                    InventoryManager.Instance.keyItemInventory.Add(keyItemRewards[i]);
                    break;
                }
            }
        }
    }

    void GiveNodes()
    {
        //If theres no nodes, return
        if(nodeRewards.Count == 0)
        {
            return;
        }

        for (int i = 0; i < nodeRewards.Count; i++)
        {
            AtlasManager.Instance.UnlockNode(nodeRewards[i].nodeID);
        }

        //Refreshes atlas so nodes are applied to characters
        AtlasManager.Instance.UpdateAtlas();
    }

    public string RewardsText()
    {
        string rewardsText = "";

        //Cycles through items needed to be given, adds them to the text
        for (int i = 0; i < boosterItemRewards.Count; i++)
        {
            rewardsText += boosterItemRewards[i].itemName;
            rewardsText += "\n";
        }
        for (int i = 0; i < consumableItemRewards.Count; i++)
        {
            rewardsText += consumableItemRewards[i].itemName;
            rewardsText += "\n";
        }
        for (int i = 0; i < keyItemRewards.Count; i++)
        {
            rewardsText += keyItemRewards[i].itemName;
            rewardsText += "\n";
        }

        //Cycles through nodes needed to be given
        for (int i = 0; i < nodeRewards.Count; i++)
        {
            rewardsText += nodeRewards[i].nodeName;
            rewardsText += "\n";
        }

        rewardsText += veniReward.ToString() + " Veni";
        return rewardsText;
    }
}
