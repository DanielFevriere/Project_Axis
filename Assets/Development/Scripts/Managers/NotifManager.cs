using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotifManager : MonoBehaviour
{

    #region Global static reference
    private static NotifManager instance;
    public static NotifManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<NotifManager>();
                return instance;
            }
        }
    }
    #endregion

    [SerializeField] TMP_Text currentNotifText;

    public bool playingNotif;
    public float displayTime;
    public float displayTimer;

    [SerializeField] List<string> notifQueue;
    [SerializeField] GameObject notifObject;
    // Start is called before the first frame update
    void Start()
    {
        displayTimer = displayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(playingNotif)
        {
            displayTimer -= Time.deltaTime;
        }

        if(displayTimer <= 0)
        {
            playingNotif = false;
            PlayNextNotif();
            if(!playingNotif)
            {
                HideNotif();
            }

            displayTimer = displayTime;
        }
    }

    /// <summary>
    /// This function is supposed to queue something to get displayed on the notification UI
    /// </summary>
    public void QueueNotif(RewardGiver reward)
    {
        //Goes through all the reward lists in the reward giver and adds each individual item to the queue

        if(reward.veniReward != 0)
        {
            notifQueue.Add(reward.veniReward.ToString() + " Veni Acquired!");
        }

        if(reward.boosterItemRewards.Count != 0)
        {
            foreach(BoosterItem b in reward.boosterItemRewards)
            {
                notifQueue.Add("'" + b.itemName + "'" + " Acquired!");
            }
        }

        if(reward.consumableItemRewards.Count != 0)
        {
            foreach (ConsumableItem c in reward.consumableItemRewards)
            {
                notifQueue.Add("'" + c.itemName + "'" + " Acquired!");
            }
        }

        if(reward.keyItemRewards.Count != 0)
        {
            foreach (KeyItem k in reward.keyItemRewards)
            {
                notifQueue.Add("'" + k.itemName + "'" + " Acquired!");
            }
        }

        if(reward.nodeRewards.Count != 0)
        {
            foreach(Node n in reward.nodeRewards)
            {
                notifQueue.Add("'" + n.nodeName + "'" + " Acquired!");
            }
        }

        if(!playingNotif)
        {
            StartNotif();
        }
    }

    public void StartNotif()
    {
        if (notifQueue.Count == 0)
        {
            return;
        }

        if (!playingNotif)
        {
            ShowNotif();
        }

        currentNotifText.text = notifQueue[0];
        playingNotif = true;
    }

    public void PlayNextNotif()
    {
        if(notifQueue.Count != 0)
        {
            notifQueue.Remove(notifQueue[0]);
        }

        if(notifQueue.Count == 0)
        {
            HideNotif();
            return;
        }

        if(!playingNotif)
        {
            ShowNotif();
        }

        currentNotifText.text = notifQueue[0];
        playingNotif = true;
    }

    public void ShowNotif()
    {
        notifObject.SetActive(true);
    }

    public void HideNotif()
    {
        notifObject.SetActive(false);
    }
}
