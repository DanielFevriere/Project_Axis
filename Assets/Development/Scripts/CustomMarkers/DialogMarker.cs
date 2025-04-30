using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogMarker : Marker, INotification
{
    [SerializeField] Conversation convo;
    public UnityEvent dialogActions;

    public PropertyName id => new PropertyName();

    //You can probably add stuff like custom color etc here too if you figure out how
    public Conversation Convo => convo;

    [Space(20)]
    [SerializeField] bool retroactive = false;
    [SerializeField] bool emitOnce = false;

    public NotificationFlags flags =>
        (retroactive ? NotificationFlags.Retroactive : default) |
        (emitOnce ? NotificationFlags.TriggerOnce : default);
}
