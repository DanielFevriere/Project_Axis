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
        HideNPCOptions();
        GameManager.Instance.OnStateChange += ShowInteractable;
        GameManager.Instance.OnStateChange += HideInteractable;
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
    }

    public void Shop()
    {
        GameManager.Instance.SetCurrentCamera(GameManager.Instance.BondsMenuCamera);
        HideNPCOptions();
        UiManager.Instance.shopMenu.ToggleVisibility();
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
        if (GameManager.Instance.CurrentState != GameState.FreeRoam)
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
