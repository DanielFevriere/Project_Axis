using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Collectible : MonoBehaviour
{
    public string collectibleID;
    public List<Transform> waypoints;
    public GameObject parent;

    void Awake()
    {
        Teleport();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject == GameManager.Instance.partyLeader)
        {
            //This is a bandaid fix. I have an unsubscribe before the
            //subscribe because it's called twice via trigger enter because
            //the player contains a capsule collider and a player controller
            //that is on the same gameobject which calls it twice.
            GameManager.Instance.OnInteract -= Collect; 
            GameManager.Instance.OnInteract += Collect;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject == GameManager.Instance.partyLeader)
        {
            GameManager.Instance.OnInteract -= Collect;
        }
    }


    public void Collect()
    {
        //Cycles through the active quests
        for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
        {
            //Cycles through the conditions of active quests
            for (int j = 0; j < QuestManager.Instance.activeQuests[i].conditions.Count; j++)
            {
                //If the collectible ID is the same as the quest condition ID,
                if (collectibleID == QuestManager.Instance.activeQuests[i].conditions[j].ConditionID)
                {
                    //Gives progress
                    QuestManager.Instance.activeQuests[i].conditions[j].GiveProgress(1);
                    Teleport();
                }
            }
        }
    }

    public void Teleport()
    {
        parent.transform.position = waypoints[Random.Range(0, waypoints.Count - 1)].position;
    }
}
