using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BondsTab : GameTab
{
    BondsManager bondsManager;

    public int currentBondsPage = 1;
    public int bondsPageAmount;
    public GameObject previousPageButton;
    public GameObject nextPageButton;
    public List<GameObject> bondsList;
    public List<TMP_Text> bondsDisplayNames;
    public List<Image> bondsDisplayIcons;
    public List<GameObject> bondsDisplayBgs;

    //Variables relating to the currently selected bonds character
    public BondsCharacter currentBond;
    public List<GameObject> detailsPages;
    public int currentBondsDetailsPage = 1;
    public TMP_Text currentBondsCharacterName;
    public Image currentBondsCharacterPortrait;
    public TMP_Text GabeLvText;
    public TMP_Text MikeLvText;
    public TMP_Text RaphLvText;
    public Material GabeBarMaterial;
    public Material MikeBarMaterial;
    public Material RaphBarMaterial;

    public TMP_Text ageText;
    public TMP_Text heightText;
    public TMP_Text favColorText;
    public TMP_Text bodyTypeText;
    public TMP_Text likesText;
    public TMP_Text dislikesText;
    public TMP_Text skinColorText;
    public TMP_Text mottoText;
    public TMP_Text hobbiesText;
    public TMP_Text fearsText;
    public TMP_Text goalsText;
    public TMP_Text personalityText;
    public TMP_Text strengthsText;
    public TMP_Text weaknessesText;
    public TMP_Text question1Text;
    public TMP_Text question2Text;
    public TMP_Text question3Text;
    public TMP_Text question4Text;
    public TMP_Text question5Text;

    private void Awake()
    {
        bondsManager = BondsManager.Instance;

        currentBond = bondsManager.bondsCharacters[0];
    }

    public override void Refresh()
    {
        //Clears bonds list
        bondsList.Clear();

        //Displays all the bonds character details on the side
        //Names of the bonds character buttons on the side become the sidequest title
        for (int i = 0; i < bondsDisplayNames.Count; i++)
        {
            int targetIndex = bondsDisplayNames.Count * (currentBondsPage - 1) + i;

            //specific case where theres not enough characters to fill up a page, break from the loop
            if (targetIndex >= bondsManager.bondsCharacters.Count)
            {
                bondsDisplayNames[i].gameObject.SetActive(false);
                bondsDisplayIcons[i].gameObject.SetActive(false);
                bondsDisplayBgs[i].SetActive(false);
                continue;
            }

            //Finds the bonds character to show based on the bonds page number and size of the list of names needed to display
            BondsCharacter targetBond = bondsManager.bondsCharacters[bondsDisplayNames.Count * (currentBondsPage - 1) + i];

            //If the target bond is not null
            if (targetBond != null)
            {
                //Shows the gameobject
                bondsDisplayNames[i].gameObject.SetActive(true);
                bondsDisplayIcons[i].gameObject.SetActive(true);
                bondsDisplayBgs[i].SetActive(true);

                //Sets the character name
                bondsDisplayNames[i].text = targetBond.characterName;
                bondsDisplayIcons[i].sprite = targetBond.characterIcon;
            }
        }

        //Sets the page amount
        SetBondsPageAmount();

        //Displays the proper bonds details page
        for (int i = 0; i < detailsPages.Count; i++)
        {
            //Displays the proper one
            if (i == currentBondsDetailsPage)
            {
                detailsPages[i].SetActive(true);
            }
            //Disables the others
            else
            {
                detailsPages[i].SetActive(false);
            }
        }

        UpdateBondsDetailsDisplay();
    }

    public void SelectBond(int bondsIndex)
    {
        //Searches for the bond in the bonds character list
        for (int i = 0; i < bondsManager.bondsCharacters.Count; i++)
        {
            //If the selected bond is it
            if (bondsDisplayNames[bondsIndex].text == bondsManager.bondsCharacters[i].characterName)
            {
                currentBond = bondsManager.bondsCharacters[i];
            }
        }
    }

    public void UpdateBondsDetailsDisplay()
    {
        currentBondsCharacterName.text = currentBond.characterName;
        currentBondsCharacterPortrait.sprite = currentBond.characterPortrait;

        GabeLvText.text = "Gabe Lv " + currentBond.gabeLevel.ToString();
        MikeLvText.text = "Mike Lv " + currentBond.mikeLevel.ToString();
        RaphLvText.text = "Raph Lv " + currentBond.raphLevel.ToString();

        //so currently youll have the bar set up by current progress / required progress, BUT
        //IMPORTANT: IN THE FUTURE, CHANGE IT SO THAT IT COMBINES THE PROGRESS OF ALL CONDITIONS, NOT JUST ONE. ITS FLAWED AND THIS IS A TEMP SOLUTION.

        float gabeFill = (float)currentBond.gabeQuests[currentBond.gabeLevel].conditions[0].CurrentProgress / (float)currentBond.gabeQuests[currentBond.gabeLevel].conditions[0].RequiredProgress;
        float mikeFill = (float)currentBond.mikeQuests[currentBond.mikeLevel].conditions[0].CurrentProgress / (float)currentBond.mikeQuests[currentBond.mikeLevel].conditions[0].RequiredProgress;
        float raphFill = (float)currentBond.raphQuests[currentBond.raphLevel].conditions[0].CurrentProgress / (float)currentBond.raphQuests[currentBond.raphLevel].conditions[0].RequiredProgress;

        //If they are max level (3) it fills up to max already
        GabeBarMaterial.SetFloat("_Fill", currentBond.gabeLevel == 3 ? 1 : gabeFill);
        MikeBarMaterial.SetFloat("_Fill", currentBond.mikeLevel == 3 ? 1 : mikeFill);
        RaphBarMaterial.SetFloat("_Fill", currentBond.raphLevel == 3 ? 1 : raphFill);

        //Updates details based on if they are locked or not
        ageText.text = currentBond.ageLocked ? "???" : currentBond.age;
        heightText.text = currentBond.heightLocked ? "???" : currentBond.height;
        favColorText.text = currentBond.favColorLocked ? "???" : currentBond.favColor;
        bodyTypeText.text = currentBond.bodyTypeLocked ? "???" : currentBond.bodyType;
        likesText.text = currentBond.likesLocked ? "???" : currentBond.likes;
        dislikesText.text = currentBond.dislikesLocked ? "???" : currentBond.dislikes;
        skinColorText.text = currentBond.skinColorLocked ? "???" : currentBond.skinColor;
        mottoText.text = currentBond.mottoLocked ? "???" : currentBond.motto;
        hobbiesText.text = currentBond.hobbiesLocked ? "???" : currentBond.hobbies;
        fearsText.text = currentBond.fearsLocked ? "???" : currentBond.fears;
        goalsText.text = currentBond.goalsLocked ? "???" : currentBond.goals;
        personalityText.text = currentBond.personalityLocked ? "???" : currentBond.personality;
        strengthsText.text = currentBond.strengthsLocked ? "???" : currentBond.strengths;
        weaknessesText.text = currentBond.weaknessesLocked ? "???" : currentBond.weaknesses;
        question1Text.text = currentBond.question1Locked ? "???" : currentBond.question1;
        question2Text.text = currentBond.question2Locked ? "???" : currentBond.question2;
        question3Text.text = currentBond.question3Locked ? "???" : currentBond.question3;
        question4Text.text = currentBond.question4Locked ? "???" : currentBond.question4;
        question5Text.text = currentBond.question5Locked ? "???" : currentBond.question5;
    }

    //Displays the page based on the number
    public void ShowDetailsPage(int pageNum)
    {
        currentBondsDetailsPage = pageNum;

        for (int i = 0; i < detailsPages.Count; i++)
        {
            //Turns off all the other pages and sets the selected page number to true
            if (i != pageNum)
            {
                detailsPages[i].SetActive(false);
            }
            else
            {
                detailsPages[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Previous Bonds Page
    /// </summary>
    public void PreviousPage()
    {
        if (currentBondsPage != 1)
        {
            currentBondsPage--;
        }
    }

    /// <summary>
    /// Next Bonds Page
    /// </summary>
    public void NextPage()
    {
        if (currentBondsPage != bondsPageAmount)
        {
            currentBondsPage++;
        }
    }

    public void SetBondsPageAmount()
    {
        bondsPageAmount = bondsManager.bondsCharacters.Count / bondsDisplayNames.Count;

        if (bondsPageAmount == 0)
        {
            bondsPageAmount++;
        }
        else if ((bondsManager.bondsCharacters.Count % bondsDisplayNames.Count) != 0)
        {
            bondsPageAmount++;
        }

        //If the page number is 1, hide the previous page button
        if (currentBondsPage == 1)
        {
            previousPageButton.SetActive(false);
        }

        //If theres only one page, hide the next page button
        if (bondsPageAmount == 1)
        {
            nextPageButton.SetActive(false);
        }
        else
        {
            nextPageButton.SetActive(true);
        }

        //If the page number is greater than 1, show the previous page button
        if (currentBondsPage > 1)
        {
            previousPageButton.SetActive(true);

            //now pageAmount serves as the max amount of pages
            if (currentBondsPage == bondsPageAmount)
            {
                nextPageButton.SetActive(false);
            }
            else
            {
                nextPageButton.SetActive(true);
            }
        }
    }
}