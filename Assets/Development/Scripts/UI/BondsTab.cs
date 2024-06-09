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
    public string currentBondName;
    public string currentBondLevel;
    public List<GameObject> detailsPages;
    public int bondsDetailsPage = 1;
    public List<TMP_Text> detailsTexts;
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

        //Adds all the texts to a list
        detailsTexts = new List<TMP_Text>();
        detailsTexts.Add(ageText);
        detailsTexts.Add(heightText);
        detailsTexts.Add(favColorText);
        detailsTexts.Add(bodyTypeText);
        detailsTexts.Add(likesText);
        detailsTexts.Add(dislikesText);
        detailsTexts.Add(skinColorText);
        detailsTexts.Add(mottoText);
        detailsTexts.Add(hobbiesText);
        detailsTexts.Add(fearsText);
        detailsTexts.Add(goalsText);
        detailsTexts.Add(personalityText);
        detailsTexts.Add(strengthsText);
        detailsTexts.Add(weaknessesText);
        detailsTexts.Add(question1Text);
        detailsTexts.Add(question2Text);
        detailsTexts.Add(question3Text);
        detailsTexts.Add(question4Text);
        detailsTexts.Add(question5Text);
    }

    public override void Refresh()
    {
        //To do list:
        //Grab the total list of bonds from the bonds menu and display them up properly here
        //Use the quest thing to figure out whats right

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
        for (int i = 0; i < bondsDetailsPage; i++)
        {
            //Displays the proper one
            if(currentBondsPage == bondsDetailsPage)
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

    public void UpdateBondsDetailsDisplay()
    {
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
        for (int i = 0; i < detailsPages.Count; i++)
        {
            //Turns off all the other pages and sets the selected page number to true
            if(i != pageNum)
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