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

    [SerializeField] Controller playerController;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Awake()
    {
        state = GameState.FreeRoam;
    }

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {

        }
        else if (state == GameState.Dialog)
        {

        }
    }
}
