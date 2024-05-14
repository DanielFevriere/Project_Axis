using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransferCube : MonoBehaviour
{
    [SerializeField] string characterName;
    bool inTalkingDistance = false;

    [SerializeField] Conversation npcConvo;
    public string promptText;
    public string choice1Text;
    public string choice2Text;

    public Transform teleportLocation;

    private void Awake()
    {
    }

    private void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if (inTalkingDistance && GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
        {
            //Changes the games state to dialogue
            GameManager.Instance.ChangeState(GameState.Dialogue);

            StartCoroutine(DialogueManager.Instance.ShowConversation(npcConvo));
            DialogueManager.Instance.OnCloseDialogue += BeginChoosing;
        }
    }

    public void BeginChoosing()
    {
        //Changes the games state to dialogue
        GameManager.Instance.ChangeState(GameState.Dialogue);

        DialogueManager.Instance.OnCloseDialogue -= BeginChoosing;
        DialogueManager.Instance.OnCloseDialogue += ChoiceMade;
        DialogueManager.Instance.StartPrompt(promptText, choice1Text, choice2Text);
    }

    public void ChoiceMade()
    {
        DialogueManager.Instance.OnCloseDialogue -= ChoiceMade;

        if(DialogueManager.Instance.dialogueChoice == 1)
        {
            StartCoroutine(TeleportCoroutine(GameManager.Instance.partyLeader.GetComponent<Controller>()));
        }
    }

    IEnumerator TeleportCoroutine(Controller PlayerController)
    {
        GameManager.Instance.ChangeState(GameState.Freeze);
        PlayerController.gameObject.transform.position = teleportLocation.position;
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.ChangeState(GameState.FreeRoam);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Instance.partyLeader)
        {
            inTalkingDistance = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == GameManager.Instance.partyLeader)
        {
            inTalkingDistance = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inTalkingDistance = false;
    }
}