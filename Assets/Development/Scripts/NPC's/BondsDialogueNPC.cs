using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BondsDialogueNPC : MonoBehaviour
{
    [SerializeField] string characterName;
    public bool inTalkingDistance;

    BondsCharacter testBondsCharacter;

    void Awake()
    {
        //Finds the bonds character in the manager
        for (int i = 0; i < BondsManager.Instance.bondsCharacters.Count; i++)
        {
            if (characterName == BondsManager.Instance.bondsCharacters[i].characterName)
            {
                testBondsCharacter = BondsManager.Instance.bondsCharacters[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //If you are in talking distance,
        if (inTalkingDistance && GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
        {
            //Depending on whos being talked to
            switch(GameManager.Instance.partyLeader.name)
            {
                case "Gabriel":

                    break;

                case "Michael":

                    break;

                case "Raphael":

                    break;
            }
            //Depending on a shitton of circumstances, stemming all from bonds data, depends on
            //1, what conversation I want to be played
            //and 2 what Quest needs to be accepted
        }
    }


    void OnTriggerEnter(Collider other)
    {
        inTalkingDistance = true;

    }
    private void OnTriggerExit(Collider other)
    {
        inTalkingDistance = false;
    }
}
