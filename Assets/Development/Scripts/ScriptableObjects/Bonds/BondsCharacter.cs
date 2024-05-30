using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bonds Character")]
public class BondsCharacter : ScriptableObject
{
    //Character Name and icon
    public string characterName;
    public Sprite characterIcon;

    //if the character is locked, it cannot be progressed yet
    public bool gabeLocked;
    public bool mikeLocked;
    public bool raphLocked;

    //Character bonds levels with the main crew
    public int gabeLevel = 0;
    public int mikeLevel = 0;
    public int raphLevel = 0;


    //Character bonds quests
    public List<Quest> gabeQuests;
    public List<Quest> mikeQuests;
    public List<Quest> raphQuests;

    //List of convos for each support level
    public List<Conversation> gabeBondsConvos;
    public List<Conversation> mikeBondsConvos;
    public List<Conversation> raphBondsConvos;

    //Character Info
    public string age;
    public string height;
    public string favColor;
    public string goals;
    public string likes;
    public string dislikes;
    public string personality;

    public bool ageLocked;
    public bool heightLocked;
    public bool favColorLocked;
    public bool goalsLocked;
    public bool likesLocked;
    public bool dislikesLocked;
    public bool personalityLocked;

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
    /// Levels up the bonds level for one of the 3 characters by passing in an int
    /// 1 - Gabe,
    /// 2 - Mike,
    /// 3 - Raph
    /// </summary>
    public void BondLevelUp(int character)
    {
        //Checks if one of the three are properly selected
        if(character != 1 && character != 2 && character != 3)
        {
            //Returns if the selection is invalid
            Debug.Log("Invalid character to level up bond");
            return;
        }

        //Depending on the character, depends on which one gets leveled up while getting the rewards
        switch(character)
        {
            case 1:

                if(gabeLevel < gabeBondsConvos.Count)
                {
                    Debug.Log("Gabe Bond level up!");
                    gabeLevel++;
                    //gabeRewards[gabeLevel - 1].GiveReward(); Uncomment when you make the rewards system
                }
                else
                {
                    Debug.Log("Gabe Bond level is max.");
                }
                break;
            case 2:
                if (mikeLevel < mikeBondsConvos.Count)
                {
                    Debug.Log("Mike Bond level up!");
                    mikeLevel++;
                    //mikeRewards[mikeLevel - 1].GiveReward(); Uncomment when you make the rewards system
                }
                else
                {
                    Debug.Log("Mike Bond level is max.");
                }
                break;
            case 3:
                if (raphLevel < raphBondsConvos.Count)
                {
                    Debug.Log("Raph Bond level up!");
                    raphLevel++;
                    //raphRewards[raphLevel - 1].GiveReward(); Uncomment when you make the rewards system
                }
                else
                {
                    Debug.Log("Raph Bond level is max.");
                }
                break;
        }

    }

    //Make sure to eventually incorporate and add progression to the quest and support system
}
