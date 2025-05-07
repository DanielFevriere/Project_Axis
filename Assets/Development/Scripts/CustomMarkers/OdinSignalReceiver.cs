using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[Serializable]
public class SignalEventPair
{
    [Searchable]
    [ValueDropdown(nameof(GetAllSignals))]
    public SignalAsset signal;

    public UnityEvent onSignalReceived;

    private static IEnumerable<SignalAsset> GetAllSignals()
    {
        return Resources.FindObjectsOfTypeAll<SignalAsset>();
    }
}

public class OdinSignalReceiver : MonoBehaviour, INotificationReceiver
{
    [ListDrawerSettings(Expanded = true)]
    public List<SignalEventPair> signalEvents = new List<SignalEventPair>();

    private Dictionary<SignalAsset, UnityEvent> signalLookup;

    private void Awake()
    {
        signalLookup = new Dictionary<SignalAsset, UnityEvent>();
        foreach (var pair in signalEvents)
        {
            if (pair.signal != null && !signalLookup.ContainsKey(pair.signal))
            {
                signalLookup[pair.signal] = pair.onSignalReceived;
            }
        }
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is SignalEmitter emitter && signalLookup != null)
        {
            if (signalLookup.TryGetValue(emitter.asset as SignalAsset, out var unityEvent))
            {
                unityEvent?.Invoke();
                Debug.Log($"[OdinSignalReceiver] Signal '{emitter.asset.name}' received and invoked.");
            }
        }
    }
}
