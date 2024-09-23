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

    //Character bonds current exp
    public int gabeXp = 0;
    public int mikeXp = 0;
    public int raphXp = 0;

    //These are the lists of intergers needed for them to hit their next bond level
    public List<int> gabeBondLevelRequirements;
    public List<int> mikeBondLevelRequirements;
    public List<int> raphBondLevelRequirements;

    //The initial starting convo before accepting the bond characters first quest
    public Conversation gabeStarterConvo;
    public Conversation mikeStarterConvo;
    public Conversation raphStarterConvo;

    //The progress convos for each character
    public Conversation gabeProgressConvo;
    public Conversation mikeProgressConvo;
    public Conversation raphProgressConvo;

    public Conversation gabeCompletedConvo;
    public Conversation mikeCompletedConvo;
    public Conversation raphCompletedConvo;

    //Convos depending on gifts given
    public Conversation giftSuccess;
    public Conversation wrongPerson;
    public Conversation wrongGift;

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

    [Header("Detail numbers: \n" +
        "1 = Age \n" +
        "2 = Height \n" +
        "3 = Favorite Color \n" +
        "4 = Body Type \n" +
        "5 = Likes \n" +
        "6 = Dislikes \n" +
        "7 = Skin Color \n" +
        "8 = Motto \n" +
        "9 = Hobbies \n" +
        "10 = Fears \n" +
        "11 = Goals \n" +
        "12 = Personality \n" +
        "13 = Strengths \n" +
        "14 = Weaknesses \n" +
        "15 = Question 1 \n" +
        "16 = Question 2 \n" +
        "17 = Question 3 \n" +
        "18 = Question 4 \n" +
        "19 = Question 5 \n")]

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
}
