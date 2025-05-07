using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneCommandReceiver : MonoBehaviour, INotificationReceiver
{
    public DialogueManager dialogueManager;
    public CutsceneManager cutsceneManager;
    public AnimatorEntry[] animators;

    //public Dictionary<string, Animator> sceneAnimators = new (); // or via inspector

    [System.Serializable]
    public class AnimatorEntry
    {
        public string key; // e.g. "door", "npc01"
        public Animator animator;
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogMarker marker)
        {
            cutsceneManager.PauseCutscene();
            dialogueManager.StartConvo(marker.convo);
            //Remember that you are essentially creating a very very very similar version
            //of the signal reciever. The only difference is that you need a list of animators
            //and for each animator referenced, there needs to be a corresponding string of the 
            //animation you would like to play during a cutscene
            //so if you grab the animator for gabriel, you need to have a corresponding string
            //I wonder if theres a data type that holds both a string and wait- just create one
            //Wait that wont work i think
            //HOWEVER,
            //What i want is within the inspector, upon a certain
        }
    }

    void ParseAndExecuteCommand(string command)
    {
        var split = command.Split(':');
        if (split.Length < 2) return;

        var keyword = split[0];
        var argument = split[1];

        /*
        switch (keyword)
        {
            case "playAnim":
                if (sceneAnimators.TryGetValue(argument, out var animator))
                    animator.Play(argument);
                break;
            case "vfx":
                VFXManager.Instance.PlayEffect(argument);
                break;
            case "activate":
                GameObject.Find(argument)?.SetActive(true);
                break;
                // etc...
        }
        */
    }
}