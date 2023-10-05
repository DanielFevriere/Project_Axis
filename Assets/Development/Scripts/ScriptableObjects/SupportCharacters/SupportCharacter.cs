using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Support Character")]
public class SupportCharacter : ScriptableObject
{
    //Character Name
    public string characterName; 

    //if the character is locked, it cannot be progressed yet
    public bool gabeLocked;
    public bool mikeLocked;
    public bool raphLocked;

    //Character support levels with the main crew
    public int gabeLevel = 0;
    public int mikeLevel = 0;
    public int raphLevel = 0;

    //Character support conditions
    public List<QuestCondition> gabeProgression;
    public List<QuestCondition> mikeProgression;
    public List<QuestCondition> raphProgression;

    //List of convos for each support level
    public List<Conversation> gabeSupportConvos;
    public List<Conversation> mikeSupportConvos;
    public List<Conversation> raphSupportConvos;

    //Rewards for each support level
    public List<RewardGiver> gabeRewards;
    public List<RewardGiver> mikeRewards;
    public List<RewardGiver> raphRewards;

    /// <summary>
    /// 
    /// </summary>
    public void UnlockSupport(int character)
    {
        //Checks if one of the three are properly selected
        if (character != 1 && character != 2 && character != 3)
        {
            //Returns if the selection is invalid
            Debug.Log("Invalid character to level up support");
            return;
        }

        //Unlocks the support level for the specific character
        switch (character)
        {
            case 1:
                gabeLocked = false;
                break;
            case 2:
                mikeLocked = false;
                break;
            case 3:
                raphLocked = false;
                break;
        }

    }

    /// <summary>
    /// Levels up the support level for one of the 3 characters by passing in an int
    /// 1 - Gabe,
    /// 2 - Mike,
    /// 3 - Raph
    /// </summary>
    public void SupportLevelUp(int character)
    {
        //Checks if one of the three are properly selected
        if(character != 1 && character != 2 && character != 3)
        {
            //Returns if the selection is invalid
            Debug.Log("Invalid character to level up support");
            return;
        }

        //Depending on the character, depends on which one gets leveled up while getting the rewards
        switch(character)
        {
            case 1:

                if(gabeLevel < gabeSupportConvos.Count)
                {
                    Debug.Log("Gabe Support level up!");
                    gabeLevel++;
                    //gabeRewards[gabeLevel - 1].GiveReward(); Uncomment when you make the rewards system
                }
                else
                {
                    Debug.Log("Gabe Support level is max.");
                }
                break;
            case 2:
                if (mikeLevel < mikeSupportConvos.Count)
                {
                    Debug.Log("Mike Support level up!");
                    mikeLevel++;
                    //mikeRewards[mikeLevel - 1].GiveReward(); Uncomment when you make the rewards system
                }
                else
                {
                    Debug.Log("Mike Support level is max.");
                }
                break;
            case 3:
                if (raphLevel < raphSupportConvos.Count)
                {
                    Debug.Log("Raph Support level up!");
                    raphLevel++;
                    //raphRewards[raphLevel - 1].GiveReward(); Uncomment when you make the rewards system
                }
                else
                {
                    Debug.Log("Raph Support level is max.");
                }
                break;
        }

    }

    //Make sure to eventually incorporate and add progression to the quest and support system
}
