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

    SupportCharacter cubeSupportCharacter;

    private void Awake()
    {
        //Finds the support character in the manager
        for (int i = 0; i < SupportManager.Instance.supportCharacters.Count; i++)
        {
            if(characterName == SupportManager.Instance.supportCharacters[i].characterName)
            {
                cubeSupportCharacter = SupportManager.Instance.supportCharacters[i];
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
                        cubeSupportCharacter.SupportLevelUp(1);
                        StartCoroutine(DialogueManager.Instance.ShowConversation(cubeSupportCharacter.gabeSupportConvos[cubeSupportCharacter.gabeLevel - 1]));
                    }
                    else if(GameManager.Instance.partyLeader.name == "Michael")
                    {
                        cubeSupportCharacter.SupportLevelUp(2);
                        StartCoroutine(DialogueManager.Instance.ShowConversation(cubeSupportCharacter.mikeSupportConvos[cubeSupportCharacter.mikeLevel - 1]));
                    }
                    else if(GameManager.Instance.partyLeader.name == "Raphael")
                    {
                        cubeSupportCharacter.SupportLevelUp(3);
                        StartCoroutine(DialogueManager.Instance.ShowConversation(cubeSupportCharacter.raphSupportConvos[cubeSupportCharacter.raphLevel - 1]));
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
