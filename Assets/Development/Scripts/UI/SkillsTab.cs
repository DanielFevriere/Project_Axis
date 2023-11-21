using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsTab : GameTab
{
    SkillManager skillManager;

    public int characterIndex = 0;
    public int skillsPage = 0;

    public TMP_Text currentCharacterName;
    public Image currentCharacterImage;

    public Sprite gabeImage;
    public Sprite mikeImage;
    public Sprite raphImage;

    public TMP_Text skillNameDisplay;
    public TMP_Text skillDescriptionDisplay;

    public Image equippedSkillIcon1;
    public Image equippedSkillIcon2;

    public List<Skill> availableSkillsList;
    public List<Image> availableSkillIcons;
    public List<string> availableSkillNames;


    private void Start()
    {
        skillManager = SkillManager.Instance;
    }

    public void Refresh()
    {
        UpdateCharacterDisplay();
        UpdateSkillDisplay();
    }

    void UpdateSkillDisplay()
    {
        //Clears the current list of displayed skills
        availableSkillsList.Clear();

        //If its gabe mike or raph, it adds their list of unlocked individual skills to the list of available skills which will be displayed
        //If its on the team skills page, it just displays the list of team skills instead
        if (skillsPage == 0)
        {

            switch (characterIndex)
            {
                case 0:
                    for (int i = 0; i < skillManager.gabrielUnlockedSkills.Count; i++)
                    {
                        availableSkillsList.Add(skillManager.gabrielUnlockedSkills[i]);
                    }
                    break;

                case 1:
                    for (int i = 0; i < skillManager.michaelUnlockedSkills.Count; i++)
                    {
                        availableSkillsList.Add(skillManager.michaelUnlockedSkills[i]);
                    }
                    break;

                case 2:
                    for (int i = 0; i < skillManager.raphaelUnlockedSkills.Count; i++)
                    {
                        availableSkillsList.Add(skillManager.raphaelUnlockedSkills[i]);
                    }
                    break;
            }
        }
        else if(skillsPage == 1)
        {

            //If its gabe mike or raph, it adds their list of unlocked individual skills to the list of available skills which will be displayed
            switch (characterIndex)
            {
                case 0:
                    for (int i = 0; i < skillManager.gabrielUnlockedSkills.Count; i++)
                    {
                        availableSkillsList.Add(skillManager.gabrielUnlockedSkills[i]);
                    }
                    break;

                case 1:
                    for (int i = 0; i < skillManager.michaelUnlockedSkills.Count; i++)
                    {
                        availableSkillsList.Add(skillManager.michaelUnlockedSkills[i]);
                    }
                    break;

                case 2:
                    for (int i = 0; i < skillManager.raphaelUnlockedSkills.Count; i++)
                    {
                        availableSkillsList.Add(skillManager.raphaelUnlockedSkills[i]);
                    }
                    break;
            }

        }

        //Updates names for each skill slot
        for (int i = 0; i < availableSkillIcons.Count; i++)
        {
            availableSkillIcons[i] = availableSkillsList[i].icon;
            availableSkillNames[i] = availableSkillsList[i].skillName;
        }
    }

    void UpdateCharacterDisplay()
    {
        string characterName = GameManager.Instance.partyMembers[characterIndex].GetComponent<Controller>().characterName;

        currentCharacterName.text = characterName;

        if (characterName == "Gabriel")
        {
            currentCharacterImage.sprite = gabeImage;
        }
        else if (characterName == "Michael")
        {
            currentCharacterImage.sprite = mikeImage;
        }
        else if (characterName == "Raphael")
        {
            currentCharacterImage.sprite = raphImage;
        }
    }

    public void SwitchSkillPage(int page)
    {
        skillsPage = page;
        Refresh();
    }

    public void NextCharacter()
    {
        if (characterIndex == GameManager.Instance.partyMembers.Count)
        {
            characterIndex = 0;
        }
        else
        {
            characterIndex++;
        }
        Refresh();
    }

    public void PreviousCharacter()
    {
        if (characterIndex == 0)
        {
            characterIndex = GameManager.Instance.partyMembers.Count;
        }
        else
        {
            characterIndex--;
        }
        Refresh();
    }
}