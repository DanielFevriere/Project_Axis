using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsTab : GameTab
{
    public SkillHolder gabeSkillHolder;
    public SkillHolder mikeSkillHolder;
    public SkillHolder raphSkillHolder;

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
    public List<TMP_Text> availableSkillNames;

    public override void Refresh()
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
                    for (int i = 0; i < gabeSkillHolder.skillList.Count; i++)
                    {
                        availableSkillsList.Add(gabeSkillHolder.skillList[i]);
                    }
                    break;

                case 1:
                    for (int i = 0; i < raphSkillHolder.skillList.Count; i++)
                    {
                        availableSkillsList.Add(raphSkillHolder.skillList[i]);
                    }
                    break;

                case 2:
                    for (int i = 0; i < mikeSkillHolder.skillList.Count; i++)
                    {
                        availableSkillsList.Add(mikeSkillHolder.skillList[i]);
                    }
                    break;

            }
        }
        else if(skillsPage == 1)
        {
        }

        //Updates names and icons for each skill slot
        for (int i = 0; i < availableSkillIcons.Count; i++)
        {
            availableSkillIcons[i].sprite = availableSkillsList[i].skillIcon;
            availableSkillNames[i].text = availableSkillsList[i].skillName;
        }

        equippedSkillIcon1.sprite = availableSkillIcons[0].sprite;
        equippedSkillIcon2.sprite = availableSkillIcons[1].sprite;
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
        if (characterIndex == GameManager.Instance.partyMembers.Count - 1)
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
            characterIndex = GameManager.Instance.partyMembers.Count - 1;
        }
        else
        {
            characterIndex--;
        }
        Refresh();
    }

    public void Activate()
    {
        SwitchSkillPage(0);
    }


    public void DisplaySkill(int skillIndex)
    {
        if(skillIndex < availableSkillsList.Count)
        {
            skillNameDisplay.text = availableSkillsList[skillIndex].skillName;
            skillDescriptionDisplay.text = availableSkillsList[skillIndex].skillDescription;
        }
    }
}