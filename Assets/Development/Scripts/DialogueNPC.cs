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

    private void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if(inTalkingDistance && GameManager.Instance.CurrentState != GameState.Dialogue)
        {
            if (GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
            {
                GameManager.Instance.ChangeState(GameState.Dialogue);

                //Shows a different conversation if you have already talked to it
                if (previouslyTalkedTo)
                {
                    StartCoroutine(DialogueManager.Instance.ShowConversation(repeatConvo));
                }
                else
                {
                    StartCoroutine(DialogueManager.Instance.ShowConversation(firstConvo));
                    previouslyTalkedTo = true;
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
