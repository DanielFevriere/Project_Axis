using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeDialogueNPC : MonoBehaviour
{
    [SerializeField] string characterName;
    bool inTalkingDistance = false;
    bool previouslyTalkedTo = false;

    [SerializeField] Conversation firstConvo;
    [SerializeField] Conversation repeatConvo;
    [SerializeField] Quest talkingQuest;

    BondsCharacter cubeBondsCharacter;

    private void Awake()
    {
        //Finds the support character in the manager
        for (int i = 0; i < BondsManager.Instance.bondsCharacters.Count; i++)
        {
            if(characterName == BondsManager.Instance.bondsCharacters[i].characterName)
            {
                cubeBondsCharacter = BondsManager.Instance.bondsCharacters[i];
            }
        }
    }

    private void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if(inTalkingDistance && GameManager.Instance.CurrentState != GameState.Dialogue)
        {
            if (GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
            {
                //Changes the games state to dialogue
                GameManager.Instance.ChangeState(GameState.Dialogue);

                //Shows a different conversation if you have already talked to it
                if (previouslyTalkedTo)
                {
                    //StartCoroutine(DialogueManager.Instance.ShowConversation(repeatConvo));
                    QuestManager.Instance.FinishQuest(talkingQuest);

                    //Depending on the party leader, level up the corresponding support level and start the proper support conversation
                    if(GameManager.Instance.partyLeader.name == "Gabriel")
                    {
                        BondsManager.Instance.BondLevelUp(cubeBondsCharacter, 1);
                        StartCoroutine(DialogueManager.Instance.ShowConversation(cubeBondsCharacter.gabeBondsConvos[cubeBondsCharacter.gabeLevel - 1]));
                    }
                    else if(GameManager.Instance.partyLeader.name == "Michael")
                    {
                        BondsManager.Instance.BondLevelUp(cubeBondsCharacter, 2);
                        StartCoroutine(DialogueManager.Instance.ShowConversation(cubeBondsCharacter.mikeBondsConvos[cubeBondsCharacter.mikeLevel - 1]));
                    }
                    else if(GameManager.Instance.partyLeader.name == "Raphael")
                    {
                        BondsManager.Instance.BondLevelUp(cubeBondsCharacter, 3);
                        StartCoroutine(DialogueManager.Instance.ShowConversation(cubeBondsCharacter.raphBondsConvos[cubeBondsCharacter.raphLevel - 1]));
                    }
                }
                else
                {
                    StartCoroutine(DialogueManager.Instance.ShowConversation(firstConvo));
                    previouslyTalkedTo = true;
                    QuestManager.Instance.AcceptQuest(talkingQuest);
                }

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        inTalkingDistance = true;

    }

    void OnTriggerStay(Collider other)
    {
        inTalkingDistance = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inTalkingDistance = false;
    }
}
