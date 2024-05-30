using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BondsDialogueNPC : MonoBehaviour
{
    [SerializeField] string characterName;
    public bool inTalkingDistance;
    public Conversation lockedConvo;

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
                    //If the characters bond is not locked
                    if(!testBondsCharacter.gabeLocked)
                    {
                        //Check to see if any bonds quest is currently accepted
                        //Check to see if any bonds quest is completed but checked in

                        //If they don't have any bonds quests accepted, accept the bonds quest for the said level
                        //If they have a bonds quest accepted, check for completion

                        //if incomplete, play the progression convo
                        //if complete, finish the quest to recieve rewards etc, but also level up the support level
                        //AND play the convo for the next bond level afterwards

                        //Going through active quests
                        for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
                        {
                            //Going through the character's bond quests
                            for (int j = 0; j < testBondsCharacter.gabeQuests.Count; j++)
                            {
                                //If the active quests is equal to the bond quest
                                if (testBondsCharacter.gabeQuests[j] == QuestManager.Instance.activeQuests[i])
                                {
                                    //if the active quest is complete
                                    if(QuestManager.Instance.CheckIfQuestComplete(testBondsCharacter.gabeQuests[j]))
                                    {
                                        //Finish up the quest and give the rewards to the player
                                        QuestManager.Instance.FinishQuest(testBondsCharacter.gabeQuests[j]);
                                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeBondsConvos[testBondsCharacter.gabeLevel]));
                                        testBondsCharacter.BondLevelUp(1);
                                        //You need a return, but ur currently in update, make a function
                                    }
                                    //Otherwise, play the progress convo
                                    else
                                    {
                                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeProgressConvo));
                                    }
                                }
                            }
                        }


                        switch(testBondsCharacter.gabeLevel)
                        {
                            case 0:
                                StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeBondsConvos[0]));
                                QuestManager.Instance.AcceptQuest(testBondsCharacter.gabeQuests[0]);
                                break;

                            case 1:
                                break;

                            case 2:
                                break;

                            case 3:
                                break;

                        }
                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeBondsConvos[0]));
                    }
                    //If it is locked,
                    else
                    {
                        //Play locked convo
                        StartCoroutine(DialogueManager.Instance.ShowConversation(lockedConvo));
                    }
                    break;

                case "Michael":
                    //If michael's bond is not locked
                    if (!testBondsCharacter.mikeLocked)
                    {

                    }
                    break;

                case "Raphael":
                    //If raphael's bond is not locked
                    if (!testBondsCharacter.raphLocked)
                    {

                    }
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
