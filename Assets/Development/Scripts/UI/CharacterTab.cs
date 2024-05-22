using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterTab : GameTab
{
    public int characterIndex = 0;

    public TMP_Text currentCharacterName;
    public Image currentCharacterImage;

    public Sprite gabeImage;
    public Sprite mikeImage;
    public Sprite raphImage;

    public TMP_Text veniText;
    public TMP_Text hpText;
    public TMP_Text spText;
    public TMP_Text atkText;
    public TMP_Text defText;
    public TMP_Text spdText;

    public string itemNameText;
    public string itemNameDescription;

    public override void Refresh()
    {
        veniText.text = "Veni: " + GameManager.Instance.veni.ToString();
        UpdateCharacterDisplay();
        UpdateStatDisplay();
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

    void UpdateStatDisplay()
    {
        Stats c = GameManager.Instance.partyMembers[characterIndex].GetComponent<Stats>();
        hpText.text = c.GetStat(Stat.HP).ToString();
        spText.text = c.GetStat(Stat.SP).ToString();
        atkText.text = c.GetStat(Stat.ATK).ToString();
        defText.text = c.GetStat(Stat.DEF).ToString();
        spdText.text = c.GetStat(Stat.SPD).ToString();
    }

    public void NextCharacter()
    {
        //If you are at the end of the partymembers list, set the index to 0, otherwise increase
        if(characterIndex == GameManager.Instance.partyMembers.Count)
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
        //If you are at the beginning of the partymembers list, set the index to the final position, otherwise decrease

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