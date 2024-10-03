using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ShopKeeperNPC : MonoBehaviour
{
    public ShopKeeper shopKeeper;
    public bool inTalkingDistance;
    public GameObject interactableUI;
    public GameObject npcOptions;


    // Start is called before the first frame update
    void Awake()
    {
        ShowInteractable();
        HideNPCOptions();
    }

    // Update is called once per frame
    void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //If you are in talking distance
        if (inTalkingDistance && GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
        {
            GameManager.Instance.ChangeState(GameState.Freeze);
            HideInteractable();
            ShowNPCOptions();
        }
    }

    public void Talk()
    {
        HideNPCOptions();
        StartCoroutine(DialogueManager.Instance.ShowConversation(shopKeeper.normalConvo));
        DialogueManager.Instance.OnCloseDialogue -= ShowInteractable;
    }

    public void Shop()
    {
        GameManager.Instance.SetCurrentCamera(GameManager.Instance.BondsMenuCamera);
        HideNPCOptions();
        UiManager.Instance.shopMenu.ToggleVisibility();
    }

    public void ExitTalk()
    {
        ShowInteractable();
        HideNPCOptions();
        GameManager.Instance.ChangeState(GameState.FreeRoam);
    }

    public void ShowInteractable()
    {
        interactableUI.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
        DialogueManager.Instance.OnCloseDialogue -= ShowInteractable;
    }

    public void ShowNPCOptions()
    {
        GameManager.Instance.SetCurrentCamera(GameManager.Instance.ZoomedCamera);
        npcOptions.SetActive(true);
    }
    public void HideInteractable()
    {
        interactableUI.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
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
