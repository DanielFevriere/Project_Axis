using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class DialogReciever : MonoBehaviour, INotificationReceiver
{
    [SerializeField] DialogueManager dialogManager;
    [SerializeField] CutsceneManager cutsceneManager;


    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if(notification is DialogMarker dialogMarker && dialogManager != null)
        {
            Conversation newConvo = new Conversation
            {
                Dialogues = dialogMarker.Convo.Dialogues
            };

            dialogManager.StartConvo(newConvo);
            cutsceneManager.PauseCutscene();
            dialogMarker.dialogActions.Invoke();
        }
    }
}
