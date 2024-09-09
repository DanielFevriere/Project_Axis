using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BondsDialogueNPC : MonoBehaviour
{
    [SerializeField] string characterName;
    public bool inTalkingDistance;
    public Conversation lockedConvo;
    public Quest questToAccept;

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
            Talk();
        }
    }

    public void Talk()
    {
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
                        //Going through the character's bond quests
                        for (int j = 0; j < testBondsCharacter.gabeQuests.Count; j++)
                        {
                            //If the accepted quest is equal to the bond quest
                            if (testBondsCharacter.gabeQuests[j] == QuestManager.Instance.activeQuests[i])
                            {
                                //if the active quest is complete
                                if (QuestManager.Instance.CheckIfQuestComplete(testBondsCharacter.gabeQuests[j]))
                                {
                                    //Finish up the quest and give the rewards to the player
                                    QuestManager.Instance.FinishQuest(testBondsCharacter.gabeQuests[j]);
                                    StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeBondsConvos[testBondsCharacter.gabeLevel]));
                                    BondsManager.Instance.BondLevelUp(testBondsCharacter, 1); 

                                    //If the bond level isnt maxed
                                    if(testBondsCharacter.gabeLevel != testBondsCharacter.gabeQuests.Count)
                                    {
                                        //Automatically accept the next bonds quest after the dialogue
                                        questToAccept = testBondsCharacter.gabeQuests[testBondsCharacter.gabeLevel];
                                        DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                                    }
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
                    }
                    //If it reaches this point then it means that no bonds quest has been accepted, therefore
                    if (testBondsCharacter.gabeLevel == 3)
                    {
                        //Play the completed convo if the bond is maxed
                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeCompletedConvo));
                    }
                    else
                    {
                        //Play the starter convo
                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.gabeStarterConvo));

                        //Accepts a quest
                        questToAccept = testBondsCharacter.gabeQuests[testBondsCharacter.gabeLevel];
                        DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                    }
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
                        //Going through the character's bond quests
                        for (int j = 0; j < testBondsCharacter.mikeQuests.Count; j++)
                        {
                            //If the accepted quest is equal to the bond quest
                            if (testBondsCharacter.mikeQuests[j] == QuestManager.Instance.activeQuests[i])
                            {
                                //if the active quest is complete
                                if (QuestManager.Instance.CheckIfQuestComplete(testBondsCharacter.mikeQuests[j]))
                                {
                                    //Finish up the quest and give the rewards to the player
                                    QuestManager.Instance.FinishQuest(testBondsCharacter.mikeQuests[j]);
                                    StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.mikeBondsConvos[testBondsCharacter.mikeLevel]));
                                    BondsManager.Instance.BondLevelUp(testBondsCharacter, 2);
                                    //If the bond level isnt maxed
                                    if (testBondsCharacter.mikeLevel != testBondsCharacter.mikeQuests.Count)
                                    {
                                        //Automatically accept the next bonds quest after the dialogue
                                        questToAccept = testBondsCharacter.mikeQuests[testBondsCharacter.mikeLevel];
                                        DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                                    }
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
                    }
                    //If it reaches this point then it means that no bonds quest has been accepted, therefore
                    if (testBondsCharacter.mikeLevel == 3)
                    {
                        //Play the completed convo if the bond is maxed
                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.mikeCompletedConvo));
                    }
                    else
                    {
                        //Play the starter convo
                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.mikeStarterConvo));

                        //Accepts a quest
                        questToAccept = testBondsCharacter.mikeQuests[testBondsCharacter.mikeLevel];
                        DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                    }
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
                        //Going through the character's bond quests
                        for (int j = 0; j < testBondsCharacter.raphQuests.Count; j++)
                        {
                            //If the accepted quest is equal to the bond quest
                            if (testBondsCharacter.raphQuests[j] == QuestManager.Instance.activeQuests[i])
                            {
                                //if the active quest is complete
                                if (QuestManager.Instance.CheckIfQuestComplete(testBondsCharacter.raphQuests[j]))
                                {
                                    //Finish up the quest and give the rewards to the player
                                    QuestManager.Instance.FinishQuest(testBondsCharacter.raphQuests[j]);
                                    StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.raphBondsConvos[testBondsCharacter.raphLevel]));
                                    BondsManager.Instance.BondLevelUp(testBondsCharacter, 3);

                                    //If the bond level isnt maxed
                                    if (testBondsCharacter.raphLevel != testBondsCharacter.raphQuests.Count)
                                    {
                                        //Automatically accept the next bonds quest after the dialogue
                                        questToAccept = testBondsCharacter.raphQuests[testBondsCharacter.raphLevel];
                                        DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                                    }
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
                    }
                    //If it reaches this point then it means that no bonds quest has been accepted, therefore
                    if (testBondsCharacter.raphLevel == 3)
                    {
                        //Play the completed convo if the bond is maxed
                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.raphCompletedConvo));
                    }
                    else
                    {
                        //Play the starter convo
                        StartCoroutine(DialogueManager.Instance.ShowConversation(testBondsCharacter.raphStarterConvo));

                        //Accepts a quest
                        questToAccept = testBondsCharacter.raphQuests[testBondsCharacter.raphLevel];
                        DialogueManager.Instance.OnCloseDialogue += AcceptQuest;
                    }
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

    void AcceptQuest()
    {
        QuestManager.Instance.AcceptQuest(questToAccept);
        questToAccept = null;
        DialogueManager.Instance.OnCloseDialogue -= AcceptQuest;
    }

    void ShowInteractable()
    {
        
    }

    void HideInteractable()
    { 
    
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
