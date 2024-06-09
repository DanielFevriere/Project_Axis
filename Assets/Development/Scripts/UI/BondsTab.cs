using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondsTab : GameTab
{
    BondsManager bondsManager;

    public int bondsPage = 1;
    public List<GameObject> bondsList;

    //Variables relating to the currently selected bonds character
    public BondsCharacter currentBond;
    public string currentBondName;
    public string currentBondLevel;
    public List<GameObject> detailsPages;
    public int bondsDetailsPage = 1;

    private void Awake()
    {
        bondsManager = BondsManager.Instance;
    }

    public override void Refresh()
    {
        //To do list:
        //Grab the total list of bonds from the bonds menu and display them up properly here
        //Use the quest thing to figure out whats right
        //I fucked up, and I need to rework ui to display the bond level because I need to be showing 3 bars not just one.
    }

    public void DisplayBondCharacter(int bondsIndex)
    {

    }

    //Displays the page based on the number
    public void ShowDetailsPage(int pageNum)
    {

    }

    public void PreviousPage()
    {

    }

    public void NextPage()
    {

    }
}