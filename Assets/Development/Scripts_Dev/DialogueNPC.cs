using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour
{
    bool previouslyTalkedTo = false;

    [SerializeField] Conversation firstConvo;
    [SerializeField] Conversation repeatConvo;
    
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if (GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasReleasedThisFrame)
        {
            GameManager.Instance.ChangeState(GameState.Dialogue);

            //Shows a differnt conversation if you have already talked to it
            StartCoroutine(DialogueManager.Instance.ShowConversation(previouslyTalkedTo ? repeatConvo : firstConvo));
            previouslyTalkedTo = true;
        }

    }
}
