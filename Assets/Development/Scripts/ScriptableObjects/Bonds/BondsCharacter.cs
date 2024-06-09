using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bonds Character")]
public class BondsCharacter : ScriptableObject
{
    //Character Name and icon
    public string characterName;
    public Sprite characterIcon;
    public Sprite characterPortrait;

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

    //The initial default convo when a character bond is locked for a character
    public Conversation gabeDefaultConvo;
    public Conversation mikeDefaultConvo;
    public Conversation raphDefaultConvo;

    //The progress convos for each character
    public Conversation gabeProgressConvo;
    public Conversation mikeProgressConvo;
    public Conversation raphProgressConvo;

    //List of convos for each bond level
    public List<Conversation> gabeBondsConvos;
    public List<Conversation> mikeBondsConvos;
    public List<Conversation> raphBondsConvos;

    //List of details that will be unlocked at each level
    //The number represents which will be unlocked
    /// <summary>
    /// 1: Age
    /// 2: Height
    /// 3: Favorite Color
    /// 4: Body Type
    /// 5: Likes
    /// 6: Dislikes
    /// 7: Skin Color
    /// 8: Motto
    /// 9: Hobbies
    /// 10: Fears
    /// 11: Goals
    /// 12: Personality
    /// 13: Strengths
    /// 14: Weaknesses
    /// 15: Question 1
    /// 16: Question 2
    /// 17: Question 3
    /// 18: Question 4
    /// 19: Question 5
    /// </summary>

    public List<int> gabeLevel0Details;
    public List<int> gabeLevel1Details;
    public List<int> gabeLevel2Details;
    public List<int> gabeLevel3Details;

    public List<int> mikeLevel0Details;
    public List<int> mikeLevel1Details;
    public List<int> mikeLevel2Details;
    public List<int> mikeLevel3Details;

    public List<int> raphLevel0Details;
    public List<int> raphLevel1Details;
    public List<int> raphLevel2Details;
    public List<int> raphLevel3Details;

    //Character Info
    public string age;
    public string height;
    public string favColor;
    public string bodyType;
    public string likes;
    public string dislikes;
    public string skinColor;
    public string motto;
    public string hobbies;
    public string fears;
    public string goals;
    public string personality;
    public string strengths;
    public string weaknesses;
    public string question1;
    public string question2;
    public string question3;
    public string question4;
    public string question5;

    //Bools
    public bool ageLocked;
    public bool heightLocked;
    public bool favColorLocked;
    public bool goalsLocked;
    public bool likesLocked;
    public bool dislikesLocked;
    public bool personalityLocked;
    public bool strengthsLocked;
    public bool weaknessesLocked;
    public bool bodyTypeLocked;
    public bool mottoLocked;
    public bool hobbiesLocked;
    public bool fearsLocked;
    public bool skinColorLocked;
    public bool question1Locked;
    public bool question2Locked;
    public bool question3Locked;
    public bool question4Locked;
    public bool question5Locked;



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
                }
                else
                {
                    Debug.Log("Raph Bond level is max.");
                }
                break;
        }

    }

}
