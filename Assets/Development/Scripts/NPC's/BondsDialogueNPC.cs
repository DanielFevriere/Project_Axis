using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BondsDialogueNPC : MonoBehaviour
{
    [SerializeField] string characterName;
    public bool inTalkingDistance;
    public GameObject interactableUI;
    public GameObject npcOptions;
    public Conversation lockedConvo;

    public Conversation gabeQuestConvo;
    public Conversation mikeQuestConvo;
    public Conversation raphQuestConvo;

    public Quest gabeQuest;
    public Quest mikeQuest;
    public Quest raphQuest;

    Quest questToAccept;

    BondsCharacter testBondsCharacter;

    void Awake()
    {
        ShowInteractable();
        HideNPCOptions();

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
            GameManager.Instance.ChangeState(GameState.Freeze);
            HideInteractable();
            ShowNPCOptions();
        }
    }

    //Will either accept, check progress on, or complete a quest
    public void Talk()
    {
        DialogueManager.Instance.OnCloseDialogue += ShowInteractable;
        HideNPCOptions();

        //Depending on whos being talked to
        switch (GameManager.Instance.partyLeader.name)
        {
            case "Gabriel":
                //If the characters bond is not locked
                if (!testBondsCharacter.gabeLocked)
                {
                    //Going through active quests
                    for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
                    {
                        //If the accepted quest is equal to the bond quest
                        if (gabeQuest == QuestManager.Instance.activeQuests[i])
                        {
                            //if the active quest is complete
                            if (QuestManager.Instance.CheckIfQuestComplete(gabeQuest))
                            {
                                //Finish up the quest and give the rewards to the player
                                QuestManager.Instance.FinishQuest(gabeQuest);
                                StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeBondsConvos[testBondsCharacter.gabeLevel]));
                                BondsManager.Instance.BondLevelUp(testBondsCharacter, 1);

                                //exit function
                                return;
                            }
                            //Otherwise, play the progress convo
                            else
                            {
                                StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeProgressConvo));
                                return;
                            }
                        }
                    }

                    //If it reaches this point then it means that no bonds quest has been accepted, therefore

                    //Play the starter convo
                    StartCoroutine(DialogueManager.Instance.ShowConversation(gabeQuestConvo));

                    //Accepts a quest
                    questToAccept = gabeQuest;
                    DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                }
                //If it is locked,
                else
                {
                    //Play locked convo
                    StartCoroutine(DialogueManager.Instance.ShowConversation(lockedConvo));
                }
                break;

            case "Michael":
                //If the characters bond is not locked
                if (!testBondsCharacter.mikeLocked)
                {
                    //Going through active quests
                    for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
                    {
                        //If the accepted quest is equal to the bond quest
                        if (mikeQuest == QuestManager.Instance.activeQuests[i])
                        {
                            //if the active quest is complete
                            if (QuestManager.Instance.CheckIfQuestComplete(mikeQuest))
                            {
                                //Finish up the quest and give the rewards to the player
                                QuestManager.Instance.FinishQuest(mikeQuest);
                                StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.mikeBondsConvos[testBondsCharacter.mikeLevel]));
                                BondsManager.Instance.BondLevelUp(testBondsCharacter, 2);

                                //exit function
                                return;
                            }
                            //Otherwise, play the progress convo
                            else
                            {
                                StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.mikeProgressConvo));
                                return;
                            }
                        }
                    }

                    //If it reaches this point then it means that no bonds quest has been accepted, therefore

                    //Play the starter convo
                    StartCoroutine(DialogueManager.Instance.ShowConversation(mikeQuestConvo));

                    //Accepts a quest
                    questToAccept = mikeQuest;
                    DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                }
                //If it is locked,
                else
                {
                    //Play locked convo
                    StartCoroutine(DialogueManager.Instance.ShowConversation(lockedConvo));
                }
                break;

            case "Raphael":
                //If the characters bond is not locked
                if (!testBondsCharacter.raphLocked)
                {
                    //Going through active quests
                    for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
                    {
                        //If the accepted quest is equal to the bond quest
                        if (raphQuest == QuestManager.Instance.activeQuests[i])
                        {
                            //if the active quest is complete
                            if (QuestManager.Instance.CheckIfQuestComplete(raphQuest))
                            {
                                //Finish up the quest and give the rewards to the player
                                QuestManager.Instance.FinishQuest(raphQuest);
                                StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.raphBondsConvos[testBondsCharacter.raphLevel]));
                                BondsManager.Instance.BondLevelUp(testBondsCharacter, 3);

                                //exit function
                                return;
                            }
                            //Otherwise, play the progress convo
                            else
                            {
                                StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.raphProgressConvo));
                                return;
                            }
                        }
                    }

                    //If it reaches this point then it means that no bonds quest has been accepted, therefore

                    //Play the starter convo
                    StartCoroutine(DialogueManager.Instance.ShowConversation(raphQuestConvo));

                    //Accepts a quest
                    questToAccept = raphQuest;
                    DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                }
                //If it is locked,
                else
                {
                    //Play locked convo
                    StartCoroutine(DialogueManager.Instance.ShowConversation(lockedConvo));
                }
                break;
        }
    }

    public void Bond()
    {
        HideNPCOptions();
        GameManager.Instance.SetCurrentCamera(GameManager.Instance.BondsMenuCamera);
        UiManager.Instance.bondsMenu.ToggleVisibility();
        UiManager.Instance.bondsMenu.currentBondsCharacter = testBondsCharacter;
    }

    void AcceptQuest()
    {
        QuestManager.Instance.AcceptQuest(questToAccept);
        questToAccept = null;
        DialogueManager.Instance.OnCloseDialogue -= AcceptQuest;
    }

    public void GiveGift(BoosterItem gift)
    {
        //If the gift belongs to the right bonds character,
        if(gift.requiredBondsCharacter == testBondsCharacter)
        {
            //If the right party member is giving the gift, 
            if (gift.requiredPartyMember == GameManager.Instance.partyLeader.name)
            {
                //Play a thank you conversation for the gift? have the conversation be attached from the boosteritem
                //Play a cool effect on the npc as well
                //Discard the gift item from the inventory
                //add the amount of bonds exp too
            }
        }


    }

    public void ShowInteractable()
    {
        interactableUI.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }

    public void HideInteractable()
    {
        interactableUI.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void ShowNPCOptions()
    {
        GameManager.Instance.SetCurrentCamera(GameManager.Instance.ZoomedCamera);
        npcOptions.SetActive(true);
    }

    public void HideNPCOptions()
    {
        npcOptions.SetActive(false);
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
