using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondsManager : MonoBehaviour
{
    #region Global static reference
    private static BondsManager instance;
    public static BondsManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<BondsManager>();
                return instance;
            }
        }
    }
    #endregion

    public List<BondsCharacter> bondsCharacters;

    private void Awake()
    {
        
    }
    private void Update()
    {
        
    }

    /// <summary>
    /// Levels up the bonds level for one of the 3 characters by passing in an int
    /// 1 - Gabe,
    /// 2 - Mike,
    /// 3 - Raph
    /// </summary>
    public void BondLevelUp(BondsCharacter bondsChar, int partyMember)
    {
        //Checks if one of the three are properly selected
        if (partyMember != 1 && partyMember != 2 && partyMember != 3)
        {
            //Returns if the selection is invalid
            Debug.Log("Invalid character to level up bond");
            return;
        }

        //Depending on the character, depends on which one gets leveled up while getting the rewards
        switch (partyMember)
        {
            case 1:
                if (bondsChar.gabeLevel < bondsChar.gabeBondsConvos.Count)
                {
                    Debug.Log("Gabe Bond level up!");
                    bondsChar.gabeLevel++;
                }
                else
                {
                    Debug.Log("Gabe Bond level is max.");
                }
                break;
            case 2:
                if (bondsChar.mikeLevel < bondsChar.mikeBondsConvos.Count)
                {
                    Debug.Log("Mike Bond level up!");
                    bondsChar.mikeLevel++;
                }
                else
                {
                    Debug.Log("Mike Bond level is max.");
                }
                break;
            case 3:
                if (bondsChar.raphLevel < bondsChar.raphBondsConvos.Count)
                {
                    Debug.Log("Raph Bond level up!");
                    bondsChar.raphLevel++;
                }
                else
                {
                    Debug.Log("Raph Bond level is max.");
                }
                break;
        }

        UpdateDetails(bondsChar);
    }
    /// <summary>
    /// Unlocks support character progression for either gabe mike or raph depending on the partymember int paramaeter
    /// </summary>
    public void UnlockBond(BondsCharacter bondsChar, int partyMember)
    {
        //Checks if one of the three are properly selected
        if (partyMember != 1 && partyMember != 2 && partyMember != 3)
        {
            //Returns if the selection is invalid
            Debug.Log("Invalid character to level up support");
            return;
        }

        //Unlocks the support level for the specific character
        switch (partyMember)
        {
            case 1:
                bondsChar.gabeLocked = false;
                break;
            case 2:
                bondsChar.mikeLocked = false;
                break;
            case 3:
                bondsChar.raphLocked = false;
                break;
        }

        UpdateDetails(bondsChar);
    }

    /// <summary>
    /// Updates and unlocks details based on the bondslevels on a character
    /// </summary>
    /// <param name="bondsChar"></param>
    public void UpdateDetails(BondsCharacter bondsChar)
    { 
        //If gabes bond progress isnt locked
        if(!bondsChar.gabeLocked)
        {
            switch(bondsChar.gabeLevel)
            {
                //Depending on the bonds character level, depends on which details are unlocked
                case 0:
                    UnlockDetails(bondsChar, bondsChar.gabeLevel0Details);
                    break;
                case 1:
                    UnlockDetails(bondsChar, bondsChar.gabeLevel1Details);
                    break;
                case 2:
                    UnlockDetails(bondsChar, bondsChar.gabeLevel2Details);
                    break;
                case 3:
                    UnlockDetails(bondsChar, bondsChar.gabeLevel3Details);
                    break;
            }
        }

        //If mikes bond progress isnt locked
        if (!bondsChar.mikeLocked)
        {
            switch (bondsChar.mikeLevel)
            {
                //Depending on the bonds character level, depends on which details are unlocked
                case 0:
                    UnlockDetails(bondsChar, bondsChar.mikeLevel0Details);
                    break;
                case 1:
                    UnlockDetails(bondsChar, bondsChar.mikeLevel1Details);
                    break;
                case 2:
                    UnlockDetails(bondsChar, bondsChar.mikeLevel2Details);
                    break;
                case 3:
                    UnlockDetails(bondsChar, bondsChar.mikeLevel3Details);
                    break;
            }
        }

        //If raphs bond progress isnt locked
        if (!bondsChar.raphLocked)
        {
            switch (bondsChar.raphLevel)
            {
                //Depending on the bonds character level, depends on which details are unlocked
                case 0:
                    UnlockDetails(bondsChar, bondsChar.raphLevel0Details);
                    break;
                case 1:
                    UnlockDetails(bondsChar, bondsChar.raphLevel1Details);
                    break;
                case 2:
                    UnlockDetails(bondsChar, bondsChar.raphLevel2Details);
                    break;
                case 3:
                    UnlockDetails(bondsChar, bondsChar.raphLevel3Details);
                    break;
            }
        }
    }

    //Check the list of ints to see what detail will be unlocked
    void UnlockDetails(BondsCharacter bondsChar, List<int> unlockList)
    {
        for (int i = 0; i < unlockList.Count; i++)
        {
            switch (unlockList[i])
            {
                case 1:
                    bondsChar.ageLocked = false;
                    break;
                case 2:
                    bondsChar.heightLocked = false;
                    break;
                case 3:
                    bondsChar.favColorLocked = false;
                    break;
                case 4:
                    bondsChar.bodyTypeLocked = false;
                    break;
                case 5:
                    bondsChar.likesLocked = false;
                    break;
                case 6:
                    bondsChar.dislikesLocked = false;
                    break;
                case 7:
                    bondsChar.skinColorLocked = false;
                    break;
                case 8:
                    bondsChar.mottoLocked = false;
                    break;
                case 9:
                    bondsChar.hobbiesLocked = false;
                    break;
                case 10:
                    bondsChar.fearsLocked = false;
                    break;
                case 11:
                    bondsChar.goalsLocked = false;
                    break;
                case 12:
                    bondsChar.personalityLocked = false;
                    break;
                case 13:
                    bondsChar.strengthsLocked = false;
                    break;
                case 14:
                    bondsChar.weaknessesLocked = false;
                    break;
                case 15:
                    bondsChar.question1Locked = false;
                    break;
                case 16:
                    bondsChar.question2Locked = false;
                    break;
                case 17:
                    bondsChar.question3Locked = false;
                    break;
                case 18:
                    bondsChar.question4Locked = false;
                    break;
                case 19:
                    bondsChar.question5Locked = false;
                    break;
            }
        }
    }
}
