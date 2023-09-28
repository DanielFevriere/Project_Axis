using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour
{
    bool inTalkingDistance = false;
    bool previouslyTalkedTo = false;

    [SerializeField] Conversation firstConvo;
    [SerializeField] Conversation repeatConvo;
    [SerializeField] Quest talkingQuest;

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

                //Shows a different conversation if you have a lready talked to it
                if (previouslyTalkedTo)
                {
                    StartCoroutine(DialogueManager.Instance.ShowConversation(repeatConvo));
                    QuestManager.Instance.FinishQuest(talkingQuest);
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
