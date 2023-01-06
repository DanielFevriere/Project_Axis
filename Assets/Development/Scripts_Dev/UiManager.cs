using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    #region Global static reference
    private static UiManager instance;
    public static UiManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<UiManager>();
                return instance;
            }
        }
    }
    #endregion

    #region Sub components
    [Header("Debugger")]
    public UiDebug Debug;
    #endregion

    private void Start()
    {
        GameManager.Instance.OnStateChange += DEBUG_SetCurrentState;
        GameManager.Instance.OnPartyLeaderChange += DEBUG_SetCurrentPartyLeader;

        // first run
        DEBUG_SetCurrentState();
        DEBUG_SetCurrentPartyLeader();
    }

    public void DEBUG_SetCurrentState()
    {
        Debug.gameStateText.text = "Game State: " + GameManager.Instance.CurrentState.ToString();
    }

    public void DEBUG_SetCurrentPartyLeader()
    {
        GameManager.Instance.SetPartyLeader();
        if (GameManager.Instance.partyLeader != null)
        {
            Debug.partyLeaderText.text = "Party Leader: " + GameManager.Instance.partyLeader.name;
            Debug.partyLeaderText.color = Color.white;
            return;
        }

        Debug.partyLeaderText.text = "Party Leader NOT FOUND!";
        Debug.partyLeaderText.color = Color.red;
    }
}

[System.Serializable]
public class UiDebug
{
    public TMPro.TMP_Text gameStateText;
    public TMPro.TMP_Text partyLeaderText;
}
