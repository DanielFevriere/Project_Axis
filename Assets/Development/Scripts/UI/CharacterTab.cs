using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTab : GameTab
{
    public Image currentImage;

    public Image gabeImage;
    public Image mikeImage;
    public Image raphImage;

    public void Refresh()
    {
        string partyLeaderName = GameManager.Instance.partyLeader.GetComponent<Controller>().characterName;

        if(partyLeaderName == "Gabriel")
        {
            currentImage = gabeImage;
        }
        else if(partyLeaderName == "Michael")
        {
            currentImage = mikeImage;
        }
        else if(partyLeaderName == "Raphael")
        {
            currentImage = raphImage;
        }
    }
}