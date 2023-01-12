using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    
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

            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
        }

    }
}
