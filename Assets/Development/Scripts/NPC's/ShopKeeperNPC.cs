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

    public Conversation talkConvo;
    public Conversation shopConvo;
    public Conversation buyConvo;
    public Conversation failBuyConvo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //If you are in talking distance,
        if (inTalkingDistance && GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
        {
            GameManager.Instance.ChangeState(GameState.Freeze);
            HideInteractable();
            ShowNPCOptions();
        }
    }

    public void Talk()
    {

    }

    public void Shop()
    {
        //Connect this keeper to the shop Menu (which at this point has almostbecome a manager)
        UiManager.Instance.shopMenu.ToggleVisibility();
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
}
