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

    BondsCharacter targetBondsCharacter;

    void Awake()
    {
        HideNPCOptions();

        GameManager.Instance.OnStateChange += ShowInteractable;
        GameManager.Instance.OnStateChange += HideInteractable;

        //Finds the bonds character in the manager
        for (int i = 0; i < BondsManager.Instance.bondsCharacters.Count; i++)
        {
            if (characterName == BondsManager.Instance.bondsCharacters[i].characterName)
            {
                targetBondsCharacter = BondsManager.Instance.bondsCharacters[i];
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
            ShowNPCOptions();
        }
    }

    //Will either accept, check progress on, or complete a quest
    public void Talk()
    {
        HideNPCOptions();

        //Depending on whos being talked to
        switch (GameManager.Instance.partyLeader.name)
        {
            case "Gabriel":
                //If the characters bond is not locked
                if (!targetBondsCharacter.gabeLocked)
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
                                StartCoroutine(DialogueManager.Instance.ShowConversation(targetBondsCharacter.gabeBondsConvos[targetBondsCharacter.gabeLevel]));
                                BondsManager.Instance.BondLevelUp(targetBondsCharacter, 1);

                                //exit function
                                return;
                            }
                            //Otherwise, play the progress convo
                            else
                            {
                                StartCoroutine(DialogueManager.Instance.ShowConversation(targetBondsCharacter.gabeProgressConvo));
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
                if (!targetBondsCharacter.mikeLocked)
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
                                StartCoroutine(DialogueManager.Instance.ShowConversation(targetBondsCharacter.mikeBondsConvos[targetBondsCharacter.mikeLevel]));
                                BondsManager.Instance.BondLevelUp(targetBondsCharacter, 2);

                                //exit function
                                return;
                            }
                            //Otherwise, play the progress convo
                            else
                            {
                                StartCoroutine(DialogueManager.Instance.ShowConversation(targetBondsCharacter.mikeProgressConvo));
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
                if (!targetBondsCharacter.raphLocked)
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
                                StartCoroutine(DialogueManager.Instance.ShowConversation(targetBondsCharacter.raphBondsConvos[targetBondsCharacter.raphLevel]));
                                BondsManager.Instance.BondLevelUp(targetBondsCharacter, 3);

                                //exit function
                                return;
                            }
                            //Otherwise, play the progress convo
                            else
                            {
                                StartCoroutine(DialogueManager.Instance.ShowConversation(targetBondsCharacter.raphProgressConvo));
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
        UiManager.Instance.bondsMenu.currentBondsCharacter = targetBondsCharacter;
    }

    void AcceptQuest()
    {
        QuestManager.Instance.AcceptQuest(questToAccept);
        questToAccept = null;
        DialogueManager.Instance.OnCloseDialogue -= AcceptQuest;
    }

    public void GiveGift(BoosterItem gift)
    {
        targetBondsCharacter.gabeXp += gift.bondsExpAmount;
        InventoryManager.Instance.boosterInventory.Remove(gift);
    }

    public void ExitTalk()
    {
        HideNPCOptions();
        GameManager.Instance.ChangeState(GameState.FreeRoam);
    }

    /// <summary>
    /// If the game is in freeroam, show the interactables
    /// </summary>
    public void ShowInteractable()
    {
        if(GameManager.Instance.CurrentState == GameState.FreeRoam)
        {
            interactableUI.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
            DialogueManager.Instance.OnCloseDialogue -= ShowInteractable;
        }
    }

    /// <summary>
    /// If the game is not in freeroam, hide the interactables
    /// </summary>
    public void HideInteractable()
    {
        if(GameManager.Instance.CurrentState != GameState.FreeRoam)
        {
            interactableUI.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
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
