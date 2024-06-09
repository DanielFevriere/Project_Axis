using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionAtlasTab : GameTab
{
    AtlasManager atlasManager;
    public EvolutionAtlas currentAtlas;

    public int characterIndex;
    public bool nodeSelected = false;
    public GameObject nodeDetailsDisplay;
    public TMP_Text nodeNameText;
    public TMP_Text nodeDescriptionText;
    public TMP_Text lockedText;

    public GameObject nodeHolder;
    public List<GameObject> displayedNodes;

    private void Awake()
    {
        atlasManager = AtlasManager.Instance;
        currentAtlas = atlasManager.characterAtlases[0];
    }

    public override void Refresh()
    {
        displayedNodes.Clear();
        currentAtlas = atlasManager.characterAtlases[characterIndex];

        //Node display turns on if a node is selected
        if(nodeSelected)
        {
            nodeDetailsDisplay.SetActive(true);
        }    
        else
        {
            nodeDetailsDisplay.SetActive(false);
        }

        //Automatically adds the child gameobjects to the displayednodes list
        for (int i = 0; i < nodeHolder.transform.childCount; i++)
        {
            if (nodeHolder.transform.GetChild(i) != null)
            {
                displayedNodes.Add(nodeHolder.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < displayedNodes.Count; i++)
        {
            //If the index is less than the current atlas node count, show the node
            if (i < currentAtlas.lockedNodes.Count)
            {
                displayedNodes[i].SetActive(true);
                //Gets the icon and sets it to the node icon
                displayedNodes[i].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = currentAtlas.lockedNodes[i].nodeIcon;

                //Fades/unfades based on if they are locked or unlocked
                if(currentAtlas.lockedNodes[i].locked)
                {
                    displayedNodes[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(0,0,0,0.7f);
                }
                else
                {
                    displayedNodes[i].transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
                }
            }
            else
            {
                displayedNodes[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Highlights and displays details of a selected node
    /// </summary>
    /// <param name="nodeNum"></param>
    public void SelectNode(int nodeNum)
    {
        Node currentNode = currentAtlas.lockedNodes[nodeNum];

        //Node Details display shows and sets the name and description to the selected node
        nodeSelected = true;
        nodeNameText.text = currentNode.nodeName;
        nodeDescriptionText.text = currentNode.nodeDescription;

        //Sets the locked text to locked/unlocked
        lockedText.text = currentNode.locked ? "Locked" : "Unlocked";

        Refresh();
    }

    /// <summary>
    /// Page 0 = Gabe's Tab
    /// Page 1 = Mike's Tab
    /// Page 2 = Raph's Tab
    /// </summary>
    /// <param name="page"></param>
    public void SwitchPage(int page)
    {
        characterIndex = page;
        Refresh();
    }

    private void OnDisable()
    {
        nodeSelected = false;
    }



}
