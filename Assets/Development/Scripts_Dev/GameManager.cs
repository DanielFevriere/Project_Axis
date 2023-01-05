using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//States the game can be in
public enum GameState { FreeRoam, Battle, Dialog }

public class GameManager : MonoBehaviour
{
#region Global static reference
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<GameManager>();
                return instance;
            }
        }
    }
    #endregion

    [SerializeField] Controller currentController;
    [SerializeField] Camera worldCamera;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    public GameObject partyLeader;
    public List<GameObject> partyMembers;

    GameState state;

    private void Awake()
    {
        state = GameState.FreeRoam;
    }

    private void Update()
    {
        //The first party member in the list of party members is the current leader
        partyLeader = partyMembers[0];
        currentController = partyLeader.GetComponent<Controller>();
        partyLeader.GetComponent<Controller>().enabled = true;
        partyLeader.GetComponent<AIFollow>().enabled = false;


        if (state == GameState.FreeRoam)
        {
            //Makes sure each non leader party member is following the party leader
            foreach(GameObject p in partyMembers)
            {
                if(p != partyLeader)
                {
                    p.GetComponent<Controller>().enabled = false;
                    p.GetComponent<AIFollow>().enabled = true;
                }
            }


            worldCamera.GetComponent<DitherObjectsBetweenCamAndPlayer>().target = partyLeader.transform;
            virtualCamera.Follow = partyLeader.transform;
            currentController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {

        }
        else if (state == GameState.Dialog)
        {

        }
    }

    //Swaps control between party members
    public void SwapCharacter()
    {
        //I know this is the lazy way im sorry for my shit code anthony
        partyMembers[0] = partyMembers[1];
        partyMembers[1] = partyMembers[2];
        partyMembers[2] = partyLeader;
    }
}
